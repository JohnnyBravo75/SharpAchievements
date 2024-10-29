namespace SharpAchievements.Model
{
    public class Badge
    {
        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public string Image { get; set; } = "";

        public override string ToString()
        {
            return $"{this.Name}";
        }
    }
}
