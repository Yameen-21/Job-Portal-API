namespace JobPortalMvc.Models
{
    public class JobViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Company { get; set; } = "";
        public string Location { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal SalaryMin { get; set; }
        public decimal SalaryMax { get; set; }
    }
}
