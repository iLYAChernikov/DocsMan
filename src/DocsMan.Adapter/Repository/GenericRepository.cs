using DocsMan.App.Storage.RepositoryPattern;

namespace DocsMan.Adapter.Repository
{
	public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		private DocsMan_DBContext _context;

		public GenericRepository(DocsMan_DBContext context)
		{
			_context = context;
		}

		public async Task CreateAsync(TEntity entity)
		{
			if ( entity == null ) throw new Exception("Null input data");
			await _context.Set<TEntity>().AddAsync(entity);
		}

		public async Task DeleteAsync(TEntity entity)
		{
			if ( entity == null ) throw new Exception("Null input data");
			_context.Remove(entity);
		}

		public async Task<IEnumerable<TEntity>?> GetAllAsync()
		{
			return _context.Set<TEntity>();
		}

		public async Task<TEntity?> GetOneAsync(int id)
		{
			if ( id <= 0 ) throw new Exception("Null input data");
			return await _context.Set<TEntity>().FindAsync(id);
		}

		public async Task<TEntity?> GetOneAsync(int firstId, int secondId)
		{
			if ( firstId <= 0 || secondId <= 0 ) throw new Exception("Null input data");
			return await _context.Set<TEntity>().FindAsync(firstId, secondId);
		}

		public async Task UpdateAsync(TEntity entity)
		{
			if ( entity == null ) throw new Exception("Null input data");
			_context.Update(entity);
		}
	}
}