namespace Core.GitHub.Models
{
    public class Permissions
    {
        public bool? Admin { get; set; }

        public bool? Push { get; set; }

        public bool? Pull { get; set; }
    }
}