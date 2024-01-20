namespace DocsMan.Domain.Entity
{
	public class UploadFile
	{
		public int Id { get; set; }
		public string FilePath
		{
			get => _filePath;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new NullReferenceException("Ошибка заполнения пути файла");
				_filePath = value;
			}
		}

		private string _filePath = string.Empty;
	}
}