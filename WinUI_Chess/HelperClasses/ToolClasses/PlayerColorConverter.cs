// 
// 
namespace TrevyBurgess.Games.TrevyChess.ChessGameUI
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Data;
    using TrevyBurgess.Games.TrevyChess.ChessBoardLogic;

    public class PlayerColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ChessPieceColor pieceColor = (ChessPieceColor)value;

            if (pieceColor == ChessPieceColor.Black)
                return 0;
            else if (pieceColor == ChessPieceColor.White)
                return 1;
            else
                throw new NotSupportedException("Only ChessPieceColor.Black and ChessPieceColor.White is meaningful.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
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
}
