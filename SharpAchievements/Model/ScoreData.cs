namespace SharpAchievements.Model
{
    using System;

    public class ScoreData
    {
        private decimal score = 0;
        private bool isCompleted;

        public bool IsCompleted
        {
            get
            {
                return isCompleted;
            }
            set
            {
                this.isCompleted = value;

                if (this.isCompleted)
                {
                    this.DateEndedUtc = DateTime.UtcNow;
                }
            }
        }

        public string AchievementName { get; set; } = "";

        public decimal Score
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
