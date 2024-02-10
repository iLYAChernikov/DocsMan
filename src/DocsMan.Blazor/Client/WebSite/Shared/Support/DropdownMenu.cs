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

	public static List<SubMenu> adminMenu = new()
	{
		new ()
		{
			Title = "AdminPanel",
			Icon = "far fa-user-secret",
			HrefUrl = "Admin/Panel"
		}
	};

	public static List<SubMenu> userMenu = new()
	{
		new ()
		{
			Title = "Manager",
			Icon = "fas fa-folder-open",
			HrefUrl = "User/Files"
		},
	};

	public static List<SubMenu> adminPanelMenu = new()
	{
		new ()
		{
			Title = "Home",
			Icon = "far fa-home",
			HrefUrl = "/"
		},
		new ()
		{
			Title = "Roles",
			Icon = "fas fa-user-tag",
			HrefUrl = "Admin/Role"
		},
		new ()
		{
			Title = "DocsTypes",
			Icon = "fas fa-id-card",
			HrefUrl = "Admin/DocsType"
		},
	};
}