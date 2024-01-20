namespace DocsMan.Domain.Entity
{
	public class Folder
	{
		public int Id { get; set; }
		public string Name
		{
			get => _name;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new NullReferenceException("Ошибка заполнения имени");
				_name = value;
			}
		}
		public string? Description { get; set; }

		private string _name = string.Empty;
	}
}