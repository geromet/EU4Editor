namespace DataAccessLibrary.Models
{
    public class Religion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ReligionGroup ReligionGroup { get; set; }
       
        public bool HreReligion { get; set; }
    }
    public class ReligionGroup
    {
        public int Id { get; set; }
        public string Name;
        public bool DefenderOfFaith { get; set; }
        public bool CanFormPersonalUnions { get; set; }
        public string CenterOfReligion { get; set; }
        public List<Religion> Religions;
    }
}
