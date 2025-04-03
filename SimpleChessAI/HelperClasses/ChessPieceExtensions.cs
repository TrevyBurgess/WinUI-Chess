// 
// 
namespace TrevyBurgess.Games.TrevyChess.ChessGameAI
{
    using System.Collections.Generic;
    using TrevyBurgess.Games.TrevyChess.ChessBoardLogic;

    public static class ChessPieceExtensions
    {
        /// <summary>
        /// Return piece value if it can get killed, otherwise return 0.
        /// </summary>
        public static int Weakness(this ChessPiece testPiece)
        {
            // Get attacker list
            List<ChessPiece> attackers = [.. testPiece.Attackers()];
            if (attackers.Count == 0)
            {
                return 0;
            }
            attackers.Sort((left, right) => left.Weight - right.Weight);

            // Get defender list
            List<ChessPiece> defenders = [.. testPiece.Defenders()];
            defenders.Sort((left, right) => left.Weight - right.Weight);

            // 1+ attackers, 1+ defenders
            defenders.Insert(0, testPiece);
            return GetWeakness(attackers, defenders);
        }

        /// <summary>
        /// We have 1 or more attackers. 1 or more defenders. There are no kings.
        /// </summary>
        private static int GetWeakness(List<ChessPiece> attackers, List<ChessPiece> defenders)
        {
            int weakness = 0;

            while (attackers.Count > 0)
            {
                if (defenders.Count == 0)
                {
                    break;
                }
                else
                {
                    // 1+ attacker. 1+ defender. Capture defender
                    weakness += defenders[0].Weight;
                    defenders.RemoveAt(0);

                    if (defenders.Count > 0)
                    {
                        weakness -= attackers[0].Weight;
                        attackers.RemoveAt(0);
                    }
                }
            }

            return weakness;
        }
    }
}
