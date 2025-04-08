//
//
namespace CyberFeedForward.ChessBoardLogic.ChessMoveClasses;

using CyberFeedForward.ChessBoardLogic.ChessBoardClasses;

public class ChessMove
{
    public ChessMove() { }

    public ChessMove(ColPos startCol, RowPos startRow, ColPos endCol, RowPos endRow)
    {
        StartLocation = new ChessPieceLocation(startCol, startRow);
        EndLocation = new ChessPieceLocation(endCol, endRow);
    }

    public ChessMove(ChessPieceLocation startLocation, ChessPieceLocation endLocation)
    {
        StartLocation = startLocation;
        EndLocation = endLocation;
    }

    /// <summary>
    /// Old Rook position
    /// </summary>
    public ChessPieceLocation StartLocation;

    /// <summary>
    /// New Rook position
    /// </summary>
    public ChessPieceLocation EndLocation;

    public override string ToString()
    {
        return "Start: " + StartLocation.ToString() + " End: " + EndLocation;
    }
}
