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
                return this.isCompleted;
            }
            set
            {
                if (this.isCompleted != value)
                {
                    this.isCompleted = value;
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

        public bool IsStarted => this.DateStartedUtc.HasValue;

        public bool IsEnded => this.DateEndedUtc.HasValue;

        public void Start()
        {
            this.DateStartedUtc = DateTime.UtcNow;
        }

        public void End()
        {
            this.DateEndedUtc = DateTime.UtcNow;
        }
    }
}
