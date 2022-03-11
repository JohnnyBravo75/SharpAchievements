namespace SharpAchievements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SharpAchievements.Model;

    public class AchievementDefinition
    {
        public List<Achievement> Achievements = new List<Achievement>();

        public Achievement GetAchievment(string achievementName)
        {
            var achievement = this.Achievements.FirstOrDefault(x => x.Name == achievementName);
            if (achievement == null)
            {
                throw new Exception($"{achievementName} is not a valid achievement");
            }

            return achievement;
        }
    }
}
