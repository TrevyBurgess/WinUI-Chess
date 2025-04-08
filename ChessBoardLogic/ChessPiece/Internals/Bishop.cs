//
//
namespace CyberFeedForward.ChessBoardLogic.ChessPiece.Internals;

using CyberFeedForward.ChessBoardLogic.ChessBoardClasses;
using CyberFeedForward.ChessBoardLogic.ChessMoveClasses;
using CyberFeedForward.ChessBoardLogic.ChessPiece;
using System.Collections.Generic;

/// <summary>
/// Encapsulate Bishop behavior
/// </summary>
internal class Bishop : ChessPiece
{
    /// <summary>
    /// Constructor
    /// </summary>
    internal Bishop(ChessBoard board, ChessPieceColor Color, ChessPieceLocation position, bool initialPosition, short id) :
        base(board, Color, ChessPieceType.Bishop, position, initialPosition, id) { }

    #region MyRegion
    public override int Weight { get { return 325; } }

    /// <summary>
    /// Return if a specified move is legal for a Bishop
    /// </summary>
    public override bool CanMove(ChessPieceLocation newPosition)
    {
        return CanMoveDiag(newPosition);
    }

    /// <summary>
    /// Return if a specified move is legal for a Rook
    /// </summary>
    internal override bool CanCover(ChessPieceLocation newPosition)
    {
        return CanCoverDiag(newPosition);
    }
    
    /// <summary>
    /// Return if piece can move from current location
    /// </summary>
    public override bool CanMove()
    {
        return CanMoveDiag();
    }

    /// <summary>
    /// Return list of all possible moves piece can make on chess board
    /// </summary>
    public override IEnumerable<ChessMove> PossibleMoveList()
    {
        foreach (ChessMove move in MoveDiagList())
            yield return move;
    }
    #endregion

    /// <summary>
    /// Do housekeeping after piece is moved. Return move weight
    /// </summary>
    internal override ChessMoveResults DoMoveHousekeeping(ChessPieceLocation oldPosition, ChessPieceLocation newPosition)
    {
        InitialPosition = false;

        return new ChessMoveResults
        {
            Result = MoveMessage.MoveSucceeded
        };
    }
}
