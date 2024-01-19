using DocsMan.Domain.Entity;

namespace DocsMan.Domain.BinderEntity
{
	public class User_Role
	{
		public int RoleId { get; set; }
		public Role Role { get; set; }

		public int UserId { get; set; }
		public User User { get; set; }
	}
}