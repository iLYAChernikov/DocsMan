namespace DocsMan.Blazor.Shared.DTOs
{
	public class ProfileDto
	{
		public int Id { get; set; }

		public int UserId { get; set; }

		public string SurName { get; set; } = null!;
		public string Name { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string Full_FIO => $"{SurName} {Name} {LastName}";
		public string Short_FIO => $"{SurName} {Name[0]}.{LastName[0]}.";

		public DateTime Birthdate { get; set; } = DateTime.Now;
		public GenderDto Gender { get; set; } = GenderDto.Man;
		public string Email { get; set; } = null!;
		public string? PhoneNumber { get; set; } = string.Empty;
	}

	public enum GenderDto : short
	{
		Woman = 0,
		Man = 1
	}
}