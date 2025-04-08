//
//
namespace CyberFeedForward.ChessBoardLogic.HelperClasses;

using CyberFeedForward.ChessBoardLogic.ChessBoardClasses;
using CyberFeedForward.ChessBoardLogic.ChessPiece;
using System;
using System.Diagnostics.Contracts;
using System.Text;


internal static class ChessBoardEncoder
{
    internal static string EncodeBoard(ChessPieceColor playerColor, ChessPiece[] pieces, ColPos enPassantColumn)
    {
        var sb = new StringBuilder();

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
            int index = i * 4 + 2;

            Library.GetPieceType(chessBoardState[index++], out pieceType, out pieceColor);

            pieces[i] = ChessPieceFactory.GetNewChessPiece(
                board,
                pieceType,
                pieceColor,
                new ChessPieceLocation(Library.ParseColPos(chessBoardState[index++]), Library.ParseRowPos(chessBoardState[index++])),
                chessBoardState[index++] == '_',
                i);
        }
    }

    /// <summary>
    /// parse ChessPieceColor
    /// </summary>
    internal static ChessPieceColor GetPlayerColor(string boardState)
    {
        Contract.Requires(0 <= boardState.Length - 2);

        var playerTurn = boardState[..2];
        if (playerTurn == "B_")
            return ChessPieceColor.Black;
        else
            return ChessPieceColor.White;
    }
}
