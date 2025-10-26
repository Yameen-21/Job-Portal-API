using System.ComponentModel.DataAnnotations;

namespace JobPortalApi.Models
{
	public class Recruiter
	{
		[Key]
		public int Id { get; set; }

		[Required, MaxLength(150)]
		public string FullName { get; set; } = string.Empty;

		[Required, EmailAddress]
		public string Email { get; set; } = string.Empty;

		[MaxLength(150)]
		public string Company { get; set; } = string.Empty;

		public ICollection<Job>? Jobs { get; set; }
	}
}
