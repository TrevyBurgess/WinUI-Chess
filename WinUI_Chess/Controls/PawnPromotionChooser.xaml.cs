// Promotion is not limited to pieces that have already been captured.
// Schiller, Eric (2003), Official Rules of Chess (2nd ed.), Cardoza, ISBN 978-1-58042-092-1
namespace CyberFeedForward.WinUI_Chess.Controls;

using CyberFeedForward.ChessBoardLogic.ChessPiece;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

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
            BlackQueen.Visibility = Visibility.Visible;
            BlackKnight.Visibility = Visibility.Visible;
            BlackBishop.Visibility = Visibility.Visible;
            BlackRook.Visibility = Visibility.Visible;
        }
        else if (pieceColor == ChessPieceColor.White)
        {
            WhiteQueen.Visibility = Visibility.Visible;
            WhiteKnight.Visibility = Visibility.Visible;
            WhiteBishop.Visibility = Visibility.Visible;
            WhiteRook.Visibility = Visibility.Visible;
        }

        this.pieceColor = pieceColor;
    }

    #region Selection code
    private void BlackQueen_MouseDown(object sender, MouseButtonEventArgs e)
    {
        PieceSelected = ChessPieceType.Queen;
        Source = BlackQueen.Source;
        Close();
    }

    private void BlackKnight_MouseDown(object sender, Input.MouseButtonEventArgs e)
    {
        PieceSelected = ChessPieceType.Knight;
        Source = BlackKnight.Source;
        Close();
    }

    private void BlackBishop_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        PieceSelected = ChessPieceType.Bishop;
        Source = BlackBishop.Source;
        Close();
    }

    private void BlackRook_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        PieceSelected = ChessPieceType.Rook;
        Source = BlackRook.Source;
        Close();
    }

    private void WhiteQueen_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        PieceSelected = ChessPieceType.Queen;
        Source = WhiteQueen.Source;
        Close();
    }

    private void WhiteKnight_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        PieceSelected = ChessPieceType.Knight;
        Source = WhiteKnight.Source;
        Close();
    }

    private void WhiteBishop_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        PieceSelected = ChessPieceType.Bishop;
        Source = WhiteBishop.Source;
        Close();
    }

    private void WhiteRook_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        PieceSelected = ChessPieceType.Rook;
        Source = WhiteRook.Source;
        Close();
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
