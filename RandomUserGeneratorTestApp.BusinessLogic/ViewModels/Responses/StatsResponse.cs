namespace RandomUserGeneratorTestApp.BusinessLogic.ViewModels.Responses
{
    public class StatsResponse
    {
        public StatsResponse()
        {
            UserByGenderPercents = [];
            FirstNamesStartingWithAtoM = 0;
            LastNamesStartingWithAtoM = 0;
            UserByStatePercentsTop10 = [];
            FemaleByStatePercentsTop10 = [];
            MaleByStatePercentsTop10 = [];
            UserByAgeRangePercents = new Dictionary<string, decimal>()
            {
                { "0 - 20", 0 },
                { "21 - 40", 0 },
                { "41 - 60", 0 },
                { "61 - 80", 0 },
                { "81 - 100", 0 },
                { "100 +", 0 }
            };
        }

        public Dictionary<string, decimal> UserByGenderPercents { get; set; }

        public decimal FirstNamesStartingWithAtoM { get; set; }

        public decimal LastNamesStartingWithAtoM { get; set; }

        public Dictionary<string, decimal> UserByStatePercentsTop10 { get; set; }

        public Dictionary<string, decimal> FemaleByStatePercentsTop10 { get; set; }

        public Dictionary<string, decimal> MaleByStatePercentsTop10 { get; set; }

        public Dictionary<string, decimal> UserByAgeRangePercents { get; set; }
    }
}
