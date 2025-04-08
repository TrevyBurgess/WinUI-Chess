//
//
namespace CyberFeedForward.ChessBoardLogic.ChessBoardClasses;

using System.Diagnostics;

/// <summary>
/// Store position of chess pieces
/// </summary>
[method: DebuggerHidden]
/// <summary>
/// Store position of chess pieces
/// </summary>
public struct ChessPieceLocation(ColPos colLoc, RowPos rowLoc) : System.IEquatable<ChessPieceLocation>
{
    public ColPos ColLoc = colLoc;
    public RowPos RowLoc = rowLoc;

    [DebuggerHidden]
    public readonly bool Equals(ChessPieceLocation other)
    {
        return other.ColLoc == ColLoc && other.RowLoc == RowLoc;
    }

    public override readonly string ToString()
    {
        return ColLoc.ToString() + ((int)RowLoc).ToString();
    }
}
