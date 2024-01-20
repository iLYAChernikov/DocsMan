namespace DocsMan.Blazor.Shared.DTOs
{
	public class NotificationDto
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public string? Description { get; set; } = string.Empty;
		public DateTime DateTime { get; set; } = DateTime.Now;
	}
}