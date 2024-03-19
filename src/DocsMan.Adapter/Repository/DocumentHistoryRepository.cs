using DocsMan.App.Storage.RepositoryPattern;
using DocsMan.Domain.Entity;

using Microsoft.EntityFrameworkCore;

namespace DocsMan.Adapter.Repository
{
	public class DocumentHistoryRepository : IRepository<DocumentHistory>
	{
		private DocsMan_DBContext _context;

		public DocumentHistoryRepository(DocsMan_DBContext context)
		{
			_context = context;
		}

		public async Task CreateAsync(DocumentHistory? entity)
		{
			if ( entity == null )
				throw new ArgumentNullException("Null input data");
			await _context.DocumentHistories.AddAsync(entity);
		}

		public async Task DeleteAsync(DocumentHistory? entity)
		{
			if ( entity == null )
				throw new NullReferenceException("Not found");
			_context.DocumentHistories.Remove(entity);
		}

		public async Task DeleteAsync(object key)
		{
			throw new NotSupportedException("Has a composite key");
		}

		public async Task DeleteAsync(object firstKey, object secondKey)
		{
			int firstId = (int) firstKey;
			string secondId = (string) secondKey;

			if ( firstId <= 0 || string.IsNullOrWhiteSpace(secondId) )
				throw new ArgumentNullException("Null input data");
			var ent = await _context.DocumentHistories
				.FirstOrDefaultAsync(x => x.DocumentId == firstId && x.DateTimeOfChanges == secondId);
			if ( ent == null )
				throw new NullReferenceException("Not found");
			_context.Remove(ent);
		}

		public async Task<IEnumerable<DocumentHistory>?> GetAllAsync()
		{
			return _context.DocumentHistories;
		}

		public async Task<DocumentHistory> GetOneAsync(object key)
		{
			throw new NotSupportedException("Has a composite key");
		}

		public async Task<DocumentHistory> GetOneAsync(object firstKey, object secondKey)
		{
			int firstId = (int) firstKey;
			string secondId = (string) secondKey;

			if ( firstId <= 0 || string.IsNullOrWhiteSpace(secondId) )
				throw new ArgumentNullException("Null input data");
			var ent = await _context.DocumentHistories
				.FirstOrDefaultAsync(x => x.DocumentId == firstId && x.DateTimeOfChanges == secondId);
			if ( ent == null )
				throw new NullReferenceException("Not found");
			return ent;
		}
	}
}