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
/// Encapsulate Knight behavior
/// </summary>
internal class Knight : ChessPiece
{
    internal Knight(ChessBoard board, ChessPieceColor Color, ChessPieceLocation position, bool initialPosition, short id) :
        base(board, Color, ChessPieceType.Knight, position, initialPosition, id) { }

    #region Public API
    public override int Weight { get { return 300; } }

    /// <summary>
    /// Return if a specified move is legal for a Knight
    /// </summary>
    public override bool CanMove(ChessPieceLocation newPosition)
    {
        Contract.Assume(ValidateBounds.IsInBounds(newPosition.RowLoc));
        Contract.Assume(ValidateBounds.IsInBounds(newPosition.ColLoc));

        // End position is your own piece
        if (Board[newPosition.ColLoc, newPosition.RowLoc] != null &&
            Board[newPosition.ColLoc, newPosition.RowLoc].PieceColor == PieceColor)
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

        // Test the eight legal positions a knight can move to.
        if (iEndRow == iStartRow - 2 && iEndCol == iStartCol - 1 ||
            iEndRow == iStartRow - 2 && iEndCol == iStartCol + 1 ||
            iEndRow == iStartRow - 1 && iEndCol == iStartCol - 2 ||
            iEndRow == iStartRow - 1 && iEndCol == iStartCol + 2 ||
            iEndRow == iStartRow + 2 && iEndCol == iStartCol - 1 ||
            iEndRow == iStartRow + 2 && iEndCol == iStartCol + 1 ||
            iEndRow == iStartRow + 1 && iEndCol == iStartCol - 2 ||
            iEndRow == iStartRow + 1 && iEndCol == iStartCol + 2)
        {
            return true;
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
        int iStartCol = (int)Location.ColLoc;
        int iStartRow = (int)Location.RowLoc;

        if (iStartCol > 2 && iStartRow > 1)
        {
            if (CanMove(new ChessPieceLocation((ColPos)(iStartCol - 2), (RowPos)(iStartRow - 1))))
                return true;
        }

        if (iStartCol > 2 && iStartRow < 8)
        {
            if (CanMove(new ChessPieceLocation((ColPos)(iStartCol - 2), (RowPos)(iStartRow + 1))))
                return true;
        }

        if (iStartCol > 1 && iStartRow > 2)
        {
            if (CanMove(new ChessPieceLocation((ColPos)(iStartCol - 1), (RowPos)(iStartRow - 2))))
                return true;
        }

        if (iStartCol < 8 && iStartRow > 2)
        {
            if (CanMove(new ChessPieceLocation((ColPos)(iStartCol + 1), (RowPos)(iStartRow - 2))))
                return true;
        }

        if (iStartCol < 7 && iStartRow < 8)
        {
            if (CanMove(new ChessPieceLocation((ColPos)(iStartCol + 2), (RowPos)(iStartRow + 1))))
                return true;
        }

        if (iStartCol < 7 && iStartRow > 1)
        {
            if (CanMove(new ChessPieceLocation((ColPos)(iStartCol + 2), (RowPos)(iStartRow - 1))))
                return true;
        }

        if (iStartCol > 1 && iStartRow < 7)
        {
            if (CanMove(new ChessPieceLocation((ColPos)(iStartCol - 1), (RowPos)(iStartRow + 2))))
                return true;
        }

        if (iStartCol < 8 && iStartRow < 7)
        {
            if (CanMove(new ChessPieceLocation((ColPos)(iStartCol + 1), (RowPos)(iStartRow + 2))))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Return list of all possible moves piece can make on chess board
    /// </summary>
    public override IEnumerable<ChessMove> PossibleMoveList()
    {
        int iStartCol = (int)Location.ColLoc;
        int iStartRow = (int)Location.RowLoc;

        if (iStartCol > 2 && iStartRow > 1)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)(iStartCol - 2), (RowPos)(iStartRow - 1));
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }
        }

        if (iStartCol > 2 && iStartRow < 8)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)(iStartCol - 2), (RowPos)(iStartRow + 1));
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }
        }

        if (iStartCol > 1 && iStartRow > 2)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)(iStartCol - 1), (RowPos)(iStartRow - 2));
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }
        }

        if (iStartCol > 1 && iStartRow < 7)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)(iStartCol - 1), (RowPos)(iStartRow + 2));
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }
        }

        if (iStartCol < 8 && iStartRow > 2)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)(iStartCol + 1), (RowPos)(iStartRow - 2));
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }
        }

        if (iStartCol < 8 && iStartRow < 7)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)(iStartCol + 1), (RowPos)(iStartRow + 2));
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }
        }

        if (iStartCol < 7 && iStartRow > 1)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)(iStartCol + 2), (RowPos)(iStartRow - 1));
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }
        }

        if (iStartCol < 7 && iStartRow < 8)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)(iStartCol + 2), (RowPos)(iStartRow + 1));
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
        InitialPosition = false;

        ChessMoveResults result = new ChessMoveResults();
        result.Result = MoveMessage.MoveSucceeded;

        return result;
    }
}
