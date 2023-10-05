namespace LAB.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public long Contact { get; set; }

        public string Email { get; set; }

        public DateTime Dateofbirth { get; set; }

        public string Allergy { get; set; }
    }
}
