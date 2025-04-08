//
//
namespace CyberFeedForward.ChessBoardLogic.HelperClasses;

using CyberFeedForward.ChessBoardLogic.ChessBoardClasses;
using CyberFeedForward.ChessBoardLogic.ChessPiece;
using CyberFeedForward.ChessBoardLogic.ChessPiece.Internals;
using System;

internal static class ChessPieceFactory
{
    /// <summary>
    /// create a new chess piece based on piece color and type
    /// </summary>
    internal static ChessPiece GetNewChessPiece(ChessBoard board, ChessPieceType pieceType, ChessPieceColor color, ChessPieceLocation position, bool isInitialPosition, short id)
    {
        switch (pieceType)
        {
            case ChessPieceType.King:
                return new King(board, color, position, isInitialPosition, id);

            case ChessPieceType.Queen:
                return new Queen(board, color, position, isInitialPosition, id);

            case ChessPieceType.Bishop:
                return new Bishop(board, color, position, isInitialPosition, id);

            case ChessPieceType.Knight:
                return new Knight(board, color, position, isInitialPosition, id);

            case ChessPieceType.Rook:
                return new Rook(board, color, position, isInitialPosition, id);

            case ChessPieceType.Pawn:
                if (color == ChessPieceColor.Black)
                    return new PawnBlack(board, color, position, isInitialPosition, id);
                else
                    return new PawnWhite(board, color, position, isInitialPosition, id);

            default:
                throw new NotSupportedException("Chess piece type: " + pieceType.ToString() + " is not allowed");
        }
    }
}
