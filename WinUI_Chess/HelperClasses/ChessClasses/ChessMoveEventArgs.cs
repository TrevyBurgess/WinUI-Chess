//
//
namespace CyberFeedForward.WinUI_Chess.HelperClasses.ChessClasses;

using CyberFeedForward.ChessBoardLogic.ChessPiece;
using System;

/// <summary>
/// Event arguments to be passed into the drag event.
/// </summary>
public class ChessMoveEventArgs(ChessPieceColor playersTurn, string statusMessage, string aiComments, string chessCode) : EventArgs
{
    /// <summary>
    /// Current player's turn
    /// </summary>
    public ChessPieceColor PlayersTurn { get; private set; } = playersTurn;

    /// <summary>
    /// Silly messages
    /// </summary>
    public string StatusMessage { get; private set; } = statusMessage;

    /// <summary>
    /// Funny messages AI gives when playing
    /// </summary>
    public string AiComments { get; private set; } = aiComments;

    /// <summary>
    /// Serialized chess board
    /// </summary>
    public string ChessCode { get; private set; } = chessCode;
}
