//
//
namespace TrevyBurgess.Games.TrevyChess.ChessBoardLogic.Internal
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Encapsulate King behavior
    /// </summary>
    internal class King : ChessPiece
    {
        internal King(ChessBoard board, ChessPieceColor Color, ChessPieceLocation position, bool initialPosition, short id) :
            base(board, Color, ChessPieceType.King, position, initialPosition, id) { }

        #region Public API
        public override int Weight { get { return int.MaxValue; } }

        /// <summary>
        /// Check to see if king can move one space in any direction. Does not test for check.
        /// </summary>
        public override bool CanMove(ChessPieceLocation newPosition)
        {
            Contract.Assume(ValidateBounds.IsInBounds(newPosition.RowLoc));
            Contract.Assume(ValidateBounds.IsInBounds(newPosition.ColLoc));

            // End position is your own piece
            if (Board[newPosition.ColLoc, newPosition.RowLoc] != null &&
                Board[newPosition.ColLoc, newPosition.RowLoc].PieceColor == base.PieceColor)
                return false;

            return CanCover(newPosition);
        }
        
        /// <summary>
        /// Return if a specified move is legal for a Rook
        /// </summary>
        internal override bool CanCover(ChessPieceLocation newPosition)
        {
            int colMove = System.Math.Abs((int)Location.ColLoc - (int)newPosition.ColLoc);
            int rowMove = System.Math.Abs((int)Location.RowLoc - (int)newPosition.RowLoc);

            if ((colMove == 1 && rowMove == 1) ||
                (colMove == 0 && rowMove == 1) ||
                (colMove == 1 && rowMove == 0) ||
                CanCastle(newPosition))
                return true;
            else
                return false;            
        }

        /// <summary>
        /// Return if piece can move from current location
        /// </summary>
        public override bool CanMove()
        {
            return base.CanMoveDiag() || base.CanMoveHorVert();
        }

        /// <summary>
        /// Return list of all possible moves piece can make on chess board
        /// </summary>
        public override IEnumerable<ChessMove> PossibleMoveList()
        {
            // Input values.
            int iStartCol = (int)Location.ColLoc;
            int iEndCol;
            int iStartRow = (int)Location.RowLoc;
            int iEndRow;

            // Up and to left
            iEndCol = iStartCol - 1;
            iEndRow = iStartRow + 1;
            if (iEndCol > 0 && iEndRow < 9)
            {
                ChessMove move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)iEndCol, (RowPos)iEndRow);

                if (Board.CanMove(move.StartLocation, move.EndLocation))
                {
                    yield return move;
                }
            }

            // Upwards
            iEndCol = iStartCol;
            iEndRow = iStartRow + 1;
            if (iEndRow < 9)
            {
                ChessMove move = new ChessMove(Location.ColLoc, Location.RowLoc, Location.ColLoc, (RowPos)iEndRow);

                if (Board.CanMove(move.StartLocation, move.EndLocation))
                {
                    yield return move;
                }
            }

            // Up and to right
            iEndCol = iStartCol + 1;
            iEndRow = iStartRow + 1;
            if (iEndCol < 9 && iEndRow < 9)
            {
                ChessMove move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)iEndCol, (RowPos)iEndRow);

                if (Board.CanMove(move.StartLocation, move.EndLocation))
                {
                    yield return move;
                }
            }

            // To the left
            iEndCol = iStartCol - 1;
            iEndRow = iStartRow;
            if (iEndCol > 0)
            {
                ChessMove move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)iEndCol, Location.RowLoc);

                if (Board.CanMove(move.StartLocation, move.EndLocation))
                {
                    yield return move;
                }
            }

            // To the right
            iEndCol = iStartCol + 1;
            iEndRow = iStartRow;
            if (iEndCol < 9)
            {
                ChessMove move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)iEndCol, Location.RowLoc);

                if (Board.CanMove(move.StartLocation, move.EndLocation))
                {
                    yield return move;
                }
            }

            // Down and to left
            iEndCol = iStartCol - 1;
            iEndRow = iStartRow - 1;
            if (iEndCol > 0 && iEndRow > 0)
            {
                ChessMove move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)iEndCol, (RowPos)iEndRow);

                if (Board.CanMove(move.StartLocation, move.EndLocation))
                {
                    yield return move;
                }
            }

            // Down
            iEndCol = iStartCol;
            iEndRow = iStartRow - 1;
            if (iEndRow < 9)
            {
                ChessMove move = new ChessMove(Location.ColLoc, Location.RowLoc, Location.ColLoc, (RowPos)iEndRow);

                if (Board.CanMove(move.StartLocation, move.EndLocation))
                {
                    yield return move;
                }
            }

            // Down and to right
            iEndCol = iStartCol + 1;
            iEndRow = iStartRow - 1;
            if (iEndCol < 9 && iEndRow > 0)
            {
                ChessMove move = new ChessMove(Location.ColLoc, Location.RowLoc, (ColPos)iEndCol, (RowPos)iEndRow);

                if (Board.CanMove(move.StartLocation, move.EndLocation))
                {
                    yield return move;
                }
            }

            //-------------- Castling -------------------
            if (this.InitialPosition)
            {
                ChessPieceLocation right = new ChessPieceLocation(ColPos.G, Location.RowLoc);
                if (Board.CanMove(Location, right))
                {
                    yield return new ChessMove(Location, right);
                }

                ChessPieceLocation left = new ChessPieceLocation(ColPos.C, Location.RowLoc);
                if (Board.CanMove(Location, left))
                {
                    yield return new ChessMove(Location, left);
                }
            }
        }
        #endregion

        /// <summary>
        /// Do housekeeping after piece is moved. Return move weight
        /// </summary>
        internal override ChessMoveResults DoMoveHousekeeping(ChessPieceLocation oldPosition, ChessPieceLocation newPosition)
        {
            ChessMoveResults result = new ChessMoveResults();

            if (CanCastle(newPosition))
            {
                if (Board.CurrentPlayerColor == ChessPieceColor.Black)
                {
                    if (newPosition.ColLoc == ColPos.C)
                    {
                        ChessPiece castle = Board[ColPos.A, RowPos.R8];
                        castle.InitialPosition = false;

                        // Deal with Rook
                        Board.MoveChessPiece(ColPos.A, RowPos.R8, ColPos.D, RowPos.R8);
                        result.StartLocation = new ChessPieceLocation(ColPos.A, RowPos.R8);
                        result.EndLocation = new ChessPieceLocation(ColPos.D, RowPos.R8);
                    }
                    else
                    {
                        ChessPiece castle = Board[ColPos.H, RowPos.R8];
                        castle.InitialPosition = false;

                        // Deal with Rook
                        Board.MoveChessPiece(ColPos.H, RowPos.R8, ColPos.F, RowPos.R8);
                        result.StartLocation = new ChessPieceLocation(ColPos.H, RowPos.R8);
                        result.EndLocation = new ChessPieceLocation(ColPos.F, RowPos.R8);
                    }
                }
                else
                {
                    if (newPosition.ColLoc == ColPos.C)
                    {
                        ChessPiece castle = Board[ColPos.A, RowPos.R1];
                        castle.InitialPosition = false;

                        // Deal with Rook
                        Board.MoveChessPiece(ColPos.A, RowPos.R1, ColPos.D, RowPos.R1);
                        result.StartLocation = new ChessPieceLocation(ColPos.A, RowPos.R1);
                        result.EndLocation = new ChessPieceLocation(ColPos.D, RowPos.R1);

                    }
                    else
                    {
                        ChessPiece castle = Board[ColPos.H, RowPos.R1];
                        castle.InitialPosition = false;

                        // Deal with Rook
                        Board.MoveChessPiece(ColPos.H, RowPos.R1, ColPos.F, RowPos.R1);
                        result.StartLocation = new ChessPieceLocation(ColPos.H, RowPos.R1);
                        result.EndLocation = new ChessPieceLocation(ColPos.F, RowPos.R1);
                    }
                }

                result.Result = MoveMessage.CastlingSucceeded;
            }
            else
            {
                result.Result = MoveMessage.MoveSucceeded;
            }

            base.InitialPosition = false;

            return result;
        }

        /// <summary>
        /// Returns if King can move to specified castling position.
        /// </summary>
        private bool CanCastle(ChessPieceLocation newPosition)
        {
            // King is in its initial position
            if (InitialPosition)
            {
                if (base.PieceColor == ChessPieceColor.Black)
                {
                    // Black King moves left
                    if (newPosition.ColLoc == ColPos.C && newPosition.RowLoc == RowPos.R8)
                    {
                        // Left Black Castle hasn't moved
                        ChessPiece rook = Board[ColPos.A, RowPos.R8];
                        if (rook != null && rook.InitialPosition)
                        {
                            if (Board[ColPos.B, RowPos.R8] == null &&
                                Board[ColPos.C, RowPos.R8] == null &&
                                Board[ColPos.D, RowPos.R8] == null &&
                                !IsInCheck())
                            {
                                // Spaces between are empty
                                // King not in check
                                return true;
                            }
                            else
                            {
                                // Pieces between king and rook
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    // Black King moves right
                    else if (newPosition.ColLoc == ColPos.G && newPosition.RowLoc == RowPos.R8)
                    {
                        // Right Blcak Castle hasn't moved
                        ChessPiece rook = Board[ColPos.H, RowPos.R8];
                        if (rook != null && rook.InitialPosition)
                        {
                            if (Board[ColPos.F, RowPos.R8] == null &&
                                Board[ColPos.G, RowPos.R8] == null &&
                                !IsInCheck())
                            {
                                // Spaces between are empty
                                // King not in check
                                return true;
                            }
                            else
                            {
                                // Pieces between king and rook
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    // Black King moves left
                    if (newPosition.ColLoc == ColPos.C && newPosition.RowLoc == RowPos.R1)
                    {
                        // Left White Castle hasn't moved
                        ChessPiece rook = Board[ColPos.A, RowPos.R1];
                        if (rook != null && rook.InitialPosition)
                        {
                            if (Board[ColPos.B, RowPos.R1] == null &&
                                Board[ColPos.C, RowPos.R1] == null &&
                                Board[ColPos.D, RowPos.R1] == null &&
                                !IsInCheck())
                            {
                                // Spaces between are empty
                                // King not in check
                                return true;
                            }
                            else
                            {
                                // Pieces between king and rook
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    // Black King moves right
                    else if (newPosition.ColLoc == ColPos.G && newPosition.RowLoc == RowPos.R1)
                    {
                        // Right White Castle hasn't moved
                        ChessPiece rook = Board[ColPos.H, RowPos.R1];
                        if (rook != null && rook.InitialPosition)
                        {
                            if (Board[ColPos.F, RowPos.R1] == null &&
                                Board[ColPos.G, RowPos.R1] == null &&
                                !IsInCheck())
                            {
                                // Spaces between are empty
                                // King not in check
                                return true;
                            }
                            else
                            {
                                // Pieces between king and rook
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Find if moving the king to the specified position will place the king in check
        /// </summary>
        internal bool IsInCheck(ChessPieceLocation kingsPosition)
        {
            ChessPieceColor opponentColor = base.PieceColor == ChessPieceColor.Black ? ChessPieceColor.White : ChessPieceColor.Black;

            foreach (ChessPiece piece in Board.ChessPieces(opponentColor))
            {
                if (piece.CanMove(kingsPosition))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Find if king is currently in check
        /// </summary>
        internal bool IsInCheck()
        {
            return IsInCheck(base.Location);
        }
    }
}
