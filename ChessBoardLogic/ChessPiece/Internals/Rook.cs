//
//
namespace TrevyBurgess.Games.TrevyChess.ChessBoardLogic.Internal
{
    using System.Collections.Generic;

    /// <summary>
    /// Encapsulate Rook behavior
    /// </summary>
    internal class Rook : ChessPiece
    {
        internal Rook(ChessBoard board, ChessPieceColor Color, ChessPieceLocation position, bool initialPosition, short id) :
            base(board, Color, ChessPieceType.Rook, position, initialPosition, id) { }

        #region Public API
        public override int Weight { get { return 500; } }

        /// <summary>
        /// Return if a specified move is legal for a Rook
        /// </summary>
        public override bool CanMove(ChessPieceLocation newPosition)
        {
            return base.CanMoveHorVert(newPosition);
        }

        /// <summary>
        /// Return if a specified move is legal for a Rook
        /// </summary>
        internal override bool CanCover(ChessPieceLocation newPosition)
        {
            return base.CanCoverHorVert(newPosition);
        }

        /// <summary>
        /// Return if piece can move from current location
        /// </summary>
        public override bool CanMove()
        {
            return base.CanMoveHorVert();
        }

        /// <summary>
        /// Return list of all possible moves piece can make on chess board
        /// </summary>
        public override IEnumerable<ChessMove> PossibleMoveList()
        {
            foreach (ChessMove move in base.MoveHorVertList())
            {
                yield return move;
            }
        }
        #endregion

        /// <summary>
        /// Do housekeeping after piece is moved. Return move weight
        /// </summary>
        internal override ChessMoveResults DoMoveHousekeeping(ChessPieceLocation oldPosition, ChessPieceLocation newPosition)
        {
            base.InitialPosition = false;

            ChessMoveResults result = new ChessMoveResults();
            result.Result = MoveMessage.MoveSucceeded;

            return result;
        }
    }
}
