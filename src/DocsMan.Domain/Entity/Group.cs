namespace DocsMan.Domain.Entity
{
	public class Group
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

		private string _title = string.Empty;
	}
}