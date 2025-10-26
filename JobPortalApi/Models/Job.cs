using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobPortalApi.Models
{
	public class Job
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[MaxLength(150)]
		public string Title { get; set; } = string.Empty;

		[Required]
		[MaxLength(100)]
		public string Category { get; set; } = string.Empty;

		[Required]
		[MaxLength(150)]
		public string Company { get; set; } = string.Empty;

		[Required]
		[MaxLength(150)]
		public string Location { get; set; } = string.Empty;

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal SalaryMin { get; set; }

		[Required]
		[Column(TypeName = "decimal(18,2)")]
		public decimal SalaryMax { get; set; }

		[Required]
		public string Description { get; set; } = string.Empty;

		public DateTime PostedAt { get; set; } = DateTime.UtcNow;

		// Foreign key to recruiter
		public int RecruiterId { get; set; }

		[ForeignKey(nameof(RecruiterId))]
		public Recruiter Recruiter { get; set; } = default!;
	}
}
