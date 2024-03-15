using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.Domain.BinderEntity;
using Microsoft.EntityFrameworkCore;

namespace DocsMan.Adapter.Repository.Bindings
{
	public class Profile_Notify_BindRepository : IBindingRepository<Profile_Notify>
	{
		private DocsMan_DBContext _context;

		public Profile_Notify_BindRepository(DocsMan_DBContext context)
		{
			_context = context;
		}

		public async Task CreateBindAsync(Profile_Notify? bind)
		{
			if (bind == null || bind.ProfileId <= 0 || bind.NotificationId <= 0)
				throw new ArgumentNullException("Null input data");
			await _context.Profile_Notifications.AddAsync(bind);
		}

		public async Task DeleteBindAsync(Profile_Notify? bind)
		{
			if (bind == null || bind.ProfileId <= 0 || bind.NotificationId <= 0)
				throw new ArgumentNullException("Null input data");
			_context.Profile_Notifications.Remove(bind);
		}

		public async Task<IEnumerable<Profile_Notify>?> GetAllBinds()
		{
			return _context.Profile_Notifications
				.Include(x => x.Profile)
				.Include(x => x.Notification);
		}

		public async Task<IEnumerable<Profile_Notify>?> GetAllBindsNoTracking()
		{
			return _context.Profile_Notifications
				.Include(x => x.Profile)
				.Include(x => x.Notification)
				.AsNoTracking();
		}
	}
}