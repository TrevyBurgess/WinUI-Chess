using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrevyBurgess.Games.TrevyChess.ChessBoardLogic
{
    public interface IChessBoard
    {
        /// <summary>
        /// New game board state
        /// </summary>
        string NewGameBoardState { get; }

        /// <summary>
        /// Return board to its starting state
        /// </summary>
        void ResetBoard();

        /// <summary>
        /// get/Set state of the board
        /// </summary>
        string BoardState { get; set; }

        /// <summary>
        /// Get locations of all chess pieces
        /// </summary>
        ChessPieceLocation[] GetChessPieceLocations();

        /// <summary>
        /// Move chess piece, disregarding chess rules
        /// </summary>
        /// <returns>Board state</returns>
        string MoveFreeStyle(ChessPieceLocation oldPosition, ChessPieceLocation newPosition);

        /// <summary>
        /// Move chess piece
        /// </summary>
        ChessMove Move(ChessPieceLocation oldPosition, ChessPieceLocation newPosition);

        /// <summary>
        /// Return if chess piece can move to specified location
        /// </summary>
        bool CanMove(ChessPieceLocation oldPosition, ChessPieceLocation newPosition);

        /// <summary>
        /// Return if chess piece in current position can move
        /// </summary>
        bool CanMove(ChessPieceLocation location);

        /// <summary>
        /// Get chess piece at specified location
        /// </summary>
        ChessPiece this[ColPos colPos, RowPos rowPos] { get; }

        /// <summary>
        /// Get chess piece at specified location
        /// </summary>
        ChessPiece this[int colPos, int rowPos] { get; }

        /// <summary>
        /// Promote pawn in specific location to a power
        /// </summary>
        /// <param name="chessPieceType">Piece you want to promote pawn to</param>
        /// <param name="colPos">Column position of pawn</param>
        /// <param name="rowPos">Row position of pawn</param>
        void PromotePawn(ChessPieceType chessPieceType, ColPos colPos, RowPos rowPos);

        /// <summary>
        /// Return list of all possible chess moves user can make, depending on whose turn it is
        /// </summary>
        //List<ChessMove> PossibleChessMoves(ChessBoard chessBoard);

        /// <summary>
        /// Return if specified king is in check
        /// </summary>
        bool IsKingInCheck(ChessPieceColor color);

        /// <summary>
        /// Get/Set current player color
        /// </summary>
        ChessPieceColor CurrentPlayerColor { get; set; }

        // Undo-Redo list

        bool CanUndoMove();

        void UndoMove();

        bool CanRedoMove();

        void RedoMove();
    }
}
