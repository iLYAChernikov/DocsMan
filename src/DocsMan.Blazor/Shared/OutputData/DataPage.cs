namespace DocsMan.Blazor.Shared.OutputData
{
	/// <summary>
	/// Заголовок Страницы с данными
	/// <br>для запроса страницы с номером страницы и её размером</br>
	/// </summary>
	public class DataPageHeader
	{
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public DataPageHeader() { }

		public DataPageHeader(int pageNumber, int pageSize)
		{
			PageNumber = pageNumber;
			PageSize = pageSize;
		}
		public DataPageHeader(DataPageHeader pageHeader)
		{
			PageNumber = pageHeader.PageNumber;
			PageSize = pageHeader.PageSize;
		}
	}

	/// <summary>
	/// Страница с данными
	/// <br>для отображения Страницы с данными - её номером, размером</br>
	/// <br>и количеством страниц при таком размере</br>
	/// </summary>
	public class DataPage<T> : DataPageHeader
	{
		public IEnumerable<T>? Items { get; }
		public int PagesCount => Items == null ? 0 : Items.Count() / PageSize + 1;
		public DataPage() { }

		/// <summary>
		/// Создание страницы с отсечением данных по размеру Заголовка
		/// </summary>
		public DataPage(DataPageHeader pageHeader, IEnumerable<T>? fullData) : base(pageHeader)
		{
			if ( fullData != null )
				Items = fullData
						.Skip(( pageHeader.PageNumber - 1 ) * pageHeader.PageSize)
						.Take(pageHeader.PageSize);
			else { Items = null; }
		}
	}
}