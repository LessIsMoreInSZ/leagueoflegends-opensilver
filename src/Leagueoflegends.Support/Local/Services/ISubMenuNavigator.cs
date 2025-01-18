using Leagueoflegends.Support.Local.Models;
using System;
using System.Collections.Generic;

namespace Leagueoflegends.Support.Local.Services;

public interface ISubMenuNavigator
{
    event Action<List<SubMenuItem>> SubMenuItemsUpdated;
    void UpdateSubMenuItems(string mainMenu);
    List<SubMenuItem> GetSubMenuItems(string mainMenu);
    void NavigateToSubMenu(SubMenuItem subMenuItem);
    void NavigateToMainMenu();
}
