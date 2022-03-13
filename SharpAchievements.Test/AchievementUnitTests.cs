namespace SharpAchievements.Test
{
    using System.Diagnostics;
    using SharpAchievements.Model;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SharpAchievements;

    public class AchievementGroups
    {
        public const string COMBAT = "Combat";
        public const string STEALTH = "Stealth";
    }

    public class AchievementNames
    {
        public const string PYROMANIC = "Pyromanic";
        public const string EXECUTIONER = "Executioner";

        public const string SNIPER = "Sniper";
        public const string ASSASSIN = "Assassin";
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
    public class AchievementUnitTests
    {
        private AchievementDefinition SetupAchievementDefinition()
        {
            var achievementDefinition = new AchievementDefinition();
            achievementDefinition.Achievements.Add(new Achievement()
            {
                Name = AchievementNames.PYROMANIC,
                Group = AchievementGroups.COMBAT,
                Ranks = { new Rank()
                                {
                                    Name=RankNames.COMPLETED,
                                    Score = 20
                                }
                        }
            });

            achievementDefinition.Achievements.Add(new Achievement()
            {
                Name = AchievementNames.EXECUTIONER,
                Group = AchievementGroups.COMBAT,
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
            return achievementDefinition;
        }

        [TestMethod]
        public void Test_Get_Completed_By_Score()
        {
            var achievementData = new AchievementData(SetupAchievementDefinition());
            
            achievementData.AddScore(AchievementNames.PYROMANIC, 20);
            Assert.IsTrue(achievementData.IsCompleted(AchievementNames.PYROMANIC));

            achievementData.AddScore(AchievementNames.EXECUTIONER, 60);
            Assert.IsTrue(achievementData.IsCompleted(AchievementNames.EXECUTIONER));
        }

        [TestMethod]
        public void Test_Get_Completed_Manual()
        {
            var achievementData = new AchievementData(SetupAchievementDefinition());

            achievementData.SetCompleted(AchievementNames.PYROMANIC);
            Assert.IsTrue(achievementData.IsCompleted(AchievementNames.PYROMANIC));

            achievementData.SetCompleted(AchievementNames.EXECUTIONER);
            Assert.IsTrue(achievementData.IsCompleted(AchievementNames.EXECUTIONER));
        }

        [TestMethod]
        public void Test_Get_Completed_By_Rank()
        {
            var achievementData = new AchievementData(SetupAchievementDefinition());

            achievementData.EarnRank(AchievementNames.PYROMANIC, RankNames.COMPLETED);
            Assert.IsTrue(achievementData.IsCompleted(AchievementNames.PYROMANIC));

            achievementData.EarnRank(AchievementNames.EXECUTIONER, RankNames.GOLD);
            Assert.IsFalse(achievementData.IsCompleted(AchievementNames.EXECUTIONER));

            achievementData.EarnRank(AchievementNames.EXECUTIONER, RankNames.PLATIN);
            Assert.IsTrue(achievementData.IsCompleted(AchievementNames.EXECUTIONER));
        }

        [TestMethod]
        public void Test_EarnRank_By_Score()
        {
            var achievementData = new AchievementData(SetupAchievementDefinition());

            achievementData.AddScore(AchievementNames.PYROMANIC, 20);
            var scoreData1 = achievementData.GetScoreData(AchievementNames.PYROMANIC);
            Assert.AreEqual(scoreData1.EarnedRank, RankNames.COMPLETED);

            achievementData.AddScore(AchievementNames.EXECUTIONER, 15);
            var scoreData2 = achievementData.GetScoreData(AchievementNames.EXECUTIONER);
            Assert.AreEqual(scoreData2.EarnedRank, RankNames.BRONZE);
        }

        [TestMethod]
        public void TestMethod1()
        {
            var achievementDefinition = SetupAchievementDefinition();

            var achievementData = new AchievementData(achievementDefinition);

            achievementData.AchievementCompleted += this.AchievementData_AchievmentCompleted;
            achievementData.RankEarned += this.AchievementData_RankEarned;

            achievementData.AddScore(AchievementNames.PYROMANIC, 15);

            if (!achievementData.IsCompleted(AchievementNames.PYROMANIC))
            {
                Debug.WriteLine("Completed: " + achievementData.GetPercentageCompleted(AchievementNames.PYROMANIC) + "%");
                achievementData.SetCompleted(AchievementNames.PYROMANIC);
                Debug.WriteLine("Completed: " + achievementData.GetPercentageCompleted(AchievementNames.PYROMANIC) + "%");
            }

            achievementData.AddScore(AchievementNames.EXECUTIONER, 15);
            achievementData.AddScore(AchievementNames.EXECUTIONER, 23);
            achievementData.EarnRank(AchievementNames.EXECUTIONER, RankNames.GOLD);

            achievementData.PrintStats();

            achievementData.AchievementCompleted -= this.AchievementData_AchievmentCompleted;
            achievementData.RankEarned -= this.AchievementData_RankEarned;

        }

        private void AchievementData_RankEarned(object? sender, System.Tuple<string, string> data)
        {
            Debug.WriteLine("Was earned: " + data.Item1 + "\\" + data.Item2);
        }

        private void AchievementData_AchievmentCompleted(object sender, string achievementName)
        {
            Debug.WriteLine("Was completed: " + achievementName);
        }
    }
}