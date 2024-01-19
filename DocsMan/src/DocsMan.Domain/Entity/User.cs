namespace DocsMan.Domain.Entity
{
	public class User
	{
		public int Id { get; set; }
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;
	}
}