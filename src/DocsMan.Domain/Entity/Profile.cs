namespace DocsMan.Domain.Entity
{
	public class Profile
	{
		public int Id { get; set; }

		public int UserId { get; set; }
		public User User { get; set; }

		public string? SurName { get; set; } = string.Empty;
		public string? Name { get; set; } = string.Empty;
		public string? LastName { get; set; } = string.Empty;

		public DateTime Birthdate { get; set; } = DateTime.Now;
		public Gender Gender { get; set; } = Gender.Man;
		public string Email { get; set; } = null!;
		public string? PhoneNumber { get; set; } = string.Empty;
	}

	public enum Gender : short
	{
		Woman = 0,
		Man = 1
	}
}