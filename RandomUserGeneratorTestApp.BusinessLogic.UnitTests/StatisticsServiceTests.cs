using RandomUserGeneratorTestApp.BusinessLogic.Services;
using RandomUserGeneratorTestApp.BusinessLogic.ViewModels.Requests;

namespace RandomUserGeneratorTestApp.BusinessLogic.UnitTests
{
    [TestClass]
    public class StatisticsServiceTests
    {
        [TestMethod]
        public void GetStats_Calculates_UserByGenderPercents()
        {
            var users = new UserRequest[] {
                new() { 
                    Gender = "male"
                },
                new() {
                    Gender = "female"
                },
                new() {
                    Gender = "female"
                },
                new() {
                    Gender = "female"
                }
            };

            var statisticsService = new StatisticsService();

            var results = statisticsService.GetStats(users);
            
            Assert.AreEqual(75M, results.UserByGenderPercents["female"]);
        }

        [TestMethod]
        public void GetStats_Calculates_FirstNamesStartingWithAtoMPercents()
        {
            var users = new UserRequest[] {
                new() {
                    Name = new() { First = "a" }
                },
                new() {
                    Name = new() { First = "H" }
                },
                new() {
                    Name = new() { First = "Z" }
                },
                new() {
                    Name = new() { First = "n" }
                }
            };

            var statisticsService = new StatisticsService();

            var results = statisticsService.GetStats(users);

            Assert.AreEqual(50M, results.FirstNamesStartingWithAtoM);
        }

        [TestMethod]
        public void GetStats_Calculates_LastNamesStartingWithAtoM()
        {
            var users = new UserRequest[] {
                new() {
                    Name = new() { Last = "a" }
                },
                new() {
                    Name = new() { Last = "H" }
                },
                new() {
                    Name = new() { Last = "Z" }
                },
                new() {
                    Name = new() { Last = "n" }
                }
            };

            var statisticsService = new StatisticsService();

            var results = statisticsService.GetStats(users);

            Assert.AreEqual(50M, results.LastNamesStartingWithAtoM);
        }

        [TestMethod]
        public void GetStats_Calculates_UserByStatePercentsTop10()
        {
            var users = new UserRequest[] {
                new() {
                    Location = new() { State = "Arizona" }
                },
                new() {
                    Location = new() { State = "Colorado" }
                },
                new() {
                    Location = new() { State = "California" }
                },
                new() {
                    Location = new() { State = "Pennsylvania" }
                },
                new() {
                    Location = new() { State = "Missouri" }
                },
                new() {
                    Location = new() { State = "South Carolina" }
                },
                new() {
                    Location = new() { State = "Michigan" }
                },
                new() {
                    Location = new() { State = "Iowa" }
                },
                new() {
                    Location = new() { State = "Minnesota" }
                },
                new() {
                    Location = new() { State = "Kansas" }
                },
                new() {
                    Location = new() { State = "Colorado" }
                },
                new() {
                    Location = new() { State = "California" }
                },
                new() {
                    Location = new() { State = "California" }
                },
                new() {
                    Location = new() { State = "Georgia" }
                }
            };

            var statisticsService = new StatisticsService();

            var results = statisticsService.GetStats(users);

            Assert.AreEqual(10, results.UserByStatePercentsTop10.Count);
            Assert.AreEqual("california", results.UserByStatePercentsTop10.First().Key);
            Assert.AreEqual((3M / 14M) * 100M, results.UserByStatePercentsTop10.First().Value);
            Assert.AreEqual(0, results.UserByStatePercentsTop10.Count(x => x.Key == "south carolina"));
        }

        [TestMethod]
        public void GetStats_Calculates_FemaleByStatePercentsTop10()
        {
            var users = new UserRequest[] {
                new() {
                    Location = new() { State = "Arizona" },
                    Gender = "female"
                },
                new() {
                    Location = new() { State = "Colorado" },
                    Gender = "female"
                },
                new() {
                    Location = new() { State = "California" },
                    Gender = "male"
                },
                new() {
                    Location = new() { State = "Pennsylvania" },
                    Gender = "female"
                },
                new() {
                    Location = new() { State = "Missouri" },
                    Gender = "female"
                },
                new() {
                    Location = new() { State = "South Carolina" },
                    Gender = "female"
                },
                new() {
                    Location = new() { State = "Michigan" },
                    Gender = "female"
                },
                new() {
                    Location = new() { State = "Iowa" },
                    Gender = "female"
                },
                new() {
                    Location = new() { State = "Minnesota" },
                    Gender = "female"
                },
                new() {
                    Location = new() { State = "Kansas" },
                    Gender = "female"
                },
                new() {
                    Location = new() { State = "Colorado" },
                    Gender = "female"
                },
                new() {
                    Location = new() { State = "California" },
                    Gender = "male"
                },
                new() {
                    Location = new() { State = "California" },
                    Gender = "male"
                },
                new() {
                    Location = new() { State = "Georgia" },
                    Gender = "female"
                }
            };

            var statisticsService = new StatisticsService();

            var results = statisticsService.GetStats(users);

            Assert.AreEqual(10, results.FemaleByStatePercentsTop10.Count);

            Assert.AreEqual("colorado", results.FemaleByStatePercentsTop10.First().Key);
            Assert.AreEqual((2M / 14M) * 100M, results.FemaleByStatePercentsTop10.First().Value);

            Assert.AreEqual("arizona", results.FemaleByStatePercentsTop10.Skip(1).First().Key);
            Assert.AreEqual((1M / 14M) * 100M, results.FemaleByStatePercentsTop10.Skip(1).First().Value);

            Assert.AreEqual(0, results.FemaleByStatePercentsTop10.Count(x => x.Key == "california"));
        }

        [TestMethod]
        public void GetStats_Calculates_UserByAgeRangePercents()
        {
            var users = new UserRequest[] {
                new() {
                    Dob = new() { Age = 1 }
                },
                new() {
                    Dob = new() { Age = 0 }
                },
                new() {
                    Dob = new() { Age = 20 }
                },
                new() {
                    Dob = new() { Age = 5 }
                },
                new() {
                    Dob = new() { Age = 16 }
                },
                new() {
                    Dob = new() { Age = 12 }
                },
                new() {
                    Dob = new() { Age = 14 }
                },
                new() {
                    Dob = new() { Age = 11 }
                },
                new() {
                    Dob = new() { Age = 64 }
                },
                new() {
                    Dob = new() { Age = 63 }
                },
                new() {
                    Dob = new() { Age = 62 }
                },
                new() {
                    Dob = new() { Age = 61 }
                },
                new() {
                    Dob = new() { Age = 100 }
                },
                new() {
                    Dob = new() { Age = 101 }
                }
            };

            var statisticsService = new StatisticsService();

            var results = statisticsService.GetStats(users);

            Assert.AreEqual((8M / 14M) * 100M, results.UserByAgeRangePercents["0 - 20"]);
            Assert.AreEqual((4M / 14M) * 100M, results.UserByAgeRangePercents["61 - 80"]);
            Assert.AreEqual((1M / 14M) * 100M, results.UserByAgeRangePercents["81 - 100"]);
            Assert.AreEqual((1M / 14M) * 100M, results.UserByAgeRangePercents["100 +"]);
        }

        [TestMethod]
        public void GetStatsSummaryText_Calculates_UserByAgeRangePercents()
        {
            var users = new UserRequest[] {
                new() {
                    Gender = "female"
                }
            };

            var statisticsService = new StatisticsService();

            var results = statisticsService.GetStatsSummaryText(users);

            Assert.AreEqual(7, results.Length);

            Assert.AreEqual($"1. Percentage of gender in each category... female: 100.00%", results[0]);
            Assert.AreEqual($"2. Percentage of first names that start with A - M versus N-Z: 0.00%", results[1]);
            Assert.AreEqual($"3. Percentage of last names that start with A - M versus N-Z: 0.00%", results[2]);
            Assert.AreEqual($"4. Percentage of people in each state, up to the top 10 most populous states... : 100.00%", results[3]);
            Assert.AreEqual($"5. Percentage of females in each state, up to the top 10 most populous states... : 100.00%", results[4]);
            Assert.AreEqual($"6. Percentage of males in each state, up to the top 10 most populous states... ", results[5]);
            Assert.AreEqual($"7. Percentage of people in the following age ranges... 0 - 20: 0.00%, 21 - 40: 0.00%, 41 - 60: 0.00%, 61 - 80: 0.00%, 81 - 100: 0.00%, 100 +: 0.00%", results[6]);
        }
    }
}