namespace DocsMan.Blazor.Shared.Helpers
{
	public class DataFile
	{
		public int OwnerId
		{
			get => _id;
			set
			{
				if ( value <= 0 )
					throw new ArgumentNullException("Null input data");
				_id = value;
			}
		}
		public string FileName
		{
			get => _fileName;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new ArgumentNullException("Null input data");
				_fileName = value;
			}
		}
		public byte[]? FileData { get; set; }

		private int _id;
		private string _fileName = string.Empty;
	}

	public class PersonalDocumentDataDto : DataFile
	{
		public int PersonalDocumentTypeId
		{
			get => _typeId;
			set
			{
				if ( value <= 0 )
					throw new ArgumentNullException("Null input data");
				_typeId = value;
			}
		}
		public string TextData
		{
			get => _text;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new ArgumentNullException("Null input data");
				_text = value;
			}
		}

		private int _typeId;
		private string _text = string.Empty;
	}
}