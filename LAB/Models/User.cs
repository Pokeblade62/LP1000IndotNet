namespace LAB.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Lastlogin { get; set; }

        public string Permission { get; set; }

        public string Status { get; set; }

    }
}
