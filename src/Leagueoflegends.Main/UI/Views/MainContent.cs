using Jamesnet.Foundation;
using Jamesnet.Platform.OpenSilver;
using System.Windows.Controls;

namespace Leagueoflegends.Main.UI.Views;

public class MainContent : ContentControl, IView
{
    public MainContent()
    { 
        DefaultStyleKey = typeof(MainContent);
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var content = GetTemplateChild("PART_CONTENT") as OpenSilverLayer;

        var aa = content;
    }
}
