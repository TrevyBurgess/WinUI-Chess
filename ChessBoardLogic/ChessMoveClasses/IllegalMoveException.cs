//
//
namespace CyberFeedForward.ChessBoardLogic.ChessMoveClasses;

using System;

/// <summary>
/// Illegal chess move
/// </summary>
public class IllegalMoveException : InvalidOperationException
{
    public IllegalMoveException() : base() { }

    public IllegalMoveException(string message) : base(message) { }
}
