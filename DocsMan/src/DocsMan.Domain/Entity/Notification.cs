namespace DocsMan.Domain.Entity
{
	public class Notification
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public string? Description { get; set; } = string.Empty;
		public DateTime DateTime { get; set; } = DateTime.Now;
	}
}