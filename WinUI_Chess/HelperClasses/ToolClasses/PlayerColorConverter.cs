// 
// 
namespace CyberFeedForward.WinUI_Chess.HelperClasses.ToolClasses;

using CyberFeedForward.ChessBoardLogic.ChessPiece;
using Microsoft.UI.Xaml.Data;
using System;

public partial class PlayerColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        ChessPieceColor pieceColor = (ChessPieceColor)value;

        if (pieceColor == ChessPieceColor.Black)
            return 0;
        else if (pieceColor == ChessPieceColor.White)
            return 1;
        else
            throw new NotSupportedException("Only ChessPieceColor.Black and ChessPieceColor.White is meaningful.");
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        int colorIndex = (int)value;
        if (colorIndex == 0)
            return ChessPieceColor.Black;
        else if (colorIndex == 1)
            return ChessPieceColor.White;
        else
            throw new NotSupportedException("only inputs of 0 and 1 are meaningful.");
    }
}
