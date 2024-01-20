using DocsMan.Domain.Entity;

namespace DocsMan.Domain.BinderEntity
{
	public class User_Role
	{
		public int RoleId
		{
			get => _roleId;
			set
			{
				if ( value <= 0 )
					throw new NullReferenceException("Ошибка заполнения роли");
				_roleId = value;
			}
		}
		public Role Role { get; set; }

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

		private int _roleId;
		private int _userId;
	}
}