using JobPortalApi.Models;

namespace JobPortalApi.Data
{
	public static class DatabaseSeeder
	{
		public static void Seed(AppDbContext context)
		{
			if (!context.Jobs.Any())
			{
				context.Jobs.AddRange( 
					new Job
					{
						Title = "Software Engineer",
						Description = "Develop .NET Core applications.",
						Company = "TechVerse",
						Location = "Karachi",
						SalaryMin = 90000,
						SalaryMax = 150000,
						PostedAt = DateTime.UtcNow
	},
	new Job
	{
		Title = "Frontend Developer",
		Description = "Build modern web UIs.",
		Company = "Pixel Studio",
		Location = "Lahore",
		SalaryMin = 80000,
		SalaryMax = 120000,
		PostedAt = DateTime.UtcNow
	}
);

				context.SaveChanges();
			}
		}
	}

}
