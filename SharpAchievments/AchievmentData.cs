namespace SharpAchievments
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using SharpAchievments.Model;

    public class AchievmentData
    {
        private AchievmentDefinition achievmentDefinition;

        public event EventHandler<string> AchievmentCompleted;
        public event EventHandler<Tuple<string, string>> RankEarned;


        public AchievmentData(AchievmentDefinition achievmentDefinition)
        {
            if (achievmentDefinition == null)
            {
                throw new ArgumentNullException(nameof(achievmentDefinition));
            }
            this.AchievmentDefinition = achievmentDefinition;
        }

        public List<ScoreData> Scores = new List<ScoreData>();

        public AchievmentDefinition AchievmentDefinition
        {
            get { return this.achievmentDefinition; }
            private set { this.achievmentDefinition = value; }
        }

        public void PrintStats()
        {
            foreach (var score in this.Scores)
            {
                var achievment = this.achievmentDefinition.GetAchievment(score.AchievmentName);
                var percentageCompleted = achievment.GetPercentageCompleted(score.EarnedRank);

                Debug.WriteLine($"{achievment.Group}: {score} - {percentageCompleted}%  :  {DateTime.UtcNow - score.DateStartedUtc}");
            }
        }


        public string AddScore(string achievmentName, int score)
        {
            var achievment = this.achievmentDefinition.GetAchievment(achievmentName);
            if (achievment == null)
            {
                return string.Empty;
            }

            // Dependency
            if (!string.IsNullOrEmpty(achievment.DependsOnAchievmentName))
            {
                var dependsOnAchievment = this.achievmentDefinition.GetAchievment(achievment.DependsOnAchievmentName);
                var dependsOnScoreData = this.GetScoreData(dependsOnAchievment.Name);
                if (!dependsOnAchievment.IsCompleted(dependsOnScoreData))
                {
                    return $"Achievement '{dependsOnAchievment.Name}' needs to be completed first.";
                }
            }

            // Score
            var scoreData = this.GetScoreData(achievmentName);

            if (scoreData != null)
            {
                scoreData.Score += score;
            }
            else
            {
                scoreData = new ScoreData()
                {
                    AchievmentName = achievmentName,
                    DateStartedUtc = DateTime.UtcNow,
                    Score = score
                };

                this.Scores.Add(scoreData);
            }

            // Rank
            this.EarnRankFromScore(achievmentName, scoreData);

            // Ended
            if (achievment.IsCompleted(scoreData))
            {
                scoreData.DateEndedUtc = DateTime.UtcNow;
            }

            return string.Empty;
        }


        public bool IsCompleted(string achievmentName)
        {
            var achievment = this.achievmentDefinition.GetAchievment(achievmentName);
            if (achievment == null)
            {
                return false;
            }

            var scoreData = this.GetScoreData(achievmentName);

            return achievment.IsCompleted(scoreData);
        }

        public decimal GetPercentageCompleted(string achievmentName)
        {
            var achievment = this.achievmentDefinition.GetAchievment(achievmentName);
            if (achievment == null)
            {
                return 0;
            }

            var scoreData = this.GetScoreData(achievmentName);

            return achievment.GetPercentageCompleted(scoreData.EarnedRank);
        }

        public void SetCompleted(string achievmentName)
        {
            var achievment = this.achievmentDefinition.GetAchievment(achievmentName);
            if (achievment == null)
            {
                return;
            }

            var lastRank = achievment.Ranks.LastOrDefault();
            if (lastRank == null)
            {
                return;
            }

            this.EarnRank(achievmentName, lastRank.Name);
        }

        private void EarnRankFromScore(string achievmentName, ScoreData scoreData)
        {
            var rank = this.GetRankFromScore(achievmentName, scoreData.Score);
            if (rank != null)
            {
                this.SetRank(scoreData, rank.Name);
            }
        }

        private Rank GetRankFromScore(string achievmentName, int score)
        {
            var achievment = this.achievmentDefinition.GetAchievment(achievmentName);
            if (achievment == null)
            {
                return null;
            }

            var rank = achievment.Ranks.Where(x => score >= x.Score)
                                       .LastOrDefault();
            return rank;
        }

        private ScoreData GetScoreData(string achievmentName)
        {
            var achievment = this.achievmentDefinition.GetAchievment(achievmentName);
            if (achievment == null)
            {
                return null;
            }

            var scoreData = this.Scores.FirstOrDefault(x => x.AchievmentName == achievmentName);
            return scoreData;
        }

        public void EarnRank(string achievmentName, string rankName)
        {
            var achievment = this.achievmentDefinition.GetAchievment(achievmentName);
            if (achievment == null)
            {
                return;
            }

            if (!achievment.IsValidRank(rankName))
            {
                throw new Exception("{rankName} is not a valid rank");
            }

            var scoreData = this.GetScoreData(achievmentName);

            if (scoreData == null)
            {
                scoreData = new ScoreData()
                {
                    AchievmentName = achievmentName,
                };
                this.Scores.Add(scoreData);
            }

           
            this.SetRank(scoreData, rankName);
            
        }

        private void SetRank(ScoreData scoreData, string rankName)
        {
            if (scoreData.EarnedRank != rankName)
            {
                var wasCompleted = this.IsCompleted(scoreData.AchievmentName);

                scoreData.EarnedRank = rankName;

                this.RankEarned?.Invoke(this, new Tuple<string, string>(scoreData.AchievmentName, rankName));
                
                if (!wasCompleted && this.IsCompleted(scoreData.AchievmentName))
                {
                    this.AchievmentCompleted?.Invoke(this, scoreData.AchievmentName);
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
                    var achievment = this.achievmentDefinition.GetAchievment(score.AchievmentName);
                    if (achievment != null)
                    {
                        if (achievment.IsCompleted(score))
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
