using Jamesnet.Foundation;
using System.Windows.Controls;
using System.Windows.Interop;

namespace Leagueoflegends.Store.UI.Views;

public class ChatContent : ContentControl, IView
{
    public ChatContent()
    { 
        DefaultStyleKey = typeof(ChatContent);
    }
}
