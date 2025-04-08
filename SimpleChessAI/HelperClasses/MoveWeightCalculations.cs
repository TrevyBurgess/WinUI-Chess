// 
//
namespace CyberFeedForward.SimpleChessAI.HelperClasses;

using CyberFeedForward.ChessBoardLogic.ChessBoardClasses;
using CyberFeedForward.ChessBoardLogic.ChessMoveClasses;
using CyberFeedForward.ChessBoardLogic.ChessPiece;
using System.Collections.Generic;

/// <summary>
/// Calculates list of
/// </summary>
public static class MoveWeightCalculations
{
    /// <summary>
    /// Return list of moves, weighted by how much user gains by making move
    /// 
    /// For each move
    ///     MoveGain = pieceWeaknessBeforeMove - pieceWeaknessAfterMove + opponentPieceWeaknessAfterMove
    ///     PositionGain = positionWeightAfterMove - positionWeightBeforeMove
    /// </summary>
    public static List<WeightedChessMove> WeightedMoveList(ChessBoard board, WeightedChessMove previousMove)
    {
        var currentPlayer = board.CurrentPlayerColor;
        var opponentPlayer = board.CurrentPlayerColor == ChessPieceColor.Black ? ChessPieceColor.White : ChessPieceColor.Black;

        var playerWeaknessBeforeMove = PlayerWeakness(board, currentPlayer);
        var opponentWeaknessBeforeMove = PlayerWeakness(board, opponentPlayer);

        List<WeightedChessMove> weightedMoves = new List<WeightedChessMove>();
        foreach (ChessMove chessMove in board.PossibleChessMoves(currentPlayer))
        {
            var weightedChessMove = new WeightedChessMove(chessMove, previousMove);
            weightedMoves.Add(weightedChessMove);

            // Piece weakness before move
            ChessPiece pieceBeforeMoving = board[chessMove.StartLocation];
            int weaknessBeforeMove = pieceBeforeMoving.Weakness();
            if (weaknessBeforeMove > 0)
                weightedChessMove.MoveGain = weaknessBeforeMove;

            // Opponent piece weakness before moving
            ChessPiece opponentPieceBeforeMoving = board[chessMove.EndLocation];
            if (opponentPieceBeforeMoving != null)
                weightedChessMove.MoveGain += opponentPieceBeforeMoving.Weight;

            // Make move
            board.Move(chessMove, true);

            // Check end game
            weightedChessMove.EndGameStatus = board.CheckEndGame();
            if (weightedChessMove.EndGameStatus == EndGameState.CheckMate)
            {
                // We have a winner.
                weightedMoves.Clear();
                weightedMoves.Add(weightedChessMove);
                break;
            }

            // Piece weakness after move. 
            ChessPiece pieceAfterMoving = board[chessMove.EndLocation];
            int weaknessAfterMove = pieceAfterMoving.Weakness();
            if (weaknessAfterMove > 0)
                weightedChessMove.MoveGain -= weaknessAfterMove;

            // Position gain
            weightedChessMove.PositionGain = MoveWeights.GetLocationWeight(chessMove.EndLocation) - MoveWeights.GetLocationWeight(chessMove.StartLocation);

            // Opponent danger
            weightedChessMove.OpponentDanger = PlayerWeakness(board, opponentPlayer) - opponentWeaknessBeforeMove;

            // Move danger
            weightedChessMove.MoveDanger = PlayerWeakness(board, currentPlayer) - playerWeaknessBeforeMove;

            // Restore board
            board.UndoMove();
        }

        return weightedMoves;
    }

    /// <remarks>
    /// Describes how weak a player's position is.
    /// Returns the sum of all the pieces that are in danger.
    /// Returns 0 if no piece is in danger.
    /// </remarks>
    private static int PlayerWeakness(ChessBoard board, ChessPieceColor playerColor)
    {
        int danger = 0;

        foreach (ChessPiece piece in board.GetLivePieces(playerColor))
        {
            danger += piece.Weakness();
        }
        return danger;
    }
}
