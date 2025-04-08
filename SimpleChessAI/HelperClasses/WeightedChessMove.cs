// 
// 
using CyberFeedForward.ChessBoardLogic.ChessMoveClasses;

namespace CyberFeedForward.SimpleChessAI.HelperClasses;

public class WeightedChessMove
{
    public int MoveGain;
    public int MoveDanger;

    public int OpponentGain;
    public int OpponentDanger;

    public int PositionGain;
    public bool PawnPromoted;
    public EndGameState EndGameStatus;

    public WeightedChessMove(ChessMove move, WeightedChessMove previousMove)
    {
        CurrentMove = move;
        OriginalChessMove = previousMove;
    }

    /// <summary>
    /// Move we need to execute
    /// </summary>
    public ChessMove CurrentMove;

    /// <summary>
    /// Chess move current move proceeded from
    /// </summary>
    public WeightedChessMove OriginalChessMove;

    public override string ToString()
    {
        string res = string.Empty;
        if (EndGameStatus == EndGameState.GameHasNotEnded)
        {
            if (PawnPromoted)
                res = "PawnP";
        }
        else
        {
            res = EndGameStatus.ToString();
        }

        return CurrentMove.ToString() + ", Gain=" + MoveGain.ToString() + " " + res;
    }
}
