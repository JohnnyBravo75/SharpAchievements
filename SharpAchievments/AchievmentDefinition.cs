namespace SharpAchievments
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SharpAchievments.Model;

    public class AchievmentDefinition
    {
        public List<Achievment> Achievments = new List<Achievment>();

        public Achievment GetAchievment(string achievmentName)
        {
            var achievment = this.Achievments.FirstOrDefault(x => x.Name == achievmentName);
            if (achievment == null)
            {
                throw new Exception($"{achievmentName} is not a valid achievment");
            }

            return achievment;
        }
    }
}
