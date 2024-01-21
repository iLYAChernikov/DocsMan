namespace DocsMan.Domain.Entity
{
	public class Profile
	{
		public int Id
		{
			get => _id;
			set
			{
				if ( value < 0 )
					throw new ArgumentNullException("Ошибка заполнения id профиля");
				_id = value;
			}
		}

		public int UserId
		{
			get => _userId;
			set
			{
				if ( value <= 0 )
					throw new ArgumentNullException("Ошибка заполнения id пользователя в профиле");
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
					throw new ArgumentNullException("Ошибка заполнения фамилии в профиле");
				_surName = value;
			}
		}
		public string Name
		{
			get => _name;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new ArgumentNullException("Ошибка заполнения имени в профиле");
				_name = value;
			}
		}
		public string LastName
		{
			get => _lastName;
			set
			{
				if ( string.IsNullOrWhiteSpace(value) )
					throw new ArgumentNullException("Ошибка заполнения отчества в профиле");
				_lastName = value;
			}
		}

		public DateTime Birthdate { get; set; } = DateTime.Now;
		public Gender Gender { get; set; } = Gender.Man;
		public string Email => User.Email;
		public string? PhoneNumber { get; set; } = string.Empty;

		private int _id;
		private int _userId;
		private string _surName = string.Empty;
		private string _name = string.Empty;
		private string _lastName = string.Empty;
	}

	public enum Gender : short
	{
		Woman = 0,
		Man = 1
	}
}