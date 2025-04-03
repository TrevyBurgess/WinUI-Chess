//
//
namespace TrevyBurgess.Games.TrevyChess.ChessGameUI
{
    using System.Windows;
    using GameAiProperties = TrevyBurgess.Games.TrevyChess.ChessGameAI.Properties;

    /// <summary>
    /// Interaction logic for HelpPage.xaml
    /// </summary>
    public partial class HelpPage : Window
    {
        public HelpPage()
        {
            InitializeComponent();

            ChessAiLogic.Source = HelperMethods.GetImageSource(GameAiProperties.Resources.ChessMoveCalculation);
        }
    }
}
