namespace SharpAchievments.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Achievment
    {
        public Achievment(string name = "", List<Rank> ranks = null, string group = "")
        {
            this.Name = name;
            if (ranks != null)
            {
                this.Ranks = ranks;
            }
            this.Group = group;
        }

        public string Group { get; set; } = "Common";
        public string Name { get; set; } = "";
        public List<Rank> Ranks { get; set; } = new List<Rank>();

        public string DependsOnAchievmentName { get; set; } = "";

        public TimeSpan? TimeLimit { get; set; }

        public override string ToString()
        {
            return $"{this.Group}: {this.Name}";
        }

        public bool IsCompleted(ScoreData scoreData)
        {
            return this.GetPercentageCompleted(scoreData.EarnedRank) >= 100;
        }

        public decimal GetPercentageCompleted(string rankName)
        {
            var currentPos = this.GetRankPosition(rankName);
            if (currentPos < 0)
            {
                return 0;
            }
            var maxPos = this.Ranks.Count();
            if (maxPos == 0)
            {
                return 0;
            }

            return (decimal)currentPos * 100 / maxPos;
        }

        public int GetRankPosition(string rankName)
        {
            if (string.IsNullOrEmpty(rankName))
            {
                return 0;
            }

            var rank = this.GetRank(rankName);
            if (rank == null)
            {
                return 0;
            }

            return this.Ranks.IndexOf(rank) + 1;
        }

        public bool IsLastRank(string rankName)
        {
            if (string.IsNullOrEmpty(rankName))
            {
                return false;
            }
            return this.Ranks.LastOrDefault()?.Name == rankName;
        }

        public Rank GetRank(string rankName)
        {
            if (string.IsNullOrEmpty(rankName))
            {
                return null;
            }
            return this.Ranks.FirstOrDefault(x => x.Name == rankName);
        }

        public bool IsValidRank(string rankName)
        {
            return this.GetRank(rankName) != null;
        }
    }
}
