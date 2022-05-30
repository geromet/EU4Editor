namespace DataAccessLibrary.Models
{
    public class Idea
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Modifier> Modifiers { get; set; }
        
        public string Description { get; set; }
    }
    public class IdeaGroup
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public List<Modifier> Bonus { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Idea> Ideas { get; set; }
    }
}
