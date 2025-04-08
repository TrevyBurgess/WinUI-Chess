//
//
namespace CyberFeedForward.ChessBoardLogic.ChessPiece;

using CyberFeedForward.ChessBoardLogic.ChessBoardClasses;
using CyberFeedForward.ChessBoardLogic.ChessMoveClasses;
using CyberFeedForward.ChessBoardLogic.ChessPiece.Internals;
using System;
using System.Collections.Generic;

/// <summary>
/// Implements the basic functionality for a chess piece
/// </summary>
public abstract class ChessPiece
{
    #region Public API
    /// <summary>
    /// Get weight of chess piece times 100
    /// Bishop has a weight of 3.25, which gives 325
    /// King's weight is not defined. However for this, we define it as MaxValue
    /// </summary>
    public abstract int Weight { get; }

    /// <summary>
    /// Unique ID to identify instance
    /// </summary>
    public short ID { get; private set; }

    /// <summary>
    /// Chess piece Type: Pawn, Rook, Knight, Bishop, King, Queen
    /// </summary>
    public ChessPieceType PieceType { get; private set; }

    /// <summary>
    /// Chess piece color
    /// </summary>
    public ChessPieceColor PieceColor { get; private set; }

    /// <summary>
    /// Return if piece is a power (Not a pwen)
    /// </summary>
    public bool IsPower { get; private set; }

    /// <summary>
    /// Returns the current position of chess piece
    /// </summary>
    public ChessPieceLocation Location { get; internal set; }

    /// <summary>
    /// Set weather piece is captured.
    /// </summary>
    public bool IsCaptured { get { return Location.ColLoc == ColPos.Captured; } }

    /// <summary>
    /// Return if a specified move is legal
    /// </summary>
    public abstract bool CanMove(ChessPieceLocation newPosition);

    /// <summary>
    /// Return if piece can move from current location
    /// </summary>
    public abstract bool CanMove();

    /// <summary>
    /// Get list of all possible moves this can make
    /// </summary>
    public abstract IEnumerable<ChessMove> PossibleMoveList();

    /// <summary>
    /// Return all pieces that can defend this
    /// Moves that put own king in check aren't included
    /// </summary>
    public IEnumerable<ChessPiece> Defenders()
    {
        foreach (ChessPiece possibleDefender in Board.GetLivePieces(PieceColor))
        {
            // don't include this piece
            if (possibleDefender == this)
                continue;

            if (possibleDefender.CanCover(Location))
                yield return possibleDefender;
        }
    }

    /// <summary>
    /// Return all pieces that can defend this
    /// Moves that put own king in check aren't included
    /// </summary>
    public IEnumerable<ChessPiece> Attackers()
    {
        ChessPieceColor attackerColor = PieceColor == ChessPieceColor.Black ? ChessPieceColor.White : ChessPieceColor.Black;

        foreach (ChessPiece possibleAttacker in Board.GetLivePieces(attackerColor))
        {
            if (possibleAttacker.CanMove(Location))
                yield return possibleAttacker;
        }
    }

    public override string ToString()
    {
        return PieceColor.ToString() +
            " " + PieceType.ToString() +
            ", Position = " + Location.ColLoc.ToString() + ((int)Location.RowLoc).ToString() +
            ", ID = " + ID.ToString();
    }
    #endregion

    /// <summary>
    /// Return if a specified move is legal
    /// </summary>
    internal abstract bool CanCover(ChessPieceLocation newPosition);

    /// <summary>
    /// Marks if the chess piece is in its initial position
    /// </summary>
    internal bool InitialPosition { get; set; }

    /// <summary>
    /// Chess board chess piece resides on
    /// </summary>
    internal ChessBoard Board { get; private set; }

    internal ChessPiece(ChessBoard board, ChessPieceColor pieceColor, ChessPieceType pieceType, ChessPieceLocation position, bool initialPosition, short id)
    {
        Board = board;
        PieceColor = pieceColor;
        PieceType = pieceType;
        Location = position;
        InitialPosition = initialPosition;
        ID = id;
    }

    /// <summary>
    /// Perform specified move
    /// </summary>
    internal ChessMoveResults Move(ChessPieceLocation newPosition, bool moveFreeStyle)
    {
        if (moveFreeStyle || CanMove(newPosition))
        {
            // Initial position
            ChessPieceLocation oldPosition = Location;

            // Do necessary housekeeping
            ChessMoveResults result = DoMoveHousekeeping(oldPosition, newPosition);

            // Check if we're killing a pawn En Passant
            ChessPiece endPiece = Board[newPosition.ColLoc, newPosition.RowLoc];
            if (endPiece == null)
            {
                // Check if we're capturing a pawn En Passant
                if (PieceColor == ChessPieceColor.White && newPosition.RowLoc == RowPos.R6)
                {
                    PawnBlack pawn = Board[newPosition.ColLoc, RowPos.R5] as PawnBlack;
                    if (pawn != null && Board.undoList.GetEnPassantColumn() == newPosition.ColLoc)
                    {
                        Board.CaptureChessPiece(pawn);
                        result.Result = MoveMessage.PawnCapturedEnPassant;
                    }
                }
                else if (PieceColor == ChessPieceColor.Black && newPosition.RowLoc == RowPos.R3)
                {
                    PawnWhite pawn = Board[newPosition.ColLoc, RowPos.R4] as PawnWhite;
                    if (pawn != null && Board.undoList.GetEnPassantColumn() == newPosition.ColLoc)
                    {
                        Board.CaptureChessPiece(pawn);
                        result.Result = MoveMessage.PawnCapturedEnPassant;
                    }
                }
            }
            else
            {
                result.Result = MoveMessage.PieceCaptured;
                result.PieceKilled = newPosition;
            }

            // Move chess piece
            Board.MoveChessPiece(oldPosition.ColLoc, oldPosition.RowLoc, newPosition.ColLoc, newPosition.RowLoc);

            // Switch players                    
            Board.CurrentPlayerColor = Board.CurrentPlayerColor == ChessPieceColor.Black ? ChessPieceColor.White : ChessPieceColor.Black;

            return result;
        }
        else
        {
            throw new IllegalMoveException("Piece can't be moved");
        }
    }

    /// <summary>
    /// Do housekeeping after piece is moved. Return move weight
    /// </summary>
    internal abstract ChessMoveResults DoMoveHousekeeping(ChessPieceLocation oldPosition, ChessPieceLocation newPosition);


    #region Test Moves
    /// <summary>
    /// Check to see if piece can move horizontally or vertically. Does not test for check.
    /// </summary>
    protected bool CanMoveHorVert(ChessPieceLocation newPosition)
    {
        // End position is your own piece
        if (Board[newPosition.ColLoc, newPosition.RowLoc] != null &&
            Board[newPosition.ColLoc, newPosition.RowLoc].PieceColor == PieceColor)
            return false;

        return CanCoverHorVert(newPosition);
    }

    /// <summary>
    /// Check to see if piece can move horizontally or vertically. Does not test for check.
    /// </summary>
    protected bool CanCoverHorVert(ChessPieceLocation newPosition)
    {
        // Piece does not move.
        if (Location.ColLoc == newPosition.ColLoc && Location.RowLoc == newPosition.RowLoc)
            return false;

        // Input values.
        int iStartCol = (int)Location.ColLoc;
        int iEndCol = (int)newPosition.ColLoc;
        int iStartRow = (int)Location.RowLoc;
        int iEndRow = (int)newPosition.RowLoc;

        int currPos;
        int endPos;

        // Moving vertically.
        if (Location.ColLoc == newPosition.ColLoc)
        {
            // Count in the correct direction.
            if (iStartRow < iEndRow)
            {
                currPos = iStartRow;
                endPos = iEndRow;
            }
            else
            {
                currPos = iEndRow;
                endPos = iStartRow;
            }

            // Check if path is clear.
            currPos++;
            while (currPos < endPos)
            {
                if (Board[iStartCol, currPos] == null)
                {
                    currPos++;
                }
                else
                {
                    return false;
                }
            }
        }
        // Moving horizontally.
        else if (Location.RowLoc == newPosition.RowLoc)
        {
            // Count in the correct direction.
            if (iStartCol < iEndCol)
            {
                currPos = iStartCol;
                endPos = iEndCol;
            }
            else
            {
                currPos = iEndCol;
                endPos = iStartCol;
            }

            // Check if path is clear.
            currPos++;
            while (currPos < endPos)
            {
                if (Board[currPos, iStartRow] == null)
                {
                    currPos++;
                }
                else
                {
                    return false;
                }
            }
        }
        // Illegal move
        else
        {
            return false;
        }

        // Move succeed.
        return true;
    }

    /// <summary>
    /// Return if piece can move from current location
    /// </summary>
    protected bool CanMoveHorVert()
    {
        int col = (int)Location.ColLoc;
        int row = (int)Location.RowLoc;

        if (col > 1)
        {
            var newPosition = new ChessPieceLocation((ColPos)(col - 1), (RowPos)row);
            if (CanMoveHorVert(newPosition))
                return true;
        }

        if (col < 8)
        {
            var newPosition = new ChessPieceLocation((ColPos)(col + 1), (RowPos)row);
            if (CanMoveHorVert(newPosition))
                return true;
        }

        if (row > 1)
        {
            var newPosition = new ChessPieceLocation((ColPos)col, (RowPos)(row - 1));
            if (CanMoveHorVert(newPosition))
                return true;
        }

        if (row < 8)
        {
            var newPosition = new ChessPieceLocation((ColPos)col, (RowPos)(row + 1));
            if (CanMoveHorVert(newPosition))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Check to see if piece can move diagonally. Does not test for check.
    /// </summary>
    protected bool CanMoveDiag(ChessPieceLocation newPosition)
    {
        // End position is your own piece
        if (Board[newPosition.ColLoc, newPosition.RowLoc] != null &&
            Board[newPosition.ColLoc, newPosition.RowLoc].PieceColor == PieceColor)
            return false;

        return CanCoverDiag(newPosition);
    }

    /// <summary>
    /// Check to see if piece can move diagonally. Does not test for check.
    /// </summary>
    protected bool CanCoverDiag(ChessPieceLocation newPosition)
    {
        // Piece does not move.
        if (Location.ColLoc == newPosition.ColLoc && Location.RowLoc == newPosition.RowLoc)
            return false;

        // Input values.
        int iStartCol = (int)Location.ColLoc;
        int iEndCol = (int)newPosition.ColLoc;
        int iStartRow = (int)Location.RowLoc;
        int iEndRow = (int)newPosition.RowLoc;

        // Piece has moved diagonally.
        if (Math.Abs(iStartCol - iEndCol) == Math.Abs(iStartRow - iEndRow))
        {
            int diffCol = iEndCol - iStartCol > 0 ? 1 : -1;
            int diffRow = iEndRow - iStartRow > 0 ? 1 : -1;
            int currCol = iStartCol + diffCol;
            int currRow = iStartRow + diffRow;

            // Check if path is clear.
            while (currCol != iEndCol)
            {
                if (Board[currCol, currRow] == null)
                {
                    currCol += diffCol;
                    currRow += diffRow;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Return if piece can move from current location
    /// </summary>
    protected bool CanMoveDiag()
    {
        int col = (int)Location.ColLoc;
        int row = (int)Location.RowLoc;

        if (col > 1 && row > 1)
        {
            if (CanMoveDiag(new ChessPieceLocation((ColPos)(col - 1), (RowPos)(row - 1))))
                return true;
        }

        if (col > 1 && row < 8)
        {
            if (CanMoveDiag(new ChessPieceLocation((ColPos)(col - 1), (RowPos)(row + 1))))
                return true;
        }

        if (col < 8 && row > 1)
        {
            if (CanMoveDiag(new ChessPieceLocation((ColPos)(col + 1), (RowPos)(row - 1))))
                return true;
        }

        if (col < 8 && row < 8)
        {
            if (CanMoveDiag(new ChessPieceLocation((ColPos)(col + 1), (RowPos)(row + 1))))
                return true;
        }

        return false;
    }
    #endregion

    /// <summary>
    /// Return list of all possible moves piece can make
    /// </summary>
    protected IEnumerable<ChessMove> MoveHorVertList()
    {
        // Input values.
        int iStartCol = (int)Location.ColLoc;
        int iEndCol;
        int iStartRow = (int)Location.RowLoc;
        int iEndRow;

        // To the left
        iEndCol = iStartCol - 1;
        iEndRow = (int)Location.RowLoc;
        while (iEndCol > 0)
        {
            ChessMove move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)iEndCol, Location.RowLoc);

            // Check if move puts king in check
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }

            if (Board[iEndCol, iEndRow] != null)
            {
                break;
            }

            iEndCol--;
        }

        // To the right
        iEndCol = iStartCol + 1;
        iEndRow = (int)Location.RowLoc;
        while (iEndCol < 9)
        {
            ChessMove move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)iEndCol, Location.RowLoc);

            // Check if move puts king in check
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }

            if (Board[iEndCol, iEndRow] != null)
            {
                break;
            }

            iEndCol++;
        }

        // Upwards
        iEndCol = (int)Location.ColLoc;
        iEndRow = iStartRow + 1;
        while (iEndRow < 9)
        {
            ChessMove move = new ChessMove(Location.ColLoc, Location.RowLoc, Location.ColLoc, (RowPos)iEndRow);

            // Check if move puts king in check
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }

            if (Board[iEndCol, iEndRow] != null)
            {
                break;
            }

            iEndRow++;
        }

        // Down
        iEndCol = (int)Location.ColLoc;
        iEndRow = iStartRow - 1;
        while (iEndRow > 0)
        {
            ChessMove move = new ChessMove(Location.ColLoc, Location.RowLoc, Location.ColLoc, (RowPos)iEndRow);

            // Check if move puts king in check
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }

            if (Board[iEndCol, iEndRow] != null)
            {
                break;
            }

            iEndRow--;
        }
    }

    /// <summary>
    /// Return list of all possible moves piece can make
    /// </summary>
    protected IEnumerable<ChessMove> MoveDiagList()
    {
        // Input values.
        int iStartCol = (int)Location.ColLoc;
        int iEndCol;
        int iStartRow = (int)Location.RowLoc;
        int iEndRow;

        // Down and to left
        iEndCol = iStartCol - 1;
        iEndRow = iStartRow - 1;
        while (iEndCol > 0 && iEndRow > 0)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)iEndCol, (RowPos)iEndRow);

            // Check if move puts king in check
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }

            if (Board[iEndCol, iEndRow] != null)
            {
                break;
            }

            iEndCol--;
            iEndRow--;
        }

        // Down and to right
        iEndCol = iStartCol + 1;
        iEndRow = iStartRow - 1;
        while (iEndCol < 9 && iEndRow > 0)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)iEndCol, (RowPos)iEndRow);

            // Check if move puts king in check
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }

            if (Board[iEndCol, iEndRow] != null)
            {
                break;
            }

            iEndCol++;
            iEndRow--;
        }

        // Up and to left
        iEndCol = iStartCol - 1;
        iEndRow = iStartRow + 1;
        while (iEndCol > 0 && iEndRow < 9)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)iEndCol, (RowPos)iEndRow);

            // Check if move puts king in check
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }

            if (Board[iEndCol, iEndRow] != null)
            {
                break;
            }

            iEndCol--;
            iEndRow++;
        }

        // Down and to right
        iEndCol = iStartCol + 1;
        iEndRow = iStartRow + 1;
        while (iEndCol < 9 && iEndRow < 9)
        {
            var move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)iEndCol, (RowPos)iEndRow);

            // Check if move puts king in check
            if (Board.CanMove(move.StartLocation, move.EndLocation))
            {
                yield return move;
            }

            if (Board[iEndCol, iEndRow] != null)
            {
                break;
            }

            iEndCol++;
            iEndRow++;
        }
    }
}
