namespace DocsMan.Domain.Entity
{
	public class Folder
	{
		public int Id
		{
			get => _id;
			set
			{
				if (value < 0)
					throw new ArgumentNullException("Ошибка заполнения id папки");
				_id = value;
			}
		}
		public string Name
		{
			get => _name;
			set
			{
				if (string.IsNullOrWhiteSpace(value))
					throw new ArgumentNullException("Ошибка заполнения имени папки");
				_name = value;
			}
		}
		public string? Description { get; set; }
		public bool IsDeleted { get; set; } = false;

		private int _id;
		private string _name = string.Empty;
	}
}