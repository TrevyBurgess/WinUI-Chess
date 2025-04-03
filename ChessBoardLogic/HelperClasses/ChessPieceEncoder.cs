using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrevyBurgess.Games.TrevyChess.ChessBoardLogic.Internal;

namespace TrevyBurgess.Games.TrevyChess.ChessBoardLogic.HelperClasses
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Text;
    using TrevyBurgess.Games.TrevyChess.ChessBoardLogic.Internal;

    internal static class ChessBoardEncoder
    {
        internal static string EncodeBoard(ChessPieceColor playerColor, ChessPiece[] pieces, ColPos enPassantColumn)
        {
            StringBuilder sb = new StringBuilder();

            if (playerColor == ChessPieceColor.Black)
                sb.Append("B_");
            else
                sb.Append("W_");

            foreach (ChessPiece piece in pieces)
            {
                if (piece == null)
                {
                    throw new ArgumentException();
                }
                else
                {
                    // piece type and color
                    sb.Append(Library.GetPieceCode(piece.PieceType, piece.PieceColor));

                    // piece location
                    sb.Append(((int)piece.Location.ColLoc).ToString() + ((int)piece.Location.RowLoc).ToString());

                    // isInitialPositin
                    if (piece.InitialPosition)
                        sb.Append("_");
                    else
                        sb.Append("-");

                }
            }

            // add en passant location
            sb.Append(((int)enPassantColumn).ToString());

            // Mark user is player
            sb.Append("U");

            return sb.ToString();
        }

        internal static void DecodeBoardState(ChessBoard board, string chessBoardState, out ChessPieceColor playerColor, ChessPiece[] pieces)
        {
            Contract.Requires<ArgumentNullException>(chessBoardState != null);
            Contract.Requires<ArgumentException>(chessBoardState.Length == ChessBoard.C_BoardStateStringLength);
            Contract.Requires<ArgumentException>(pieces.Length * 4 + 4 == ChessBoard.C_BoardStateStringLength);

            playerColor = GetPlayerColor(chessBoardState);

            // Get pieces
            for (short i = 0; i < pieces.Length; i++)
            {
                ChessPieceType pieceType;
                ChessPieceColor pieceColor;
                int index = (i * 4) + 2;

                Library.GetPieceType(chessBoardState[index++], out pieceType, out pieceColor);

                pieces[i] = ChessPieceFactory.GetNewChessPiece(
                    board,
                    pieceType,
                    pieceColor,
                    new ChessPieceLocation(Library.ParseColPos(chessBoardState[index++]), Library.ParseRowPos(chessBoardState[index++])),
                    chessBoardState[index++] == '_' ? true : false,
                    i);
            }
        }

        /// <summary>
        /// parse ChessPieceColor
        /// </summary>
        internal static ChessPieceColor GetPlayerColor(string boardState)
        {
            Contract.Requires(0 <= (boardState.Length - 2));

            var playerTurn = boardState.Substring(0, 2);
            if (playerTurn == "B_")
                return ChessPieceColor.Black;
            else
                return ChessPieceColor.White;
        }
    }
}
