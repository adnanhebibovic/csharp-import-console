using csharp_import_console.models;
using csharp_import_console.models.metadata;

namespace csharp_import_console.classes
{
    public class Statistics
    {
        public static Dictionary<string, int> CalculateCityComplaintsInTotal(ExportResult export, MetadataResponse metadata)
        {
            Dictionary<string, int> cityComplaints = new Dictionary<string, int>();

            var answerAlternatives = metadata.Data?.FirstOrDefault(data => data.Code == "City")?.AnswerAlternatives;
            if (answerAlternatives != null)
            {
                foreach (var answer in answerAlternatives)
                {
                    if (answer != null && answer.Code != null && !string.IsNullOrEmpty(answer.Text))
                    {
                        if (export.CaseData != null)
                        {
                            foreach (var complaint in export.CaseData.Where(caseData => caseData.City.Contains(answer.Code)))
                            {
                                if (!cityComplaints.ContainsKey(answer.Text))
                                    cityComplaints.Add(answer.Text, 0);

                                if (Int32.TryParse(complaint.ComplaintsPerRespondent, out int result))
                                {
                                    cityComplaints[answer.Text] += result;
                                }
                            }
                        }
                    }
                }
            }

            return cityComplaints;
        }

        public static string GetCityWithTheMostComplaintsAboutPillow(ExportResult export, MetadataResponse metadata)
        {
            Dictionary<string, int> cityComplaints = new Dictionary<string, int>();

            var answerAlternatives = metadata.Data?.FirstOrDefault(data => data.Code == "City")?.AnswerAlternatives;
            if (answerAlternatives != null)
            {
                foreach (var answer in answerAlternatives)
                {
                    if (answer != null && answer.Code != null && !string.IsNullOrEmpty(answer.Text))
                    {
                        if (export.CaseData != null)
                        {
                            foreach (var complaint in export.CaseData.Where(caseData => caseData.City.Contains(answer.Code)))
                            {
                                if (!cityComplaints.ContainsKey(answer.Text))
                                    cityComplaints.Add(answer.Text, 0);

                                if (Int32.TryParse(complaint.Pillows, out int result))
                                {
                                    cityComplaints[answer.Text] += result;
                                }
                            }
                        }
                    }
                }
            }

            return cityComplaints.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
        }

        public static Tuple<string, int> GetHotelManagerWithLeastComplaintsInTotal(ExportResult export, MetadataResponse metadata)
        {
            Dictionary<string, int> managerComplaints = new Dictionary<string, int>();

            var answerAlternatives = metadata.Data?.FirstOrDefault(data => data.Code == "Person")?.AnswerAlternatives;
            if (answerAlternatives != null)
            {
                foreach (var answer in answerAlternatives)
                {
                    if (answer != null && answer.Code != null && !string.IsNullOrEmpty(answer.Text))
                    {
                        if (export.CaseData != null)
                        {
                            foreach (var complaint in export.CaseData.Where(caseData => caseData.Person.Contains(answer.Code)))
                            {
                                if (!managerComplaints.ContainsKey(answer.Text))
                                    managerComplaints.Add(answer.Text, 0);

                                if (Int32.TryParse(complaint.ComplaintsPerRespondent, out int result))
                                {
                                    managerComplaints[answer.Text] += result;
                                }
                            }
                        }
                    }
                }
            }

            var aggregate = managerComplaints.Aggregate((x, y) => x.Value < y.Value ? x : y);

            return new Tuple<string, int>(aggregate.Key, aggregate.Value);
        }

        public static IEnumerable<KeyValuePair<string, int>> CalculateTop10ProblemAreasWithMostComplaints(ExportResult export)
        {
            Dictionary<string, int> areaComplaints = new Dictionary<string, int>();

            if (export.CaseData != null)
            {
                foreach (var complaint in export.CaseData)
                {
                    int result = 0;

                    if (!areaComplaints.ContainsKey(nameof(complaint.BathroomFloor)))
                        areaComplaints.Add(nameof(complaint.BathroomFloor), 0);

                    if (Int32.TryParse(complaint.BathroomFloor, out result))
                        areaComplaints[nameof(complaint.BathroomFloor)] += result;
                    
                    if (!areaComplaints.ContainsKey(nameof(complaint.BedsideTables)))
                        areaComplaints.Add(nameof(complaint.BedsideTables), 0);

                    if (Int32.TryParse(complaint.BedsideTables, out result))
                        areaComplaints[nameof(complaint.BedsideTables)] += result;
                    
                    if (!areaComplaints.ContainsKey(nameof(complaint.Blankets)))
                        areaComplaints.Add(nameof(complaint.Blankets), 0);

                    if (Int32.TryParse(complaint.Blankets, out result))
                        areaComplaints[nameof(complaint.Blankets)] += result;
                    
                    if (!areaComplaints.ContainsKey(nameof(complaint.Carpet)))
                        areaComplaints.Add(nameof(complaint.Carpet), 0);

                    if (Int32.TryParse(complaint.Carpet, out result))
                        areaComplaints[nameof(complaint.Carpet)] += result;
                    
                    if (!areaComplaints.ContainsKey(nameof(complaint.Entertainment)))
                        areaComplaints.Add(nameof(complaint.Entertainment), 0);

                    if (Int32.TryParse(complaint.Entertainment, out result))
                        areaComplaints[nameof(complaint.Entertainment)] += result;

                    if (!areaComplaints.ContainsKey(nameof(complaint.HygieneProducts)))
                        areaComplaints.Add(nameof(complaint.HygieneProducts), 0);

                    if (Int32.TryParse(complaint.HygieneProducts, out result))
                        areaComplaints[nameof(complaint.HygieneProducts)] += result;

                    if (!areaComplaints.ContainsKey(nameof(complaint.Minibar)))
                        areaComplaints.Add(nameof(complaint.Minibar), 0);

                    if (Int32.TryParse(complaint.Minibar, out result))
                        areaComplaints[nameof(complaint.Minibar)] += result;

                    if (!areaComplaints.ContainsKey(nameof(complaint.Mirror)))
                        areaComplaints.Add(nameof(complaint.Mirror), 0);

                    if (Int32.TryParse(complaint.Mirror, out result))
                        areaComplaints[nameof(complaint.Mirror)] += result;

                    if (!areaComplaints.ContainsKey(nameof(complaint.Pillows)))
                        areaComplaints.Add(nameof(complaint.Pillows), 0);

                    if (Int32.TryParse(complaint.Pillows, out result))
                        areaComplaints[nameof(complaint.Pillows)] += result;

                    if (!areaComplaints.ContainsKey(nameof(complaint.SeatGroup)))
                        areaComplaints.Add(nameof(complaint.SeatGroup), 0);

                    if (Int32.TryParse(complaint.SeatGroup, out result))
                        areaComplaints[nameof(complaint.SeatGroup)] += result;

                    if (!areaComplaints.ContainsKey(nameof(complaint.Sideboard)))
                        areaComplaints.Add(nameof(complaint.Sideboard), 0);

                    if (Int32.TryParse(complaint.Sideboard, out result))
                        areaComplaints[nameof(complaint.Sideboard)] += result;

                    if (!areaComplaints.ContainsKey(nameof(complaint.TowelsAndBathrobes)))
                        areaComplaints.Add(nameof(complaint.TowelsAndBathrobes), 0);

                    if (Int32.TryParse(complaint.TowelsAndBathrobes, out result))
                        areaComplaints[nameof(complaint.TowelsAndBathrobes)] += result;

                    if (!areaComplaints.ContainsKey(nameof(complaint.Washbasin)))
                        areaComplaints.Add(nameof(complaint.Washbasin), 0);

                    if (Int32.TryParse(complaint.Washbasin, out result))
                        areaComplaints[nameof(complaint.Washbasin)] += result;
                }
            }

            return areaComplaints.OrderByDescending(kvp => kvp.Value).Take(10);
        }
    }
}