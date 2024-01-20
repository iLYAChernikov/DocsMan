namespace DocsMan.Domain.Entity
{
	public class Notification
	{
		public int Id { get; set; }
		public string Title
		{
			get => _title;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new NullReferenceException("Ошибка заполнения названия");
				_title = value;
			}
		}
		public string? Description { get; set; } = string.Empty;
		public DateTime DateTime { get; set; } = DateTime.Now;

		private string _title = string.Empty;
	}
}