namespace DocsMan.App.Storage.RepositoryPattern
{
	public interface IBindingRepository<TBind> where TBind : class
	{
		public Task<IEnumerable<TBind>?> GetAllBinds();
		public Task<IEnumerable<TBind>?> GetAllBindsNoTracking();
		public Task CreateBindAsync(TBind? bind);
		public Task DeleteBindAsync(TBind? bind);
	}
}