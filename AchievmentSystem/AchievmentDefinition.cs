namespace AchievmentSystem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AchievmentSystem.Model;

    public class AchievmentDefinition
    {
        public List<Achievment> Achievments = new List<Achievment>();

        public Achievment GetAchievment(string achievmentName)
        {
            var achievment = Achievments.FirstOrDefault(x => x.Name == achievmentName);
            if (achievment == null)
            {
                throw new Exception($"{achievmentName} is not a valid achievment");
            }

            return achievment;
        }
    }
}
