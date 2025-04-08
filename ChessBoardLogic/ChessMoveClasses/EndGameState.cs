//
//
namespace CyberFeedForward.ChessBoardLogic.ChessMoveClasses;

public enum EndGameState
{
    /// <summary>
    /// If game is continuing
    /// </summary>
    GameHasNotEnded,

    /// <summary>
    /// Either black or white king is in check
    /// </summary>
    IsCheck,

    /// <summary>
    /// Only kings remain on field
    /// </summary>
    IsADraw,

    /// <summary>
    /// A king is checkmated
    /// </summary>
    CheckMate
}
