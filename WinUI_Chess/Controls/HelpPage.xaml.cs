//
//
namespace CyberFeedForward.WinUI_Chess.Controls;

using CyberFeedForward.WinUI_Chess.HelperClasses.ToolClasses;
using Microsoft.UI.Xaml;
using WinUI_Chess.Properties;

/// <summary>
/// Interaction logic for HelpPage.xaml
/// </summary>
public partial class HelpPage : Window
{
    public HelpPage()
    {
        //InitializeComponent();

        ChessAiLogic.Source = HelperMethods.GetImageSource(Resources.ChessMoveCalculation);
    }
}
