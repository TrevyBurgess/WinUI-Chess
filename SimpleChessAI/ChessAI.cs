// 
// 
namespace CyberFeedForward.SimpleChessAI;

using CyberFeedForward.ChessBoardLogic.ChessBoardClasses;
using CyberFeedForward.ChessBoardLogic.ChessMoveClasses;
using CyberFeedForward.ChessBoardLogic.ChessPiece;
using CyberFeedForward.SimpleChessAI.HelperClasses;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

public class ChessAI
{
    /// <summary>
    /// Suggest best move
    /// </summary>
    public static async Task<ChessMove> SuggestMoveAsync(ChessBoard board, int difficulty)
    {
        Contract.Requires<ArgumentNullException>(board != null);
        Contract.Requires<ArgumentException>(difficulty > 0, "Difficulty level must be 1 or greater.");

        var testBoard = new ChessBoard(board);

        var bestChessMove = await SuggestMove(testBoard, difficulty, 2, null);

        return bestChessMove.CurrentMove;
    }

    private static async Task<WeightedChessMove> SuggestMove(ChessBoard board, int difficulty, int minResults, WeightedChessMove previousMove)
    {
        var results = await CreateMoveList(board, difficulty, minResults, previousMove);
        if (results.Count == 0)
        {
            throw new NotSupportedException("No results found.");
        }
        if (results.Count == 1)
        {
            return results[0];
        }
        else
        {
            // Highest move gain
            results = MoveWeights.SortMoveGain(results, minResults,
                (left, right) => right.MoveGain - left.MoveGain);

            // Lowest move danger
            results = MoveWeights.SortMoveDanger(results, minResults,
                (left, right) => left.MoveDanger - right.MoveDanger);

            // Lowest opponent gain
            results = MoveWeights.SortOpponentGain(results, minResults,
                (left, right) => left.OpponentGain - right.OpponentGain);

            // Highest opponent danger
            results = MoveWeights.SortOpponentDanger(results, minResults,
                (left, right) => right.OpponentDanger - left.OpponentDanger);

            // Best position
            results = MoveWeights.SortPositionGain(results, minResults,
                (left, right) => right.PositionGain - left.PositionGain);

            // all moves have equal merit. Select one at random.
            var r = new Random(DateTime.Now.Millisecond);
            return results[r.Next(results.Count - 1)];
        }
    }

    /// <summary>
    /// Return list of best possible moves
    /// </summary>
    internal static async Task<List<WeightedChessMove>> CreateMoveList(ChessBoard board, int difficulty, int minResults, WeightedChessMove previousMove)
    {
        difficulty--;

        // Make list of all possible moves.
        var moves = MoveWeightCalculations.WeightedMoveList(board, previousMove);

        if (moves.Count == 0)
        {
            // There are no winning moves remaining
            return null;
        }
        else if (moves.Count == 1 && moves[0].EndGameStatus == EndGameState.CheckMate)
        {
            // Return winning move
            return moves;
        }
        else
        {
            // Create list of all moves that puts opponent king in check
            var checkMoves = new List<WeightedChessMove>();
            foreach (WeightedChessMove move in moves)
            {
                // We will accept a draw
                if (move.EndGameStatus == EndGameState.IsADraw || move.EndGameStatus == EndGameState.CheckMate)
                {
                    moves.Clear();
                    moves.Add(move);

                    return moves;
                }
            }
            if (checkMoves.Count > 0)
            {
                moves = checkMoves;
            }

            if (difficulty == 0)
            {
                return moves;
            }
            else
            {
                var opponentBestMoves = await GetOpponentBestMoves(board, difficulty, minResults, moves);
                if (opponentBestMoves.Count == 0 || previousMove == null)
                {
                    // Any move will win
                    return moves;
                }
                else
                {
                    var bestMoves = new List<WeightedChessMove>();
                    foreach (WeightedChessMove opponentMove in opponentBestMoves)
                    {
                        if (opponentMove.EndGameStatus == EndGameState.GameHasNotEnded)
                        {
                            var testMove = opponentMove.OriginalChessMove;
                            //testMove.OpponentGain = opponentMove.MoveGain;
                            testMove.MoveGain -= opponentMove.MoveGain;

                            bestMoves.Add(testMove);
                        }
                    }

                    return bestMoves;
                }
            }
        }
    }

    private static async Task<List<WeightedChessMove>> GetOpponentBestMoves(ChessBoard board, int difficulty, int minResults, List<WeightedChessMove> moves)
    {
        #region Used for debugging
        //List<WeightedChessMove> opponentMoves = new List<WeightedChessMove>();
        //foreach (WeightedChessMove move in moves)
        //{
        //    // Create copy of board
        //    ChessBoard testBoard = new ChessBoard(board);

        //    ChessMoveResults moveRsults = testBoard.Move(move.CurrentMove, true);
        //    if (moveRsults.PawnPromoted)
        //    {
        //        // Check for promoting to queen
        //        testBoard.PromotePawn(ChessPieceType.Queen, move.CurrentMove.EndLocation.ColLoc, move.CurrentMove.EndLocation.RowLoc, false);
        //        move.PawnPromoted = true;
        //        opponentMoves.Add(await SuggestMove(testBoard, difficulty, minResults, move));

        //        testBoard.UndoMove();

        //        // Check for promoting to knight
        //        testBoard.PromotePawn(ChessPieceType.Knight, move.CurrentMove.EndLocation.ColLoc, move.CurrentMove.EndLocation.RowLoc, false);
        //        move.PawnPromoted = true;
        //        opponentMoves.Add(await SuggestMove(testBoard, difficulty, minResults, move));
        //    }
        //    else
        //    {
        //        opponentMoves.Add(await SuggestMove(testBoard, difficulty, minResults, move));
        //    }
        //} 
        #endregion

        // For each move we make, find best move opponent makes
        var opponentMoveTasks = new List<Task<WeightedChessMove>>();
        foreach (WeightedChessMove move in moves)
        {
            // Create copy of board
            var testBoard = new ChessBoard(board);

            var moveRsults = testBoard.Move(move.CurrentMove, true);
            if (moveRsults.PawnPromoted)
            {
                // Check for promoting to queen
                testBoard.PromotePawn(ChessPieceType.Queen, move.CurrentMove.EndLocation.ColLoc, move.CurrentMove.EndLocation.RowLoc);
                move.PawnPromoted = true;

                opponentMoveTasks.Add(
                Task<WeightedChessMove>.Factory.StartNew(() => { return SuggestMove(testBoard, difficulty, minResults, move).Result; })
                );

                testBoard.UndoMove();

                // Check for promoting to knight
                testBoard.PromotePawn(ChessPieceType.Knight, move.CurrentMove.EndLocation.ColLoc, move.CurrentMove.EndLocation.RowLoc);
                move.PawnPromoted = true;

                opponentMoveTasks.Add(
                Task<WeightedChessMove>.Factory.StartNew(() => { return SuggestMove(testBoard, difficulty, minResults, move).Result; })
                );
            }
            else
            {
                opponentMoveTasks.Add(
                Task<WeightedChessMove>.Factory.StartNew(() => { return SuggestMove(testBoard, difficulty, minResults, move).Result; })
                );
            }
        }

        // Get opponent moves
        var opponentMoves = new List<WeightedChessMove>();
        foreach (Task<WeightedChessMove> opponentMoveTask in opponentMoveTasks)
        {
            var opponentMove = await opponentMoveTask;

            opponentMoves.Add(opponentMove.OriginalChessMove);
        }

        return opponentMoves;
    }
}
