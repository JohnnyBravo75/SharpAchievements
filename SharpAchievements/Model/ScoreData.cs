namespace SharpAchievements.Model
{
    using System;

    public class ScoreData
    {
        private int score = 0;

        public string AchievementName { get; set; } = "";

        public int Score
        {
            get
            {
                return this.score;
            }
            set
            {
                if (this.score != value)
                {
                    if (this.score == 0 && value > 0 && !this.DateStartedUtc.HasValue)
                    {
                        this.DateStartedUtc = DateTime.UtcNow;
                    }

                    this.score = value;

                }
            }
        }
        public string EarnedRank { get; set; } = "";

        public DateTime? DateStartedUtc { get; set; }

        public DateTime? DateEndedUtc { get; set; }

        public override string ToString()
        {
            return $"{this.AchievementName}: {this.Score} ({this.EarnedRank})";
        }
    }
}
