//
//
namespace TrevyBurgess.Games.TrevyChess.ChessBoardLogic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using TrevyBurgess.Games.TrevyChess.ChessBoardLogic.HelperClasses;
    using TrevyBurgess.Games.TrevyChess.ChessBoardLogic.Internal;

    /// <summary>
    /// Encapsulate chess logic associated the chess board
    /// </summary>
    public class ChessBoard
    {
        /// <summary>
        /// New game board state
        /// </summary>
        public string NewGameBoardState { get { return _NewGameBoardState; } }
        private const string _NewGameBoardState = "W_RA1_NB1_BC1_QD1_KE1_BF1_NG1_RH1_PA2_PB2_PC2_PD2_PE2_PF2_PG2_PH2_pA7_pB7_pC7_pD7_pE7_pF7_pG7_pH7_rA8_nB8_bC8_qD8_kE8_bF8_nG8_rH8_0U";
        private const int pieceCount = 16; // One side has 16 black, the other side has 16 white.

        /// <summary>
        /// Length of string used to encode the state of the chess board.
        /// </summary>
        internal const int C_BoardStateStringLength = 132;

        #region Constructors
        /// <summary>
        /// Create chess board with pieces in their starting positions
        /// </summary>
        public ChessBoard()
        {
            ResetBoard();
        }

        /// <summary>
        /// Create the board
        /// </summary>
        /// <param name="boardLayout">layout of the chess board</param>
        public ChessBoard(string boardLayout)
        {
            Contract.Requires<ArgumentNullException>(boardLayout != null, "Can't be null");
            Contract.Assume(boardLayout.Length == C_BoardStateStringLength, "Invalid input string");

            // Reset Undo list
            undoList = new ChessMoveUndoRedoList(boardLayout);

            // Set board state
            BoardState = boardLayout;
        }

        public ChessBoard(ChessBoard board)
        {
            Contract.Requires<ArgumentNullException>(board != null, "Can't be null");
            Contract.Requires<ArgumentException>(board.BoardState.Length == C_BoardStateStringLength, "Invalid input string");

            // Reset Undo list
            undoList = new ChessMoveUndoRedoList(board.BoardState);

            // Set board state
            BoardState = board.BoardState;
        }

        /// <summary>
        /// Resets the chess board to its initial condition, so a new game can start
        /// </summary>
        public void ResetBoard()
        {
            Contract.Assume(_NewGameBoardState.Length == C_BoardStateStringLength);

            // Reset Undo list
            undoList = new ChessMoveUndoRedoList(_NewGameBoardState);

            // Setup the board
            BoardState = _NewGameBoardState;
        }
        #endregion

        #region Board State
        /// <summary>
        /// Get the state of the chess board
        /// </summary>
        /// <Pattern>Memento - Capture and restore an object's internal state</Pattern>
        public string BoardState
        {
            get
            {
                return ChessBoardEncoder.EncodeBoard(CurrentPlayerColor, allChessPieces, enPassantColumn);
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value != null, "Can't be null");
                Contract.Requires<ArgumentException>(value.Length == C_BoardStateStringLength, "Invalid input string");

                theChessBoard = new ChessPiece[9, 9];
                allChessPieces = new ChessPiece[32];

                ChessBoardEncoder.DecodeBoardState(this, value, out _CurrentPlayerColor, allChessPieces);

                foreach (ChessPiece chessPiece in allChessPieces)
                {
                    SetChessPiece(chessPiece);
                    if (chessPiece.PieceType == ChessPieceType.King)
                    {
                        if (chessPiece.PieceColor == ChessPieceColor.Black)
                            blackKing = chessPiece as King;
                        else
                            whiteKing = chessPiece as King;
                    }
                }

                enPassantColumn = undoList.GetEnPassantColumn();
            }
        }

        /// <summary>
        /// Returns all the pieces on the chess board
        /// </summary>
        public IEnumerable<ChessPiece> GetAllChessPieces()
        {
            foreach (ChessPiece piece in allChessPieces)
            {
                yield return piece;
            }
        }

        /// <summary>
        /// Returns live pieces of the specified color.
        /// If 'None', return all live pieces
        /// </summary>
        public IEnumerable<ChessPiece> GetLivePieces(ChessPieceColor pieceColor)
        {
            switch (pieceColor)
            {
                case ChessPieceColor.Black:
                    foreach (ChessPiece piece in allChessPieces)
                    {
                        if (piece.PieceColor == ChessPieceColor.Black && !piece.IsCaptured)
                            yield return piece;
                    }
                    break;

                case ChessPieceColor.White:
                    foreach (ChessPiece piece in allChessPieces)
                    {
                        if (piece.PieceColor == ChessPieceColor.White && !piece.IsCaptured)
                            yield return piece;
                    }
                    break;

                default:
                    foreach (ChessPiece piece in allChessPieces)
                    {
                        if (!piece.IsCaptured)
                            yield return piece;
                    }
                    break;
            }
        }

        /// <summary>
        /// Returns a list of all possible moves the current player can make
        /// </summary>
        public IEnumerable<ChessMove> PossibleChessMoves(ChessPieceColor sideToCheck)
        {
            // Get list of live pieces current player has
            List<ChessPiece> currentPlayerPieces = new List<ChessPiece>(pieceCount);
            foreach (ChessPiece chessPiece in allChessPieces)
            {
                if (chessPiece.PieceColor == sideToCheck && !chessPiece.IsCaptured)
                {
                    currentPlayerPieces.Add(chessPiece);
                }
            }

            // Create list of all possible chess moves
            foreach (ChessPiece piece in currentPlayerPieces)
            {
                foreach (ChessMove move in piece.PossibleMoveList())
                    yield return move;
            }
        }
        #endregion

        #region Move chess pieces
        /// <summary>
        /// Move specified chess piece
        /// </summary>
        public ChessMoveResults Move(ChessMove move, bool moveFreeStyle, bool computerMove = false)
        {
            return Move(move.StartLocation, move.EndLocation, moveFreeStyle, computerMove);
        }

        /// <summary>
        /// Move specified chess piece
        /// </summary>
        public ChessMoveResults Move(ChessPieceLocation oldPosition, ChessPieceLocation newPosition, bool moveFreeStyle, bool computerMove = false)
        {
            Contract.Requires<ArgumentException>(ValidateBounds.IsInBounds(oldPosition), "oldPosition contains values that are out of bounds.");
            Contract.Requires<ArgumentException>(ValidateBounds.IsInBounds(newPosition), "newPosition contains values that are out of bounds.");

            ChessPiece piece = theChessBoard[(int)oldPosition.ColLoc, (int)oldPosition.RowLoc];
            if (piece != null)
            {
                ChessMoveResults moveResult = piece.Move(newPosition, moveFreeStyle);

                enPassantColumn = moveResult.EnPassantColumn;

                undoList.AddMoveToUndoList(BoardState, computerMove);

                return moveResult;
            }
            else
            {
                throw new ArgumentException("There is no chess piece at oldPosition: " + oldPosition.ColLoc.ToString() + ", " + oldPosition.RowLoc.ToString());
            }
        }

        private ColPos enPassantColumn = ColPos.Captured;

        /// <summary>
        /// Return if specified chess move is legal
        /// </summary>
        public bool CanMove(ChessPieceLocation oldPosition, ChessPieceLocation newPosition)
        {
            if (ValidateBounds.IsInBounds(oldPosition) && ValidateBounds.IsInBounds(newPosition))
            {
                ChessPiece piece = theChessBoard[(int)oldPosition.ColLoc, (int)oldPosition.RowLoc];
                if (piece == null)
                {
                    // No piece to move
                    return false;
                }
                else if (piece.PieceColor == CurrentPlayerColor)
                {
                    ChessPiece endPiece = theChessBoard[(int)newPosition.ColLoc, (int)newPosition.RowLoc];
                    if (endPiece != null && endPiece.PieceColor == CurrentPlayerColor)
                    {
                        // Trying to capture your own piece
                        return false;
                    }
                    else if (piece.CanMove(newPosition))
                    {
                        #region Test if move puts own piece in check
                        // Store current state
                        string currentBoard = BoardState;
                        ChessPieceColor currentPlayer = CurrentPlayerColor;

                        // Test move
                        piece.Move(newPosition, true);
                        bool isInCheck = IsKingInCheck(currentPlayer);

                        // Restore board
                        BoardState = currentBoard;

                        return !isInCheck;
                        #endregion
                    }
                    else
                    {
                        // Piece can't move
                        return false;
                    }
                }
                else
                {
                    // Not player's turn
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Return if piece can move from current position
        /// </summary>
        public bool CanMove(ChessPieceLocation location)
        {
            if (ValidateBounds.IsInBounds(location))
            {
                ChessPiece piece = theChessBoard[(int)location.ColLoc, (int)location.RowLoc];
                if (piece == null)
                {
                    // No piece to move
                    return false;
                }
                else if (piece.PieceColor == CurrentPlayerColor)
                {
                    return piece.CanMove();
                }
                else
                {
                    // Not player's turn
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region Track chess pieces
        /// <summary>
        /// Get contents of chess board
        /// Returns chess piece if present, or null if empty
        /// </summary>
        public ChessPiece this[ChessPieceLocation location]
        {
            get
            {
                Contract.Requires<ArgumentException>(ValidateBounds.IsInBounds(location), "Location is out of bounds.");

                return theChessBoard[(int)location.ColLoc, (int)location.RowLoc];
            }
        }

        /// <summary>
        /// Get contents of chess board
        /// Returns chess piece if present, or null if empty
        /// </summary>
        public ChessPiece this[ColPos colPos, RowPos rowPos]
        {
            get
            {
                Contract.Requires<ArgumentException>(ValidateBounds.IsInBounds(colPos), "Column location is out of bounds.");
                Contract.Requires<ArgumentException>(ValidateBounds.IsInBounds(rowPos), "Row location is out of bounds.");

                return theChessBoard[(int)colPos, (int)rowPos];
            }
        }

        /// <summary>
        /// Get contents of chess board
        /// Returns chess piece if present, or null if empty
        /// </summary>
        internal ChessPiece this[int colPos, int rowPos]
        {
            get
            {
                Contract.Requires<ArgumentException>(ValidateBounds.IsInBounds((ColPos)colPos), "Column location is out of bounds.");
                Contract.Requires<ArgumentException>(ValidateBounds.IsInBounds((RowPos)rowPos), "Row location is out of bounds.");

                return theChessBoard[colPos, rowPos];
            }
        }
        private ChessPiece[,] theChessBoard;

        /// <summary>
        /// Used to initialize chess board
        /// </summary>
        internal void SetChessPiece(ChessPiece piece)
        {
            Contract.Requires<ArgumentNullException>(piece != null);

            if (!piece.IsCaptured)
            {
                theChessBoard[(int)piece.Location.ColLoc, (int)piece.Location.RowLoc] = piece;
            }
        }

        internal void CaptureChessPiece(ChessPiece piece)
        {
            Contract.Requires<ArgumentNullException>(piece != null);

            // update board
            theChessBoard[(int)piece.Location.ColLoc, (int)piece.Location.RowLoc] = null;

            // Mark as captured
            piece.Location = new ChessPieceLocation();
        }

        /// <summary>
        /// Move the chess piece from the source location to t he destination location.
        /// </summary>
        /// <param name="oldColPos">Old column position of chess piece</param>
        /// <param name="oldRowPos">Old row position of chess piece</param>
        /// <param name="newColPos">New column position of chess piece</param>
        /// <param name="newRowPos">New row position of chess piece</param>
        internal void MoveChessPiece(ColPos oldColPos, RowPos oldRowPos, ColPos newColPos, RowPos newRowPos)
        {
            if (theChessBoard[(int)oldColPos, (int)oldRowPos] == null)
                throw new IllegalMoveException("There is no chess piece at the old position.");

            // Capture destination piece if it exists
            ChessPiece destinationPiece = theChessBoard[(int)newColPos, (int)newRowPos];
            if (destinationPiece != null)
            {
                destinationPiece.Location = new ChessPieceLocation();
            }

            // Copy piece to destination
            theChessBoard[(int)newColPos, (int)newRowPos] = theChessBoard[(int)oldColPos, (int)oldRowPos];

            // Delete from destination
            theChessBoard[(int)oldColPos, (int)oldRowPos] = null;

            // Update chess piece location
            theChessBoard[(int)newColPos, (int)newRowPos].Location = new ChessPieceLocation(newColPos, newRowPos);
        }

        /// <summary>
        /// Used to track all pieces
        /// </summary>
        private ChessPiece[] allChessPieces;

        /// <summary>
        /// Returns all the pieces on the chess board of the specified color
        /// </summary>
        /// <remarks>
        /// Just used as an example of an iterator. We would use a dedicated array for speed
        /// </remarks>
        internal IEnumerable<ChessPiece> ChessPieces(ChessPieceColor color)
        {
            foreach (ChessPiece chessPiece in allChessPieces)
            {
                if (chessPiece != null && chessPiece.PieceColor == color)
                {
                    yield return chessPiece;
                }
            }
        }

        /// <summary>
        /// return number of powers specified color has
        /// </summary>
        public int NumberOfPowers(ChessPieceColor playerColor)
        {
            int noOfPowers = 0;

            foreach (ChessPiece chessPiece in allChessPieces)
            {
                if (chessPiece.PieceColor == playerColor && chessPiece.IsPower)
                    noOfPowers++;
            }

            return noOfPowers;
        }

        /// <summary>
        /// Promote pawn in specific location to a power
        /// </summary>
        /// <param name="chessPieceType">Piece you want to promote pawn to</param>
        /// <param name="colPos">Column position of pawn</param>
        /// <param name="rowPos">Row position of pawn</param>
        public void PromotePawn(ChessPieceType chessPieceType, ColPos colPos, RowPos rowPos, bool computerMove = false)
        {
            for (short i = 0; i < allChessPieces.Length; i++)
            {
                if (allChessPieces[i].Location.ColLoc == colPos && allChessPieces[i].Location.RowLoc == rowPos)
                {
                    allChessPieces[i] =
                        ChessPieceFactory.GetNewChessPiece(
                        this,
                        chessPieceType,
                        allChessPieces[i].PieceColor,
                        new ChessPieceLocation(colPos, rowPos),
                        false,
                        i);
                    SetChessPiece(allChessPieces[i]);

                    // Get rid of move where pawn is in end of board
                    undoList.GetPreviousMove();
                    undoList.AddMoveToUndoList(BoardState, computerMove);
                    return;
                }
            }

            throw new IllegalMoveException("There is no pawn in specified location: " + colPos.ToString() + ", " + rowPos.ToString());
        }

        /// <summary>
        /// get the color of the player whose move it currently is.
        /// </summary>
        public ChessPieceColor CurrentPlayerColor
        {
            get { return _CurrentPlayerColor; }
            set { _CurrentPlayerColor = value; }
        }
        private ChessPieceColor _CurrentPlayerColor;


        // Used to track if king is in check
        private King blackKing;
        private King whiteKing;
        #endregion

        #region Undo-Redo list
        internal ChessMoveUndoRedoList undoList;

        /// <summary>
        /// Return true if can undo current move
        /// </summary>
        public bool CanUndoMove()
        {
            return undoList.CanGetPreviousState();
        }

        /// <summary>
        /// Undo last chess move
        /// </summary>
        public void UndoMove()
        {
            BoardState = undoList.GetPreviousMove();
        }

        /// <summary>
        /// Return true if can redo current move
        /// </summary>
        public bool CanRedoMove()
        {
            return undoList.CanGetNextState();
        }

        /// <summary>
        /// Redo last chess move
        /// </summary>
        public void RedoMove()
        {
            BoardState = undoList.GetNextMove();
        }
        #endregion

        #region End game scenarios
        public EndGameState CheckEndGame()
        {
            if (IsADraw())
            {
                return EndGameState.IsADraw;
            }
            else if (KingIsInCheckMate(CurrentPlayerColor))
            {
                return EndGameState.CheckMate;
            }
            else if (IsKingInCheck(CurrentPlayerColor))
            {
                return EndGameState.IsCheck;
            }
            else
            {
                return EndGameState.GameHasNotEnded;
            }
        }

        /// <summary>
        /// Return if specified king is in check
        /// </summary>
        public bool IsKingInCheck(ChessPieceColor sideToCheck)
        {
            if (sideToCheck == ChessPieceColor.Black)
            {
                return blackKing.IsInCheck();
            }
            else
            {
                return whiteKing.IsInCheck();
            }
        }

        /// <summary>
        /// Return true if player is out of moves
        /// </summary>
        /// <returns></returns>
        public bool KingIsInCheckMate(ChessPieceColor sideToCheck)
        {
            foreach (ChessMove move in PossibleChessMoves(sideToCheck))
            {
                // player has moves available
                return false;
            }

            // no moves available
            return true;
        }

        /// <summary>
        /// Return true if game is a draw - only the two kings are on board.
        /// </summary>
        public bool IsADraw()
        {
            foreach (ChessPiece chessPiece in allChessPieces)
            {
                if (!chessPiece.IsCaptured && chessPiece != blackKing && chessPiece != whiteKing)
                {
                    // Non-king found
                    return false;
                }
            }

            // Only kings remaining
            return true;
        }
        #endregion
    }
}
