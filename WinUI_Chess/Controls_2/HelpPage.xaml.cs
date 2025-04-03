//
//
namespace TrevyBurgess.Games.TrevyChess.ChessGameUI
{
    using Microsoft.UI.Xaml;
    using System.Windows;
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
}
