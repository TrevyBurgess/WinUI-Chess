//
//
namespace CyberFeedForward.ChessBoardLogic.ChessMoveClasses;

using CyberFeedForward.ChessBoardLogic.ChessBoardClasses;

public class ChessMoveResults
{
    /// <summary>
    /// Old Rook position
    /// </summary>
    public ChessPieceLocation StartLocation;

    /// <summary>
    /// New Rook position
    /// </summary>
    public ChessPieceLocation EndLocation;

    /// <summary>
    /// Result of specific move
    /// </summary>
    public MoveMessage Result;

    /// <summary>
    /// Location of piece that was killed
    /// </summary>
    public ChessPieceLocation PieceKilled;

    /// <summary>
    /// Location of piece that was killed
    /// </summary>
    public ColPos EnPassantColumn;

    /// <summary>
    /// True if pawn is being promoted
    /// </summary>
    public bool PawnPromoted;
}
