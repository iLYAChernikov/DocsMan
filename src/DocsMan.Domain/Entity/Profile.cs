namespace DocsMan.Domain.Entity
{
	public class Profile
	{
		public int Id { get; set; }

		public int UserId
		{
			get => _userId;
			set
			{
				if ( value <= 0 )
					throw new NullReferenceException("Ошибка заполнения пользователя");
				_userId = value;
			}
		}
		public User User { get; set; }

		public string SurName
		{
			get => _surName;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new NullReferenceException("Ошибка заполнения фамилии");
				_surName = value;
			}
		}
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
		public string LastName
		{
			get => _lastName;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new NullReferenceException("Ошибка заполнения отчества");
				_lastName = value;
			}
		}

		public DateTime Birthdate { get; set; } = DateTime.Now;
		public Gender Gender { get; set; } = Gender.Man;
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
		public string? PhoneNumber { get; set; } = string.Empty;

		private int _userId;
		private string _surName = string.Empty;
		private string _name = string.Empty;
		private string _lastName = string.Empty;
		private string _email = string.Empty;
	}

	public enum Gender : short
	{
		Woman = 0,
		Man = 1
	}
}