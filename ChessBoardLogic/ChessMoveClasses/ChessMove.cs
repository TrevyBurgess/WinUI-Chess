//
//
namespace TrevyBurgess.Games.TrevyChess.ChessBoardLogic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class ChessMove
    {
        public ChessMove() { }

        public ChessMove(ColPos startCol, RowPos startRow, ColPos endCol, RowPos endRow)
        {
            this.StartLocation = new ChessPieceLocation(startCol, startRow);
            this.EndLocation = new ChessPieceLocation(endCol, endRow);
        }

        public ChessMove(ChessPieceLocation startLocation, ChessPieceLocation endLocation)
        {
            this.StartLocation = startLocation;
            this.EndLocation = endLocation;
        }

        /// <summary>
        /// Old Rook position
        /// </summary>
        public ChessPieceLocation StartLocation;

        /// <summary>
        /// New Rook position
        /// </summary>
        public ChessPieceLocation EndLocation;

        public override string ToString()
        {
            return "Start: " + StartLocation.ToString() + " End: " + EndLocation;
        }
    }
}
