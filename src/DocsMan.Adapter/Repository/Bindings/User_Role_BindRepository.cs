using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.Domain.BinderEntity;
using Microsoft.EntityFrameworkCore;

namespace DocsMan.Adapter.Repository.Bindings
{
	public class User_Role_BindRepository : IBindingRepository<User_Role>
	{
		private DocsMan_DBContext _context;

		public User_Role_BindRepository(DocsMan_DBContext context)
		{
			_context = context;
		}

		public async Task CreateBindAsync(User_Role? bind)
		{
			if (bind == null || bind.UserId <= 0 || bind.RoleId <= 0)
				throw new ArgumentNullException("Null input data");
			await _context.User_Roles.AddAsync(bind);
		}

		public async Task DeleteBindAsync(User_Role? bind)
		{
			if (bind == null || bind.UserId <= 0 || bind.RoleId <= 0)
				throw new ArgumentNullException("Null input data");
			_context.User_Roles.Remove(bind);
		}

		public async Task<IEnumerable<User_Role>?> GetAllBinds()
		{
			return _context.User_Roles
				.Include(x => x.User)
				.Include(x => x.Role);
		}

		public async Task<IEnumerable<User_Role>?> GetAllBindsNoTracking()
		{
			return _context.User_Roles
				.Include(x => x.User)
				.Include(x => x.Role)
				.AsNoTracking();
		}
	}
}