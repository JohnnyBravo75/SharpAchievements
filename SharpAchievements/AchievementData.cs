namespace SharpAchievements
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using SharpAchievements.Model;

    public class AchievementData
    {
        private AchievementDefinition achievementDefinition;

        public event EventHandler<string> AchievementCompleted;
        public event EventHandler<Tuple<string, string>> RankEarned;


        public AchievementData(AchievementDefinition achievementDefinition)
        {
            if (achievementDefinition == null)
            {
                throw new ArgumentNullException(nameof(achievementDefinition));
            }
            this.AchievementDefinition = achievementDefinition;
        }

        public List<ScoreData> Scores = new List<ScoreData>();

        public AchievementDefinition AchievementDefinition
        {
            get { return this.achievementDefinition; }
            private set { this.achievementDefinition = value; }
        }

        public void PrintStats()
        {
            foreach (var score in this.Scores)
            {
                var achievement = this.achievementDefinition.GetAchievment(score.AchievementName);
                var percentageCompleted = achievement.GetPercentageCompleted(score);

                Debug.WriteLine($"{achievement.Group}: {score} - {percentageCompleted}%  :  Started: {score.DateStartedUtc}  CompletedScore: {achievement.CompletedScore}, IsCompleted: {score.IsCompleted}");
            }
        }

        public void Start(string achievementName)
        {
            var scoreData = this.GetScoreData(achievementName);
            scoreData.Start(); 
        }

        public bool IsStarted(string achievementName)
        {
            var scoreData = this.GetScoreData(achievementName);
            return scoreData.IsStarted;
        }

        public void End(string achievementName)
        {
            var scoreData = this.GetScoreData(achievementName);
            ValidateStarted(scoreData);

            scoreData.End();
        }

        public bool IsEnded(string achievementName)
        {
            var scoreData = this.GetScoreData(achievementName);
            return scoreData.IsEnded;
        }

        
        private static void ValidateStarted(ScoreData scoreData)
        {
            if (!scoreData.IsStarted)
            {
                throw new Exception($"Achievement '{scoreData.AchievementName}' was not started");
            }
        }

        public string AddScore(string achievementName, decimal score)
        {
            var scoreData = this.GetScoreData(achievementName);

            return this.SetScore(achievementName, scoreData.Score + score);
        }

        public string SetScore(string achievementName, decimal score)
        {
            var achievement = this.achievementDefinition.GetAchievment(achievementName);
            if (achievement == null)
            {
                return string.Empty;
            }

            // Dependency
            if (!string.IsNullOrEmpty(achievement.DependsOnAchievementName))
            {
                var dependsOnAchievment = this.achievementDefinition.GetAchievment(achievement.DependsOnAchievementName);
                var dependsOnScoreData = this.GetScoreData(dependsOnAchievment.Name);
                if (!dependsOnAchievment.IsCompleted(dependsOnScoreData))
                {
                    return $"Achievement '{dependsOnAchievment.Name}' needs to be completed first.";
                }
            }

            // Score
            var scoreData = this.GetScoreData(achievementName);

            ValidateStarted(scoreData);

            scoreData.Score = score;
            
            if (scoreData.Score >= achievement.CompletedScore)
            {
                scoreData.IsCompleted = true;
                if (achievement.AutoEndWhenCompleted)
                {
                    scoreData.End();
                }
            }
            

            // Rank
            if (achievement.AutoEarnRankFromScore)
            {
                this.EarnRankFromScore(achievementName, scoreData);
            }
            return string.Empty;
        }


        public bool IsCompleted(string achievementName)
        {
            var achievement = this.achievementDefinition.GetAchievment(achievementName);
            if (achievement == null)
            {
                return false;
            }

            var scoreData = this.GetScoreData(achievementName);

            return achievement.IsCompleted(scoreData);
        }

        public decimal GetPercentageCompleted(string achievementName)
        {
            var achievement = this.achievementDefinition.GetAchievment(achievementName);
            if (achievement == null)
            {
                return 0;
            }

            var scoreData = this.GetScoreData(achievementName);

            return achievement.GetPercentageCompleted(scoreData);
        }

        public void SetCompleted(string achievementName)
        {
            var achievement = this.achievementDefinition.GetAchievment(achievementName);
            if (achievement == null)
            {
                return;
            }

            var scoreData = this.GetScoreData(achievementName);
            
            scoreData.IsCompleted = true;
            if (achievement.AutoEndWhenCompleted)
            {
                scoreData.End();
            }   

            // this.EarnLastRank(achievement);
        }

        private void EarnLastRank(Achievement achievement)
        {
            var lastRank = achievement.Ranks.LastOrDefault();
            if (lastRank == null)
            {
                return;
            }

            this.EarnRank(achievement.Name, lastRank.Name);
        }

        private void EarnRankFromScore(string achievementName, ScoreData scoreData = null)
        {
            if (scoreData == null)
            {
                scoreData = this.GetScoreData(achievementName);
            }

            var rank = this.GetRankFromScore(achievementName, scoreData.Score);
            if (rank != null)
            {
                this.SetRank(scoreData, rank.Name);
            }
        }

        public Rank GetRankFromScore(string achievementName, decimal score)
        {
            var achievement = this.achievementDefinition.GetAchievment(achievementName);
            if (achievement == null)
            {
                return null;
            }

            var rank = achievement.Ranks.Where(x => score >= x.Score)
                                       .LastOrDefault();
            return rank;
        }

        public ScoreData GetScoreData(string achievementName)
        {
            var achievement = this.achievementDefinition.GetAchievment(achievementName);
            if (achievement == null)
            {
                return null;
            }

            var scoreData = this.Scores.FirstOrDefault(x => x.AchievementName == achievementName);

            if (scoreData == null)
            {
                scoreData = new ScoreData()
                {
                    AchievementName = achievementName                    
                };

                if (achievement.AutoStart)
                {
                    scoreData.Start();
                }   

                this.Scores.Add(scoreData);
            }

            return scoreData;
        }

        public void EarnRank(string achievementName, string rankName)
        {
            var achievement = this.achievementDefinition.GetAchievment(achievementName);
            if (achievement == null)
            {
                return;
            }

            if (!achievement.IsValidRank(rankName))
            {
                throw new Exception("{rankName} is not a valid rank");
            }

            var scoreData = this.GetScoreData(achievementName);

            if (scoreData == null)
            {
                scoreData = new ScoreData()
                {
                    AchievementName = achievementName,
                };
                this.Scores.Add(scoreData);
            }


            this.SetRank(scoreData, rankName);

        }

        private void SetRank(ScoreData scoreData, string rankName)
        {
            if (scoreData.EarnedRank != rankName)
            {
                var wasCompleted = this.IsCompleted(scoreData.AchievementName);

                scoreData.EarnedRank = rankName;

                this.RankEarned?.Invoke(this, new Tuple<string, string>(scoreData.AchievementName, rankName));

                if (!wasCompleted && this.IsCompleted(scoreData.AchievementName))
                {
                    this.AchievementCompleted?.Invoke(this, scoreData.AchievementName);
                }
            }
        }

        public int CountCompleted
        {
            get
            {
                int completed = 0;
                foreach (var score in this.Scores)
                {
                    var achievement = this.achievementDefinition.GetAchievment(score.AchievementName);
                    if (achievement != null)
                    {
                        if (achievement.IsCompleted(score))
                        {
                            completed++;
                        }
                    }
                }

                return completed;
            }
        }

        public int CountTotal
        {
            get { return this.Scores.Count; }
        }
    }
}
