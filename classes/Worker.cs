using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

using csharp_import_console.models;
using csharp_import_console.models.metadata;

namespace csharp_import_console.classes
{
    public class Worker
    {
        private readonly IConfiguration configuration;

        public Worker(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private async Task<HttpClient> CreateRestClient()
        {

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Practical test");

            var tokenRequest = new TokenRequest
            {
                Username = configuration.GetValue<string>("Username"),
                Password = configuration.GetValue<string>("Password")
            };

            var url = configuration.GetValue<string>("BaseUrl") + configuration.GetValue<string>("AccessTokenEndpoint");

            var response = await httpClient.PostAsync(url,
                new StringContent(JsonSerializer.Serialize(tokenRequest), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new ArgumentException($"Url: {url}, Status code: {response.StatusCode}, Reason phrase: {response.ReasonPhrase}");

            var tokenResponse = await JsonSerializer.DeserializeAsync<TokenResponse>(response.Content.ReadAsStream());

            if (tokenResponse == null)
                throw new ArgumentNullException(nameof(tokenResponse));

            if (string.IsNullOrEmpty(tokenResponse.AccessToken))
                throw new ArgumentNullException(tokenResponse.AccessToken);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

            return httpClient;
        }

        private async Task<Tuple<ExportResult, MetadataResponse>> ExportDataFromWebAPI()
        {
            var httpClient = await CreateRestClient();

            var exportRequest = new ExportRequest
            {
                Filters = new List<Filter> { new Filter { QuestionCode = string.Empty, Operator = string.Empty, Value = string.Empty } },
                QuestionCodes = new List<string> { string.Empty },
                SortCriteria = new List<SortCriteria> { new SortCriteria { QuestionCode = string.Empty, SortOrder = "ASC" } },
                ExportStackedData = true,
                IncludeGroupedAnswers = false,
                IncludeComputeVariables = false,
                IncludeInputVariables = false,
                IncludeMergedVariables = false,
                OutputEmptyStrings = false,
                PageIndex = 1,
                PageSize = 1000000,
                StackedDataBatches = new List<string> { string.Empty },
                DataExportFormat = "JSON"
            };

            Console.WriteLine("Creating export request ...");

            var url = configuration.GetValue<string>("BaseUrl") + configuration.GetValue<string>("ExportRequestEndpoint");

            var response = await httpClient.PostAsync(url,
                new StringContent(JsonSerializer.Serialize(exportRequest), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new ArgumentException($"Url: {url}, Status code: {response.StatusCode}, Reason phrase: {response.ReasonPhrase}");

            var exportResponse = await JsonSerializer.DeserializeAsync<ExportResponse>(response.Content.ReadAsStream());

            if (exportResponse == null)
                throw new ArgumentNullException(nameof(exportResponse));

            if (exportResponse.Status == null)
                throw new ArgumentNullException(nameof(exportResponse.Status));

            if (string.IsNullOrEmpty(exportResponse.Status.StatusCode))
                throw new ArgumentNullException(nameof(exportResponse.Status.StatusCode));

            if (exportResponse.Status.StatusCode != "SUCCESS")
                throw new ArgumentException($"Url: {url}, Status code: {exportResponse.Status.StatusCode}, Message: {exportResponse.Status.Messages?.FirstOrDefault()} ");

            if (exportResponse.Data == null)
                throw new ArgumentNullException(nameof(exportResponse.Data));

            if (exportResponse.Data.RequestId == null)
                throw new ArgumentNullException(nameof(exportResponse.Data.RequestId));

            url = url + configuration.GetValue<string>("CheckStatusWithRequestParameter") + exportResponse.Data.RequestId;

            do
            {
                Console.Write("Checking status of request ...");

                response = await httpClient.PostAsync(url, null);

                if (!response.IsSuccessStatusCode)
                    throw new ArgumentException($"Url: {url}, Status code: {response.StatusCode}, Reason phrase: {response.ReasonPhrase}");

                exportResponse = await JsonSerializer.DeserializeAsync<ExportResponse>(response.Content.ReadAsStream());

                if (exportResponse == null)
                    throw new ArgumentNullException(nameof(exportResponse));

                if (exportResponse.Status == null)
                    throw new ArgumentNullException(nameof(exportResponse.Status));

                if (string.IsNullOrEmpty(exportResponse.Status.StatusCode))
                    throw new ArgumentNullException(nameof(exportResponse.Status.StatusCode));

                if (exportResponse.Status.StatusCode != "SUCCESS")
                    throw new ArgumentException($"{url}, Status code: {exportResponse.Status.StatusCode}, Message: {exportResponse.Status.Messages?.FirstOrDefault()} ");

                if (exportResponse.Data == null)
                    throw new ArgumentNullException(nameof(exportResponse.Data));

                if (string.IsNullOrEmpty(exportResponse.Data.Status))
                    throw new ArgumentNullException(nameof(exportResponse.Data.Status));

                Console.WriteLine($" {exportResponse.Data.Status}");

                if (exportResponse.Data.Status != "COMPLETED")
                    await Task.Delay(5000);
            }
            while (exportResponse.Data.Status != "COMPLETED");

            if (exportResponse.Data.Files == null)
                throw new ArgumentNullException(nameof(exportResponse.Data.Files));

            if (exportResponse.Data.Files.Count == 0)
                throw new ArgumentException(nameof(exportResponse.Data.Files));

            var streamTask = httpClient.GetStreamAsync(exportResponse.Data.Files.FirstOrDefault());

            var exportResult = await JsonSerializer.DeserializeAsync<ExportResult>(await streamTask);

            if (exportResult == null)
                throw new ArgumentNullException(nameof(exportResult));

            url = configuration.GetValue<string>("BaseUrl") + configuration.GetValue<string>("MetadataEndpoint");

            var metadataRequest = new MetadataRequest
            {
                IncludeStackedVariables = true,
                IncludeComputeVariables = false,
                IncludeGroupedAnswers = false,
                IncludeInputVariables = false,
                IncludeMergedVariables = false
            };

            Console.WriteLine("Extracting metadata ...");

            response = await httpClient.PostAsync(url,
                new StringContent(JsonSerializer.Serialize(metadataRequest), Encoding.UTF8, "application/json"));

            var metadataResponse = await JsonSerializer.DeserializeAsync<MetadataResponse>(response.Content.ReadAsStream());

            if (metadataResponse == null)
                throw new ArgumentNullException(nameof(metadataResponse));

            return new Tuple<ExportResult, MetadataResponse>(exportResult, metadataResponse);
        }

        public async void DoWork()
        {
            await ExportDataFromWebAPI()
                .ContinueWith(task =>
               {
                   if (task.IsFaulted)
                   {
                       if (task.Exception != null)
                           Console.WriteLine(task.Exception.GetBaseException().Message);
                   }
                   else if (task.IsCompletedSuccessfully)
                   {
                       Console.WriteLine("Preparing statistical data ...");
                       Console.WriteLine();

                       Dictionary<string, int> cityComplaints = Statistics.CalculateCityComplaintsInTotal(task.Result.Item1, task.Result.Item2);

                       Console.WriteLine("1. Total number of complaints per respondent per city:");
                       foreach (var city in cityComplaints)
                       {
                           Console.WriteLine($"- {city.Key}: {city.Value}");
                       }

                       var cityWithMostComplaintsAboutPillow = Statistics.GetCityWithTheMostComplaintsAboutPillow(task.Result.Item1, task.Result.Item2);
                       Console.WriteLine($"2. City with the most complaints about pillow: {cityWithMostComplaintsAboutPillow}");

                       var managerWithLeastNumberOfComplaints = Statistics.GetHotelManagerWithLeastComplaintsInTotal(task.Result.Item1, task.Result.Item2);
                       Console.WriteLine($"3. Hotel manager {managerWithLeastNumberOfComplaints.Item1} with least number of {managerWithLeastNumberOfComplaints.Item2} complaints");

                       var top10ProblemAreasWithMostComplaints = Statistics.CalculateTop10ProblemAreasWithMostComplaints(task.Result.Item1);
                       Console.WriteLine("4. Top 10 problem areas with most complaints:");
                       foreach (var complaint in top10ProblemAreasWithMostComplaints)
                       {
                           Console.WriteLine($"- {complaint.Key}: {complaint.Value}");
                       }
                   }
               });
        }
    }
}