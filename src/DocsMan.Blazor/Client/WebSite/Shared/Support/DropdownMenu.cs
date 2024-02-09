namespace DocsMan.Blazor.Client.WebSite.Shared.Support;

public class DropdownMenu
{
	public string Title { get; set; } = string.Empty;
	public string Icon { get; set; } = string.Empty;
	public bool IsOpen { get; set; } = false;
	public List<SubMenu> Submenus { get; set; } = new();
}

public class SubMenu
{
	public string Title { get; set; } = string.Empty;
	public string Icon { get; set; } = string.Empty;
	public string HrefUrl { get; set; } = string.Empty;
}

public static class CSS_Styles
{
	public static List<string> OpenCloseStyles => new() { "", "" };
}

public static class HeaderNavMenuList
{
	public static List<DropdownMenu> headerMenu = new()
	{
		new ()
		{
			Title = "User",
			Icon = "fas fa-user-tie",
			Submenus = new()
			{
				new()
				{
					Title = "Профиль",
					Icon = "fas fa-user-tie",
					HrefUrl = "User/Profile"
				},
				new ()
				{
					Title = "Настройки",
					HrefUrl = "User/Settings",
					Icon = "fas fa-user-cog"
				}
			}
		}
	};
}

public static class SideBarNavMenuList
{
	public static List<DropdownMenu> sidebarMenu = new()
	{
		new ()
		{
			Title = "UserPanel",
			Icon = "far fa-user",
			Submenus = new()
			{
				new ()
				{
					Title = "Документы",
					HrefUrl = "User/Docs",
					Icon = "fas fa-id-card"
				},
				new ()
				{
					Title = "Work",
					HrefUrl = "User/Workplace",
					Icon = "fas fa-briefcase"
				},
				new ()
				{
					Title = "Группы",
					HrefUrl = "User/Groups",
					Icon = "fas fa-user-friends"
				}
			}
		},
	};
}