namespace DocsMan.App.Storage.Transaction
{
	public interface IUnitWork
	{
		public Task Commit();
		public Task Rollback();
	}
}