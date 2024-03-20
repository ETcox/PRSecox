namespace PRSecox.Models
{
    public class SafeUserDTO
    {

        public int Id { get; set; }

        
        public string Username { get; set; }

       
        public string Firstname { get; set; }

       
        public string Lastname { get; set; }

      
        public string? Phone { get; set; }

       
        public string? Email { get; set; }

       
        public bool Reviewer { get; set; }

        
        public bool Admin { get; set; }
    }
}
