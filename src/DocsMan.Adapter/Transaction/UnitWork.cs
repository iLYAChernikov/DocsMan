using DocsMan.App.Storage.Transaction;

namespace DocsMan.Adapter.Transaction
{
	public class UnitWork : IUnitWork
	{
		private DocsMan_DBContext _context;
		public UnitWork(DocsMan_DBContext context)
		{
			_context = context;
		}

		public async Task Commit()
		{
			try
			{
				await _context.SaveChangesAsync();
			}
			catch ( Exception ex )
			{
				throw new Exception($"Internal error of writing database.\n Error message:{ex.Message}\n" +
					$"Inner ex message: {ex.InnerException?.Message}");
			}
		}

		public async Task Rollback()
		{
			_context.Dispose();
		}
	}
}