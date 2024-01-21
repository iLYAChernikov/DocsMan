namespace DocsMan.Domain.Entity
{
	public class Role
	{
		public int Id
		{
			get => _id;
			set
			{
				if ( value < 0 )
					throw new ArgumentNullException("Ошибка заполнения id роли");
				_id = value;
			}
		}
		public string Title
		{
			get => _title;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new ArgumentNullException("Ошибка заполнения названия роли");
				_title = value;
			}
		}
		public string? Description { get; set; } = string.Empty;

		private int _id;
		private string _title = string.Empty;
	}
}