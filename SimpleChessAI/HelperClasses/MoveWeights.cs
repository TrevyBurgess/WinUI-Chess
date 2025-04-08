//
//
namespace CyberFeedForward.SimpleChessAI.HelperClasses;

using CyberFeedForward.ChessBoardLogic.ChessBoardClasses;
using System;
using System.Collections.Generic;

internal static class MoveWeights
{
    /// <summary>
    /// Each location on a chess board has value when occupied.
    /// Return the value associated with the specified location
    /// </summary>
    internal static int GetLocationWeight(ChessPieceLocation location)
    {
        return GetLocationWeight(location.ColLoc, location.RowLoc);
    }

    /// <summary>
    /// Each location on a chess board has value when occupied.
    /// Return the value associated with the specified location
    /// </summary>
    internal static int GetLocationWeight(ColPos Col, RowPos Row)
    {
        int weight = 0;
        switch (Col)
        {
            case ColPos.A:
                weight = 5;
                break;

            case ColPos.B:
                weight = 6;
                break;

            case ColPos.C:
                weight = 7;
                break;

            case ColPos.D:
                weight = 8;
                break;

            case ColPos.E:
                weight = 8;
                break;

            case ColPos.F:
                weight = 7;
                break;

            case ColPos.G:
                weight = 6;
                break;

            case ColPos.H:
                weight = 5;
                break;
        }

        switch (Row)
        {
            case RowPos.R1:
                weight = 1;
                break;

            case RowPos.R2:
                weight = 2;
                break;

            case RowPos.R3:
                weight = 3;
                break;

            case RowPos.R4:
                weight = 4;
                break;

            case RowPos.R5:
                weight = 4;
                break;

            case RowPos.R6:
                weight = 3;
                break;

            case RowPos.R7:
                weight = 2;
                break;

            case RowPos.R8:
                weight = 1;
                break;
        }

        return weight;
    }

    internal static List<WeightedChessMove> SortMoveGain(List<WeightedChessMove> moves, int minResults, Comparison<WeightedChessMove> comparer)
    {
        moves.Sort(comparer);

        int maxGain = moves[0].MoveGain;
        List<WeightedChessMove> reducedMoveList = new List<WeightedChessMove>();
        foreach (WeightedChessMove move in moves)
        {
            if (move.MoveGain == maxGain)
                reducedMoveList.Add(move);
        }

        if (reducedMoveList.Count < minResults)
        {
            return moves.GetRange(0, minResults);
        }
        else
        {
            return reducedMoveList;
        }
    }

    internal static List<WeightedChessMove> SortMoveDanger(List<WeightedChessMove> moves, int minResults, Comparison<WeightedChessMove> comparer)
    {
        moves.Sort(comparer);

        int maxDanger = moves[0].MoveDanger;
        List<WeightedChessMove> reducedMoveList = new List<WeightedChessMove>();
        foreach (WeightedChessMove move in moves)
        {
            if (move.MoveDanger == maxDanger)
                reducedMoveList.Add(move);
        }

        if (reducedMoveList.Count < minResults)
        {
            return moves.GetRange(0, minResults);
        }
        else
        {
            return reducedMoveList;
        }
    }

    internal static List<WeightedChessMove> SortOpponentGain(List<WeightedChessMove> moves, int minResults, Comparison<WeightedChessMove> comparer)
    {
        moves.Sort(comparer);

        int maxOpponentGain = moves[0].OpponentGain;
        List<WeightedChessMove> reducedMoveList = new List<WeightedChessMove>();
        foreach (WeightedChessMove move in moves)
        {
            if (move.OpponentGain == maxOpponentGain)
                reducedMoveList.Add(move);
        }

        if (reducedMoveList.Count < minResults)
        {
            return moves.GetRange(0, minResults);
        }
        else
        {
            return reducedMoveList;
        }
    }

    internal static List<WeightedChessMove> SortOpponentDanger(List<WeightedChessMove> moves, int minResults, Comparison<WeightedChessMove> comparer)
    {
        moves.Sort(comparer);

        int maxOpponentDanger = moves[0].MoveDanger;
        List<WeightedChessMove> reducedMoveList = new List<WeightedChessMove>();
        foreach (WeightedChessMove move in moves)
        {
            if (move.MoveDanger == maxOpponentDanger)
                reducedMoveList.Add(move);
        }

        if (reducedMoveList.Count < minResults)
        {
            return moves.GetRange(0, minResults);
        }
        else
        {
            return reducedMoveList;
        }
    }

    internal static List<WeightedChessMove> SortPositionGain(List<WeightedChessMove> moves, int minResults, Comparison<WeightedChessMove> comparer)
    {
        moves.Sort(comparer);

        if (moves.Count <= minResults)
            return moves;

        int positionGainStrength = moves[0].PositionGain;
        List<WeightedChessMove> reducedMoveList = new List<WeightedChessMove>();
        foreach (WeightedChessMove move in moves)
        {
            if (move.PositionGain == positionGainStrength)
                reducedMoveList.Add(move);
        }

        if (reducedMoveList.Count < minResults)
        {
            return moves.GetRange(0, minResults);
        }
        else
        {
            return reducedMoveList;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="moves">List we want to sort and reduce</param>
    /// <param name="minResults">Minimum results required in list. If 0, return original sorted list</param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    internal static List<WeightedChessMove> GetBestResultList(List<WeightedChessMove> moves, int minResults, Comparison<WeightedChessMove> comparer)
    {
        moves.Sort(comparer);

        if (minResults <= 0)
        {
            return moves;
        }
        else if (moves.Count <= minResults)
        {
            return moves;
        }
        else
        {
            int positionGainStrength = moves[0].PositionGain;
            List<WeightedChessMove> reducedMoveList = new List<WeightedChessMove>();
            foreach (WeightedChessMove move in moves)
            {
                if (move.PositionGain == positionGainStrength)
                    reducedMoveList.Add(move);
            }

            if (reducedMoveList.Count < minResults)
            {
                return moves.GetRange(0, minResults);
            }
            else
            {
                return reducedMoveList;
            }
        }
    }
}
