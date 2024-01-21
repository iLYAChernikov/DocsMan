namespace DocsMan.Domain.Entity
{
	public class UploadFile
	{
		public int Id
		{
			get => _id;
			set
			{
				if ( value < 0 )
					throw new ArgumentNullException("Ошибка заполнения id файла");
				_id = value;
			}
		}
		public string FilePath
		{
			get => _filePath;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new ArgumentNullException("Ошибка заполнения пути файла");
				_filePath = value;
			}
		}

		private int _id;
		private string _filePath = string.Empty;
	}
}