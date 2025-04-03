// Promotion is not limited to pieces that have already been captured.
// Schiller, Eric (2003), Official Rules of Chess (2nd ed.), Cardoza, ISBN 978-1-58042-092-1
namespace TrevyBurgess.Games.TrevyChess.ChessGameUI
{
    using Microsoft.UI.Xaml.Media;
    using System.Windows;
    using System.Windows.Media;
    using TrevyBurgess.Games.TrevyChess.ChessBoardLogic;

    /// <summary>
    /// Interaction logic for HelpPage.xaml
    /// </summary>
    public partial class PawnPromotionChooser : Window
    {
        /// <summary>
        /// Piece user wants to promote pawn
        /// </summary>
        public ChessPieceType PieceSelected { get; private set; }

        /// <summary>
        /// The image source of the promoted pawn
        /// </summary>
        public ImageSource Source { get; private set; }

        /// <summary>
        /// Color of selected piece
        /// </summary>
        private ChessPieceColor pieceColor;

        public PawnPromotionChooser(ChessPieceColor pieceColor)
        {
            //InitializeComponent();

            Instructions.Text = Properties.Resources.PawnPromotion_Instructions;

            if (pieceColor == ChessPieceColor.Black)
            {
                BlackQueen.Visibility = System.Windows.Visibility.Visible;
                BlackKnight.Visibility = System.Windows.Visibility.Visible;
                BlackBishop.Visibility = System.Windows.Visibility.Visible;
                BlackRook.Visibility = System.Windows.Visibility.Visible;
            }
            else if (pieceColor == ChessPieceColor.White)
            {
                WhiteQueen.Visibility = System.Windows.Visibility.Visible;
                WhiteKnight.Visibility = System.Windows.Visibility.Visible;
                WhiteBishop.Visibility = System.Windows.Visibility.Visible;
                WhiteRook.Visibility = System.Windows.Visibility.Visible;
            }

            this.pieceColor = pieceColor;
        }

        #region Selection code
        private void BlackQueen_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PieceSelected = ChessPieceType.Queen;
            Source = BlackQueen.Source;
            this.Close();
        }

        private void BlackKnight_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PieceSelected = ChessPieceType.Knight;
            Source = BlackKnight.Source;
            this.Close();
        }

        private void BlackBishop_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PieceSelected = ChessPieceType.Bishop;
            Source = BlackBishop.Source;
            this.Close();
        }

        private void BlackRook_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PieceSelected = ChessPieceType.Rook;
            Source = BlackRook.Source;
            this.Close();
        }

        private void WhiteQueen_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PieceSelected = ChessPieceType.Queen;
            Source = WhiteQueen.Source;
            this.Close();
        }

        private void WhiteKnight_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PieceSelected = ChessPieceType.Knight;
            Source = WhiteKnight.Source;
            this.Close();
        }

        private void WhiteBishop_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PieceSelected = ChessPieceType.Bishop;
            Source = WhiteBishop.Source;
            this.Close();
        }

        private void WhiteRook_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PieceSelected = ChessPieceType.Rook;
            Source = WhiteRook.Source;
            this.Close();
        }
        #endregion

        private void Window_Closed(object sender, System.EventArgs e)
        {
            // We use the default selection if user closes window without selecting
            if (PieceSelected == ChessPieceType.EmptySquare)
            {
                PieceSelected = ChessPieceType.Queen;

                if (pieceColor == ChessPieceColor.Black)
                {
                    Source = BlackQueen.Source;
                }
                else
                {
                    Source = WhiteQueen.Source;
                }
            }
        }
    }
}
