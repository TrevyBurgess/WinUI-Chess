//
//
namespace CyberFeedForward.WinUI_Chess.HelperClasses.InterfacesClasses;

using CyberFeedForward.ChessBoardLogic.ChessPiece;
using CyberFeedForward.WinUI_Chess.HelperClasses.ChessClasses;

public interface IChessBoardControl
{
    /// <summary>
    /// Get the board state
    /// </summary>
    string BoardState { get; set; }

    /// <summary>
    /// Return the chess board to its initial position
    /// </summary>
    void NewGame();

    /// <summary>
    /// Return the chess board to its initial position
    /// </summary>
    //void ResetBoard(string boardLayout);

    /// <summary>
    /// Return if we can undo current move
    /// </summary>
    bool CanUndoMove();

    /// <summary>
    /// Undo move
    /// </summary>
    void UndoMove();

    /// <summary>
    /// Return if we can redo current move
    /// </summary>
    bool CanRedoMove();

    /// <summary>
    /// Redo move
    /// </summary>
    void RedoMove();

    /// <summary>
    /// If true, rotate 180 degrees, else keep upright
    /// </summary>
    bool Rotate180 { get; set; }

    /// <summary>
    /// Get/Set the current player whose turn it is
    /// </summary>
    ChessPieceColor PlayerColor { get; }

    /// <summary>
    /// Get/Set the computer color
    /// </summary>
    ChessPieceColor ComputerColor { get; set; }

    /// <summary>
    /// Get/Set how challenging AI is
    /// </summary>
    int DifficultyLevel { get; set; }

    /// <summary>
    /// Get/set if you want to play against computer
    /// </summary>
    bool PlayAgainstComputer { get; set; }

    /// <summary>
    /// Event for when chess piece successfully moved
    /// </summary>
    event ChessMoveHandler ChessPieceMoved;

    /// <summary>
    /// If true, allow user to move pieces around board, disregarding chess rules
    /// </summary>
    bool PlayWithoutRules { get; set; }

    /// <summary>
    /// Redraw pieces on board
    /// </summary>
    void RefreshBoard();

    /// <summary>
    /// If true, computer is making move
    /// </summary>
    bool IsPlayerTurn { get; }

    /// <summary>
    /// Fires when computer begins move
    /// </summary>
    event ComputerMoveStartedHandler ComputerMoveStarted;

    /// <summary>
    /// Fires when computer ends turn
    /// </summary>
    event ComputerMoveEndedHandler ComputerMoveEnded;
}
