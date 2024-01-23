namespace DocsMan.Blazor.Server.DataStorage
{
	public static class PathStorage
	{
		public static string DB_Dir => Directory.GetCurrentDirectory() + @"\DataStorage\DataBase\";
		public static string PersonalDocs_Dir => Directory.GetCurrentDirectory() + @"\DataStorage\UserDocuments\";
		public static string Files_Dir => Directory.GetCurrentDirectory() + @"\DataStorage\UserFiles\";
	}
}