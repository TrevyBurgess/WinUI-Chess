//
//
namespace CyberFeedForward.ChessBoardLogic.ChessPiece.Internals;

using CyberFeedForward.ChessBoardLogic.ChessBoardClasses;
using CyberFeedForward.ChessBoardLogic.ChessMoveClasses;
using CyberFeedForward.ChessBoardLogic.ChessPiece;
using CyberFeedForward.ChessBoardLogic.HelperClasses;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

/// <summary>
/// Encapsulate Pawn behavior
/// </summary>
internal class PawnBlack : ChessPiece
{
    internal PawnBlack(ChessBoard board, ChessPieceColor Color, ChessPieceLocation position, bool initialPosition, short id) :
        base(board, Color, ChessPieceType.Pawn, position, initialPosition, id) { }

    #region Public API
    public override int Weight { get { return 100; } }

    /// <summary>
    /// Return if a specified move is legal for a Pawn
    /// </summary>
    public override bool CanMove(ChessPieceLocation newPosition)
    {
        Contract.Assume(ValidateBounds.IsInBounds(newPosition.RowLoc));
        Contract.Assume(ValidateBounds.IsInBounds(newPosition.ColLoc));

        // End position is your own piece
        if (Board[newPosition.ColLoc, newPosition.RowLoc] != null &&
            Board[newPosition.ColLoc, newPosition.RowLoc].PieceColor == ChessPieceColor.Black)
            return false;

        return CanCover(newPosition);
    }

    /// <summary>
    /// Return if a specified move is legal for a Rook
    /// </summary>
    internal override bool CanCover(ChessPieceLocation newPosition)
    {
        // Input values.
        int iStartCol = (int)Location.ColLoc;
        int iEndCol = (int)newPosition.ColLoc;
        int iStartRow = (int)Location.RowLoc;
        int iEndRow = (int)newPosition.RowLoc;

        // Moving forward
        if (iEndCol == iStartCol)
        {
            // Moves 1 space down
            if (iEndRow == iStartRow - 1 &&
                Board[iEndCol, iStartRow - 1] == null)
            {
                return true;
            }
            // Move 2 spaces down
            else if (iStartRow == 7 && iEndRow == iStartRow - 2 &&
                Board[iEndCol, iStartRow - 1] == null &&
                Board[iEndCol, iStartRow - 2] == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // Capturing pieces
        else if (iEndCol == iStartCol + 1 || iEndCol == iStartCol - 1)
        {
            // Capturing piece
            if (iEndRow == iStartRow - 1 && Board[iEndCol, iEndRow] != null)
            {
                return true;
            }
            // Check En Passant
            else if (newPosition.RowLoc == RowPos.R3 && Board.undoList.GetEnPassantColumn() == newPosition.ColLoc)
            {
                if (Board[newPosition.ColLoc, RowPos.R4] is PawnWhite)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Return if a specified move is legal for a Pawn
    /// </summary>
    public override bool CanMove()
    {
        // Input values.
        int iStartCol = (int)Location.ColLoc;
        int iStartRow = (int)Location.RowLoc;

        if (iStartRow > 1)
        {
            if (CanMove(new ChessPieceLocation((ColPos)iStartCol, (RowPos)(iStartRow - 1))))
                return true;

            if (iStartCol > 1 && CanMove(new ChessPieceLocation((ColPos)(iStartCol - 1), (RowPos)(iStartRow - 1))))
                return true;

            if (iStartCol < 8 && CanMove(new ChessPieceLocation((ColPos)(iStartCol + 1), (RowPos)(iStartRow - 1))))
                return true;

            return false;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Return list of all possible moves piece can make on chess board
    /// </summary>
    public override IEnumerable<ChessMove> PossibleMoveList()
    {
        Contract.Assume(Location.RowLoc != (RowPos)7 || ValidateBounds.IsInBounds(Location.ColLoc));

        // Input values.
        int iStartCol = (int)Location.ColLoc;
        int iStartRow = (int)Location.RowLoc;

        // Two moves ahead
        if (iStartRow == 7 &&
            Board[iStartCol, iStartRow - 1] == null &&
            Board[iStartCol, iStartRow - 2] == null)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, Location.ColLoc, (RowPos)(iStartRow - 2));
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }
        }

        // One move ahead
        if (iStartRow > 1 &&
            Board[iStartCol, iStartRow - 1] == null)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, Location.ColLoc, (RowPos)(iStartRow - 1));
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }
        }

        // Capture to left
        if (iStartCol > 1 && iStartRow > 1)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)(iStartCol - 1), (RowPos)(iStartRow - 1));
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }
        }

        // Capture to right
        if (iStartCol < 8 && iStartRow > 1)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)(iStartCol + 1), (RowPos)(iStartRow - 1));
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }
        }
    }
    #endregion

    /// <summary>
    /// Do housekeeping after piece is moved. Return move weight
    /// </summary>
    internal override ChessMoveResults DoMoveHousekeeping(ChessPieceLocation oldPosition, ChessPieceLocation newPosition)
    {
        ChessMoveResults result = new()
        {
            Result = MoveMessage.MoveSucceeded
        };
        InitialPosition = false;

        // Needed to deal with En Passant move
        if (oldPosition.RowLoc == RowPos.R7 && newPosition.RowLoc == RowPos.R5)
        {
            result.EnPassantColumn = newPosition.ColLoc;
        }

        // Deal with pawn promotion
        if (newPosition.RowLoc == RowPos.R1)
        {
            result.PawnPromoted = true;
        }

        return result;
    }
}
