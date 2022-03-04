﻿namespace AchievmentSystem.Model
{
    public class Rank
    {
        public int Order { get; set; } = 0;

        public string Name { get; set; } = "";

        public string Image { get; set; } = "";

        public int Score { get; set; } = 0;

        public override string ToString()
        {
            return $"{Order} {Name}: {Score}";
        }
    }
}
