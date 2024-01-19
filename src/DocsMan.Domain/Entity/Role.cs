namespace DocsMan.Domain.Entity
{
	public class Role
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public string? Description { get; set; } = string.Empty;
	}
}