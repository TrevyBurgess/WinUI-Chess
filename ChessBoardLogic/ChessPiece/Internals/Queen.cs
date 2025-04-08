//
//
namespace CyberFeedForward.ChessBoardLogic.ChessPiece.Internals;

using CyberFeedForward.ChessBoardLogic.ChessBoardClasses;
using CyberFeedForward.ChessBoardLogic.ChessMoveClasses;
using CyberFeedForward.ChessBoardLogic.ChessPiece;
using System.Collections.Generic;

/// <summary>
/// Encapsulate Queen behavior
/// </summary>
internal class Queen : ChessPiece
{
    internal Queen(ChessBoard board, ChessPieceColor Color, ChessPieceLocation position, bool initialPosition, short id) :
        base(board, Color, ChessPieceType.Queen, position, initialPosition, id) { }

    #region Public API
    public override int Weight { get { return 900; } }

    /// <summary>
    /// Return if a specified move is legal for a Queen
    /// </summary>
    public override bool CanMove(ChessPieceLocation newPosition)
    {
        return CanMoveHorVert(newPosition) || CanMoveDiag(newPosition);
    }

    /// <summary>
    /// Return if a specified move is legal for a Rook
    /// </summary>
    internal override bool CanCover(ChessPieceLocation newPosition)
    {
        return CanCoverHorVert(newPosition) || CanCoverDiag(newPosition);
    }

    /// <summary>
    /// Return if piece can move from current location
    /// </summary>
    public override bool CanMove()
    {
        return CanMoveDiag() || CanMoveHorVert();
    }

    /// <summary>
    /// Return list of all possible moves piece can make on chess board
    /// </summary>
    public override IEnumerable<ChessMove> PossibleMoveList()
    {
        foreach (ChessMove move in MoveDiagList())
        {
            yield return move;
        }

        foreach (ChessMove move in MoveHorVertList())
        {
            yield return move;
        }
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
