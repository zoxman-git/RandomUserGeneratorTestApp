using RandomUserGeneratorTestApp.BusinessLogic.Services.Interfaces;
using RandomUserGeneratorTestApp.BusinessLogic.ViewModels.Requests;
using RandomUserGeneratorTestApp.BusinessLogic.ViewModels.Responses;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RandomUserGeneratorTestApp.BusinessLogic.Services
{
    public class StatisticsService : IStatisticsService
    {
        /// <summary>
        /// Get summary analysis information for N random users
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public string[] GetStatsSummaryText(UserRequest[] users)
        {
            var stats = GetStats(users);

            var percentByGender = string.Join(", ", stats.UserByGenderPercents.Select(x => $"{x.Key}: {decimal.Round(x.Value, 2):N2}%"));
            var percentByState = string.Join(", ", stats.UserByStatePercentsTop10.Select(x => $"{x.Key}: {decimal.Round(x.Value, 2):N2}%"));
            var percentFemalesByState = string.Join(", ", stats.FemaleByStatePercentsTop10.Select(x => $"{x.Key}: {decimal.Round(x.Value, 2):N2}%"));
            var percentMalesByState = string.Join(", ", stats.MaleByStatePercentsTop10.Select(x => $"{x.Key}: {decimal.Round(x.Value, 2):N2}%"));
            var percentByAgeRange = string.Join(", ", stats.UserByAgeRangePercents.Select(x => $"{x.Key}: {decimal.Round(x.Value, 2):N2}%"));

            var result = new string[] {
                $"1. Percentage of gender in each category... {percentByGender}",
                $"2. Percentage of first names that start with A - M versus N-Z: {decimal.Round(stats.FirstNamesStartingWithAtoM, 2):N2}%",
                $"3. Percentage of last names that start with A - M versus N-Z: {decimal.Round(stats.LastNamesStartingWithAtoM, 2):N2}%",
                $"4. Percentage of people in each state, up to the top 10 most populous states... {percentByState}",
                $"5. Percentage of females in each state, up to the top 10 most populous states... {percentFemalesByState}",
                $"6. Percentage of males in each state, up to the top 10 most populous states... {percentMalesByState}",
                $"7. Percentage of people in the following age ranges... {percentByAgeRange}"
            };

            return result;
        }

        /// <summary>
        /// Get summary statistics data for N random users
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public StatsResponse GetStats(UserRequest[] users)
        {
            var stats = new StatsResponse();
            bool includeGroupInCount;

            // Calculate initial sums for each stat we wish to compute
            foreach (var user in users)
            {
                AddUserCountByGroup(user.Gender, stats.UserByGenderPercents);

                AddUserCountByFirstName(user, stats);

                AddUserCountByLastName(user, stats);

                AddUserCountByGroup(user.Location?.State, stats.UserByStatePercentsTop10);

                includeGroupInCount = user.Gender?.Trim().ToLower(CultureInfo.CurrentCulture) == "female";
                AddUserCountByGroup(user.Location?.State, stats.FemaleByStatePercentsTop10, includeGroupInCount);

                includeGroupInCount = user.Gender?.Trim().ToLower(CultureInfo.CurrentCulture) == "male";
                AddUserCountByGroup(user.Location?.State, stats.MaleByStatePercentsTop10, includeGroupInCount);

                AddUserCountByAgeRange(user.Dob?.Age, stats);
            }

            // Calculate percentages for aggregations we performed above
            CalculatePercentages(stats.UserByGenderPercents, users.Length);

            stats.FirstNamesStartingWithAtoM = (stats.FirstNamesStartingWithAtoM / users.Length) * 100;
            stats.LastNamesStartingWithAtoM = (stats.LastNamesStartingWithAtoM / users.Length) * 100;

            CalculatePercentages(stats.UserByStatePercentsTop10, users.Length);
            stats.UserByStatePercentsTop10 = RemoveNonTop10Groups(stats.UserByStatePercentsTop10);

            CalculatePercentages(stats.FemaleByStatePercentsTop10, users.Length);
            stats.FemaleByStatePercentsTop10 = RemoveNonTop10Groups(stats.FemaleByStatePercentsTop10);

            CalculatePercentages(stats.MaleByStatePercentsTop10, users.Length);
            stats.MaleByStatePercentsTop10 = RemoveNonTop10Groups(stats.MaleByStatePercentsTop10);

            CalculatePercentages(stats.UserByAgeRangePercents, users.Length);

            return stats;
        }

        private static void AddUserCountByGroup(string? group, Dictionary<string, decimal> groupsWithCounts, bool includeGroupInCount = true)
        {
            if (!includeGroupInCount)
                return;

            var groupKey = group?.Trim().ToLower(CultureInfo.CurrentCulture) ?? string.Empty;

            if (!groupsWithCounts.ContainsKey(groupKey))
            {
                groupsWithCounts.Add(groupKey, 1);
            }
            else
            {
                groupsWithCounts[groupKey]++;
            }
        }

        private static void AddUserCountByAgeRange(int? age, StatsResponse stats)
        {
            var userAgeRangeGroup = stats.UserByAgeRangePercents.FirstOrDefault(x =>
            {
                var userAgeMatchesRange = false;
                var lowerEndAgeRange = 0;
                var higherEndAgeRange = int.MaxValue;
                string[] ageRange;

                if (x.Key.Contains("+"))
                {
                    ageRange = x.Key.Split(" +");
                    
                    lowerEndAgeRange = int.Parse(ageRange[0]);
                }
                else 
                {
                    ageRange = x.Key.Split(" - ");

                    lowerEndAgeRange = int.Parse(ageRange[0]);
                    higherEndAgeRange = int.Parse(ageRange[1]);
                }

                if (age >= lowerEndAgeRange && age <= higherEndAgeRange) 
                {
                    userAgeMatchesRange = true;
                }

                return userAgeMatchesRange;
            });

            if (userAgeRangeGroup.Key != default(KeyValuePair<string, decimal>).Key) 
            {
                stats.UserByAgeRangePercents[userAgeRangeGroup.Key]++;
            }
        }

        private static void AddUserCountByFirstName(UserRequest user, StatsResponse stats)
        {
            var name = user.Name?.First?.Trim().ToLower(CultureInfo.CurrentCulture) ?? string.Empty;

            if (Regex.IsMatch(name, "[a-m].*"))
            {
                stats.FirstNamesStartingWithAtoM++;
            }
        }

        private static void AddUserCountByLastName(UserRequest user, StatsResponse stats)
        {
            var name = user.Name?.Last?.Trim().ToLower(CultureInfo.CurrentCulture) ?? string.Empty;

            if (Regex.IsMatch(name, "[a-m].*"))
            {
                stats.LastNamesStartingWithAtoM++;
            }
        }

        private static void CalculatePercentages(Dictionary<string, decimal> groupsWithCounts, decimal totalCount)
        {
            foreach (var group in groupsWithCounts.Keys) 
            {
                groupsWithCounts[group] = (groupsWithCounts[group] / totalCount) * 100;
            }
        }

        private static Dictionary<string, decimal> RemoveNonTop10Groups(Dictionary<string, decimal> groupsWithCounts)
        {
            var groupsWithCountsSorted = new Dictionary<string, decimal>();
            var groupsWithCountsList = groupsWithCounts.ToList();

            groupsWithCountsList = [.. groupsWithCountsList.OrderByDescending(x => x.Value).ThenBy(x => x.Key)];

            foreach (var group in groupsWithCountsList.Take(10))
            {
                groupsWithCountsSorted.Add(group.Key, group.Value);
            }

            return groupsWithCountsSorted;
        }
    }
}
