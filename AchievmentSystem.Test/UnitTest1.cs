namespace AchievmentSystem.Test
{
    using AchievmentSystem.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class AchievmentNames
    {
        public const string FILECOPYMASTER = "FilesCopyMaster";
        public const string DOUBLETKILLER = "DoubletKiller";
    }

    public class RankNames
    {
        public const string COMPLETED = "Completed";
        public const string BRONZE = "Bronze";
        public const string SILVER = "Silver";
        public const string GOLD = "Gold";
        public const string PLATIN = "Platin";
    }

    [TestClass]
    public class UnitTest1
    {
        private AchievmentDefinition SetupAchievmentDefinition()
        {
            var achievmentDefinition = new AchievmentDefinition();
            achievmentDefinition.Achievments.Add(new Achievment()
            {
                Name = AchievmentNames.FILECOPYMASTER,
                Ranks = { new Rank()
                                {
                                    Name=RankNames.COMPLETED,
                                    Score = 20
                                }
                        }
            });

            achievmentDefinition.Achievments.Add(new Achievment()
            {
                Name = AchievmentNames.DOUBLETKILLER,
                Ranks = {       new Rank()
                                {   Order = 1,
                                    Name=RankNames.BRONZE,
                                    Score = 10
                                },
                                new Rank()
                                {   Order = 2,
                                    Name=RankNames.SILVER,
                                    Score = 20
                                },
                                new Rank()
                                {   Order = 3,
                                    Name=RankNames.GOLD,
                                    Score = 30
                                },
                                new Rank()
                                {   Order = 4,
                                    Name=RankNames.PLATIN,
                                    Score = 50
                                }
                        }
            });
            return achievmentDefinition;
        }

        [TestMethod]
        public void TestMethod1()
        {
            var achievmentDefinition = SetupAchievmentDefinition();

            var achievmentData = new AchievmentData(achievmentDefinition);

            achievmentData.AddScore(AchievmentNames.FILECOPYMASTER, 15);
            achievmentData.AddScore(AchievmentNames.FILECOPYMASTER, 15);
            achievmentData.AddScore(AchievmentNames.DOUBLETKILLER, 23);
            achievmentData.EarnRank(AchievmentNames.DOUBLETKILLER, RankNames.GOLD);

            achievmentData.PrintStats();
        }
    }
}