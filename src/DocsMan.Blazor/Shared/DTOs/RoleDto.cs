namespace DocsMan.Blazor.Shared.DTOs
{
	public class RoleDto
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public string? Description { get; set; } = string.Empty;
	}
}