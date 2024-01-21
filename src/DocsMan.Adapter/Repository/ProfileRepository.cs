using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace DocsMan.Adapter.Repository
{
	public class ProfileRepository : IRepository<Profile>
	{
		private DocsMan_DBContext _context;

		public ProfileRepository(DocsMan_DBContext context)
		{
			_context = context;
		}

		public async Task CreateAsync(Profile? entity)
		{
			if ( entity == null ) throw new ArgumentNullException("Null input data");
			await _context.Profiles.AddAsync(entity);
		}

		public async Task DeleteAsync(Profile? entity)
		{
			if ( entity == null ) throw new ArgumentNullException("Null input data");
			_context.Profiles.Remove(entity);
		}

		public async Task<IEnumerable<Profile>?> GetAllAsync()
		{
			return _context.Profiles
				.Include(x => x.User);
		}

		public async Task<Profile?> GetOneAsync(int id)
		{
			if ( id <= 0 ) throw new ArgumentNullException("Null input data");
			var ent = await _context.Profiles
				.Include(x => x.User)
				.FirstOrDefaultAsync(x => x.Id == id);
			if ( ent == null ) throw new NullReferenceException("Not found");
			return ent;
		}

		public async Task<Profile?> GetOneAsync(int firstId, int secondId)
		{
			throw new NotSupportedException("Not a composite key");
		}

		public async Task UpdateAsync(Profile? entity)
		{
			if ( entity == null ) throw new ArgumentNullException("Null input data");
			_context.Profiles.Update(entity);
		}
	}
}