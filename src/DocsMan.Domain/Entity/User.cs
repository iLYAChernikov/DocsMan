namespace DocsMan.Domain.Entity
{
	public class User
	{
		public int Id
		{
			get => _id;
			set
			{
				if ( value < 0 )
					throw new NullReferenceException("Ошибка заполнения id");
				_id = value;
			}
		}
		public string Email
		{
			get => _email;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new NullReferenceException("Ошибка заполнения почты");
				_email = value;
			}
		}
		public string Password
		{
			get => _password;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new NullReferenceException("Ошибка заполнения пароля");
				_password = value;
			}
		}

		private int _id;
		private string _email = string.Empty;
		private string _password = string.Empty;
	}
}