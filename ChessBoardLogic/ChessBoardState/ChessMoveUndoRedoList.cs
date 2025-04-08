//
//
namespace CyberFeedForward.ChessBoardLogic.ChessBoardState;

using CyberFeedForward.ChessBoardLogic.ChessBoardClasses;
using CyberFeedForward.ChessBoardLogic.ChessMoveClasses;
using CyberFeedForward.ChessBoardLogic.HelperClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

/// <summary>
/// Manage chess board UndoRedo list.
/// </summary>
internal class ChessMoveUndoRedoList
{
    private readonly LinkedList<string> undoList;

    /// <summary>
    /// Current chess move
    /// </summary>
    private LinkedListNode<string> currentMove;

    /// <summary>
    /// Keep track of how many moves passed since game started
    /// </summary>
    internal int MoveCount { get; set; }

    /// <summary>
    /// Create undo list, starting with first move
    /// </summary>
    internal ChessMoveUndoRedoList(string move)
    {
        undoList = new LinkedList<string>();
        currentMove = new LinkedListNode<string>(move);
        undoList.AddLast(currentMove);
        MoveCount = 1;
    }

    internal ColPos GetEnPassantColumn()
    {
        Contract.Assume(currentMove.Value.Length > 1);

        string boardState = currentMove.Value;

        return Library.ParseColPos(boardState[boardState.Length - 2]);
    }

    /// <summary>
    /// Add move to list of moves
    /// </summary>
    internal void AddMoveToUndoList(string boardLayout, bool computerMove)
    {
        Contract.Requires<ArgumentException>(boardLayout.Length == ChessBoard.C_BoardStateStringLength);

        if (undoList.Count > MoveCount)
        {
            // Discard moves, since they are being replaced
            while (undoList.Count > MoveCount)
            {
                undoList.RemoveLast();
            }
        }

        // Add new move
        if (computerMove)
        {
            boardLayout = boardLayout.Substring(0, boardLayout.Length - 1) + "C";
        }

        currentMove = new LinkedListNode<string>(boardLayout);

        undoList.AddLast(currentMove);
        MoveCount++;
    }

    /// <summary>
    /// Return true if previous state exists
    /// </summary>
    internal bool CanGetPreviousState()
    {
        if (currentMove.Previous == null)
            return false;
        else return true;
    }

    /// <summary>
    /// Return true if next state exists
    /// </summary>
    internal bool CanGetNextState()
    {
        if (currentMove.Next == null)
            return false;
        else return true;
    }

    /// <summary>
    /// Gets the previous board state
    /// </summary>
    /// <returns>Previous state of board</returns>
    internal string GetPreviousMove()
    {
        if (currentMove.Previous == null)
        {
            throw new IllegalMoveException("Can't Undo Move");
        }
        else
        {
            MoveCount--;
            string moveString = currentMove.Value;
            currentMove = currentMove.Previous;

            if (moveString.Substring(moveString.Length - 1) == "C")
            {
                currentMove = currentMove.Previous;
                undoList.RemoveLast();
                MoveCount --;
            }

            return currentMove.Value;
        }
    }

    /// <summary>
    /// Gets the next board state
    /// </summary>
    /// <returns>Next state of board</returns>
    internal string GetNextMove()
    {
        if (currentMove.Next == null)
        {
            throw new IllegalMoveException("Can't Undo Move");
        }
        else
        {
            currentMove = currentMove.Next;
            MoveCount++;

            return currentMove.Value;
        }
    }
}
