//
//
namespace TrevyBurgess.Games.TrevyChess.ChessGameUI.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using TrevyBurgess.Games.TrevyChess.ChessBoardLogic;
    using TrevyBurgess.Games.TrevyChess.ChessGameAI;
    using TrevyBurgess.Games.TrevyChess.ChessGameUI.HelperClasses.Interfaces;

    /// <summary>
    /// Interaction logic for ChessBoard.xaml
    /// </summary>
    public partial class ChessBoardControl : UserControl, IChessBoardControl
    {
        /// <summary>
        /// Event for when chess piece successfully moved
        /// </summary>
        public event ChessMoveHandler ChessPieceMoved;

        public event ComputerMoveEndedHandler ComputerMoveEnded;
        public event ComputerMoveStartedHandler ComputerMoveStarted;

        public ChessBoardControl()
        {
            InitializeComponent();

            // populate chess pieces
            _InternalChessBoard = new ChessBoard();

            chessPieceControls = new Dictionary<short, ChessPieceControl>();
        }

        #region Chess Board State
        /// <summary>
        /// Get the current player
        /// </summary>
        public ChessPieceColor PlayerColor { get { return _InternalChessBoard.CurrentPlayerColor; } }

        /// <summary>
        /// Return if computer's turn
        /// </summary>
        public bool IsPlayerTurn { get { return !(PlayAgainstComputer && PlayerColor == ComputerColor); } }

        /// <summary>
        /// Get/Set the computer's color
        /// Can't change computer color while computer is calculating
        /// </summary>
        public ChessPieceColor ComputerColor
        {
            get
            {
                return _ComputerColor;
            }
            set
            {
                if (IsPlayerTurn)
                {
                    _ComputerColor = value;

                    TryComputerMove();
                }
                else
                {
                    throw new NotSupportedException("Can't change computer's color when it's computer's turn.");
                }
            }
        }
        private ChessPieceColor _ComputerColor;

        /// <summary>
        /// Get/Set how challenging AI is
        /// </summary>
        public int DifficultyLevel
        {
            get
            {
                return _DifficultyLevel;
            }
            set
            {
                if (IsPlayerTurn)
                {
                    _DifficultyLevel = value;
                }
                else
                {
                    throw new NotSupportedException("Can't change difficulty level when it's computer's turn.");
                }
            }
        }
        private int _DifficultyLevel;

        /// <summary>
        /// Get/set if you want to play against computer
        /// </summary>
        public bool PlayAgainstComputer
        {
            get
            {
                return _PlayAgainstComputer;
            }
            set
            {
                if (_PlayAgainstComputer != value)
                {
                    if (IsPlayerTurn)
                    {
                        if (value)
                        {
                            TryComputerMove();
                        }
                    }
                    else
                    {
                        if (value == false)
                        {
                            ComputerMoveEnded();
                            cancelTokenSource.Cancel();
                        }
                    }

                    _PlayAgainstComputer = value;
                }
            }
        }
        private bool _PlayAgainstComputer;

        /// <summary>
        /// Get/Set the board state
        /// </summary>
        public string BoardState
        {
            get
            {
                return _InternalChessBoard.BoardState;
            }
            set
            {
                if (IsPlayerTurn)
                {
                    if (!_InternalChessBoard.BoardState.Equals(value))
                    {
                        _InternalChessBoard.BoardState = value;

                        RefreshBoard();
                    }
                }
                else
                {
                    throw new NotSupportedException("Board can't be changed on computer's turn.");
                }
            }
        }

        /// <summary>
        /// Get/set weather you want to play without rules
        /// </summary>
        public bool PlayWithoutRules { get; set; }

        /// <summary>
        /// Rotate board 180 degrees
        /// </summary>
        public bool Rotate180
        {
            get
            {
                return _Rotate180;
            }
            set
            {
                if (_Rotate180 != value)
                {
                    _Rotate180 = value;

                    // Rotate board
                    if (_Rotate180)
                    {
                        TransformGroup boardGroup = new TransformGroup();
                        boardGroup.Children.Add(new RotateTransform(180));
                        TranslateTransform boardMove = new TranslateTransform(base.ActualWidth, base.ActualHeight);
                        boardGroup.Children.Add(boardMove);
                        base.RenderTransform = boardGroup;

                        foreach (TextBlock rowAndColNo in colAndRowNumbers)
                        {
                            TransformGroup numberGroup = new TransformGroup();
                            numberGroup.Children.Add(new RotateTransform(180));
                            TranslateTransform numberMove = new TranslateTransform(rowAndColNo.ActualWidth, rowAndColNo.ActualHeight);
                            numberGroup.Children.Add(numberMove);
                            rowAndColNo.RenderTransform = numberGroup;
                        }
                    }
                    else
                    {
                        base.RenderTransform = MatrixTransform.Identity;

                        foreach (TextBlock rowAndColNo in colAndRowNumbers)
                        {
                            rowAndColNo.RenderTransform = MatrixTransform.Identity;
                        }
                    }

                    // Rotate pieces
                    foreach (ChessPieceControl piece in chessPieceControls.Values)
                    {
                        piece.Rotate180(value);
                    }
                }
            }
        }
        private bool _Rotate180;

        /// <summary>
        /// Update chess pieces on board
        /// </summary>
        public void RefreshBoard()
        {
            if (IsPlayerTurn)
            {
                foreach (ChessPiece piece in _InternalChessBoard.GetAllChessPieces())
                {
                    ChessPieceControl chessPieceControl = chessPieceControls[piece.ID];

                    chessPieceControl.PieceColor = piece.PieceColor;
                    chessPieceControl.PieceType = piece.PieceType;
                    chessPieceControl.PieceImage = ChessPieceImageSource(piece.PieceColor, piece.PieceType);
                    chessPieceControl.PieceColumnPosition = piece.Location.ColLoc;
                    chessPieceControl.PieceRowPosition = piece.Location.RowLoc;

                    if (piece.Location.ColLoc == ColPos.Captured || piece.Location.RowLoc == RowPos.Captured)
                        chessPieceControl.Visibility = System.Windows.Visibility.Collapsed;
                    else
                        chessPieceControl.Visibility = System.Windows.Visibility.Visible;
                }

                UpdateGameStatus();
            }
        }

        /// <summary>
        /// Return the chess board to its initial position
        /// </summary>
        public void NewGame()
        {
            if (!IsPlayerTurn)
            {
                ComputerMoveEnded();
                cancelTokenSource.Cancel();
            }

            _InternalChessBoard.ResetBoard();

            RefreshBoard();
            TryComputerMove();
        }
        #endregion

        #region Manage Undo-Redo list
        /// <summary>
        /// Return if we can undo current move
        /// </summary>
        public bool CanUndoMove()
        {
            if (IsPlayerTurn)
                return _InternalChessBoard.CanUndoMove();
            else
                return false;
        }

        /// <summary>
        /// Undo move
        /// </summary>
        public void UndoMove()
        {
            if (IsPlayerTurn)
            {
                _InternalChessBoard.UndoMove();

                RefreshBoard();
            }
        }

        /// <summary>
        /// Return if we can redo current move
        /// </summary>
        public bool CanRedoMove()
        {
            if (IsPlayerTurn)
                return _InternalChessBoard.CanRedoMove();
            else
                return false;
        }

        /// <summary>
        /// Redo move
        /// </summary>
        public void RedoMove()
        {
            if (IsPlayerTurn)
            {
                _InternalChessBoard.RedoMove();

                RefreshBoard();

                TryComputerMove();
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Chess board logic
        /// </summary>
        private ChessBoard _InternalChessBoard;

        /// <summary>
        /// Origin of chess board
        /// </summary>
        public Point BoardOrigin { get; private set; }

        /// <summary>
        /// Width of chess board square
        /// </summary>
        public double ChessSquareWidth { get; private set; }

        /// <summary>
        /// Height of chess board square
        /// </summary>
        public double ChessSquareHeight { get; private set; }
        #endregion

        #region Private methods
        #region Movement Control
        /// <summary>
        /// Return if piece can move from current position
        /// </summary>
        private bool CanMove(ChessPieceLocation location)
        {
            return IsPlayerTurn && _InternalChessBoard.CanMove(location);
        }

        /// <summary>
        /// Return if piece can move from current position
        /// </summary>
        private bool CanMove(ChessPieceLocation oldPosition, ChessPieceLocation newPosition)
        {
            return IsPlayerTurn && _InternalChessBoard.CanMove(oldPosition, newPosition);
        }

        /// <summary>
        /// Move specified chess piece
        /// </summary>
        private void Move(ChessPieceControl chessPiece, ChessPieceLocation newPosition, bool playWithoutRules)
        {
            Move(chessPiece, newPosition, playWithoutRules, false);
        }

        /// <summary>
        /// Move specified chess piece
        /// </summary>
        private void Move(ChessPieceControl chessPiece, ChessPieceLocation newPosition, bool playWithoutRules, bool computerMove)
        {
            if (playWithoutRules || CanMove(chessPiece.PieceLocation, newPosition))
            {
                ChessPieceColor pieceColor = this.PlayerColor;

                ChessMoveResults result = _InternalChessBoard.Move(chessPiece.PieceLocation, newPosition, playWithoutRules, computerMove);

                if (result.Result == MoveMessage.PieceCaptured)
                {
                    // Capture piece
                    ChessPieceControl capturedPiece = this.GetPiece(result.PieceKilled.ColLoc, result.PieceKilled.RowLoc);
                    capturedPiece.Visibility = System.Windows.Visibility.Collapsed;
                    capturedPiece.PieceColumnPosition = ColPos.Captured;
                    capturedPiece.PieceRowPosition = RowPos.Captured;
                }
                else if (result.Result == MoveMessage.PawnCapturedEnPassant)
                {
                    // Capture piece en passant
                    if (this.PlayerColor == ChessPieceColor.White)
                    {
                        ChessPieceControl capturedPiece = this.GetPiece(newPosition.ColLoc, RowPos.R4);
                        capturedPiece.Visibility = System.Windows.Visibility.Collapsed;
                        capturedPiece.PieceColumnPosition = ColPos.Captured;
                        capturedPiece.PieceRowPosition = RowPos.Captured;
                    }
                    else
                    {
                        ChessPieceControl capturedPiece = this.GetPiece(newPosition.ColLoc, RowPos.R5);
                        capturedPiece.Visibility = System.Windows.Visibility.Collapsed;
                        capturedPiece.PieceColumnPosition = ColPos.Captured;
                        capturedPiece.PieceRowPosition = RowPos.Captured;
                    }
                }
                else if (result.Result == MoveMessage.CastlingSucceeded)
                {
                    // Move castle
                    ChessPieceControl capturedPiece = this.GetPiece(result.StartLocation.ColLoc, result.StartLocation.RowLoc);
                    capturedPiece.PieceColumnPosition = result.EndLocation.ColLoc;
                    capturedPiece.PieceRowPosition = result.EndLocation.RowLoc;
                }

                // Move piece
                chessPiece.PieceColumnPosition = newPosition.ColLoc;
                chessPiece.PieceRowPosition = newPosition.RowLoc;

                // Deal with pawn promotion
                if (result.PawnPromoted)
                {
                    PawnPromotionChooser chooser = new PawnPromotionChooser(pieceColor);
                    chooser.ShowDialog();

                    this.PromotePawn(chooser.PieceSelected, newPosition.ColLoc, newPosition.RowLoc, computerMove);

                    chessPiece.PieceImage = chooser.Source;
                }

                UpdateGameStatus();
                TryComputerMove();
            }
        }

        /// <summary>
        /// Get computer to make move
        /// </summary>
        private async void TryComputerMove()
        {
            if (PlayAgainstComputer && PlayerColor == ComputerColor)
            {
                if (_InternalChessBoard.CheckEndGame() == EndGameState.GameHasNotEnded)
                {
                    ComputerMoveStarted();
                    cancelTokenSource = new CancellationTokenSource();

                    ChessMove move = await Task<ChessMove>.Factory.StartNew(
                    () =>
                    {
                        return ChessAI.SuggestMoveAsync(_InternalChessBoard, DifficultyLevel + 1).Result;
                    }, cancelTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);

                    ChessPieceControl piece = GetPiece(move.StartLocation.ColLoc, move.StartLocation.RowLoc);
                    this.Move(piece, move.EndLocation, true, true);

                    ComputerMoveEnded();
                }
            }
        }
        CancellationTokenSource cancelTokenSource;
        #endregion

        /// <summary>
        /// Move specified chess piece
        /// </summary>
        private EndGameState CheckEndGame()
        {
            if (IsPlayerTurn)
                return _InternalChessBoard.CheckEndGame();
            else
                return EndGameState.GameHasNotEnded;
        }

        /// <summary>
        /// Promote selected pawn
        /// </summary>
        private void PromotePawn(ChessPieceType chessPieceType, ColPos colPos, RowPos rowPos, bool computerMove)
        {
            _InternalChessBoard.PromotePawn(chessPieceType, colPos, rowPos, computerMove);
        }

        private void TheChessBoard_Loaded(object sender, RoutedEventArgs re)
        {
            BoardOrigin = new Point(base.Width / 26, base.Height / 26);
            ChessSquareWidth = BoardOrigin.X * 3;
            ChessSquareHeight = BoardOrigin.Y * 3;

            // Create chess pieces and place them on the board
            foreach (ChessPiece piece in _InternalChessBoard.GetAllChessPieces())
            {
                var chessPieceControl =
                    new ChessPieceControl(this, Move, CanMove)
                    {
                        PieceColor = piece.PieceColor,
                        PieceType = piece.PieceType,
                        PieceColumnPosition = piece.Location.ColLoc,
                        PieceRowPosition = piece.Location.RowLoc,
                        PieceImage = ChessPieceImageSource(piece.PieceColor, piece.PieceType)
                    };

                chessPieceControls.Add(piece.ID, chessPieceControl);
                chessBoardControl.Children.Add(chessPieceControl);
            }

            #region Record row and column numbers for rotating board
            colAndRowNumbers = new TextBlock[32];
            colAndRowNumbers[0] = TopCol1;
            colAndRowNumbers[1] = TopCol2;
            colAndRowNumbers[2] = TopCol3;
            colAndRowNumbers[3] = TopCol4;
            colAndRowNumbers[4] = TopCol5;
            colAndRowNumbers[5] = TopCol6;
            colAndRowNumbers[6] = TopCol7;
            colAndRowNumbers[7] = TopCol8;

            colAndRowNumbers[8] = LeftRow1;
            colAndRowNumbers[9] = LeftRow2;
            colAndRowNumbers[10] = LeftRow3;
            colAndRowNumbers[11] = LeftRow4;
            colAndRowNumbers[12] = LeftRow5;
            colAndRowNumbers[13] = LeftRow6;
            colAndRowNumbers[14] = LeftRow7;
            colAndRowNumbers[15] = LeftRow8;

            colAndRowNumbers[16] = RightRow1;
            colAndRowNumbers[17] = RightRow2;
            colAndRowNumbers[18] = RightRow3;
            colAndRowNumbers[19] = RightRow4;
            colAndRowNumbers[20] = RightRow5;
            colAndRowNumbers[21] = RightRow6;
            colAndRowNumbers[22] = RightRow7;
            colAndRowNumbers[23] = RightRow8;

            colAndRowNumbers[24] = BottomCol1;
            colAndRowNumbers[25] = BottomCol2;
            colAndRowNumbers[26] = BottomCol3;
            colAndRowNumbers[27] = BottomCol4;
            colAndRowNumbers[28] = BottomCol5;
            colAndRowNumbers[29] = BottomCol6;
            colAndRowNumbers[30] = BottomCol7;
            colAndRowNumbers[31] = BottomCol8;
            #endregion

            Rotate180 = Properties.Settings.Default.WhiteChessPiecesOnTop;
        }

        /// <summary>
        /// Get images for chess pieces
        /// </summary>
        private ImageSource ChessPieceImageSource(ChessPieceColor color, ChessPieceType pieceType)
        {
            if (color == ChessPieceColor.Black)
            {
                switch (pieceType)
                {
                    case ChessPieceType.King:
                        return HelperMethods.GetImageSource(Properties.Resources.B_King);

                    case ChessPieceType.Queen:
                        return HelperMethods.GetImageSource(Properties.Resources.B_Queen);

                    case ChessPieceType.Bishop:
                        return HelperMethods.GetImageSource(Properties.Resources.B_Bishop);

                    case ChessPieceType.Knight:
                        return HelperMethods.GetImageSource(Properties.Resources.B_Knight);

                    case ChessPieceType.Rook:
                        return HelperMethods.GetImageSource(Properties.Resources.B_Rook);

                    case ChessPieceType.Pawn:
                        return HelperMethods.GetImageSource(Properties.Resources.B_Pawn);
                }
            }
            else
            {
                switch (pieceType)
                {
                    case ChessPieceType.King:
                        return HelperMethods.GetImageSource(Properties.Resources.W_King);

                    case ChessPieceType.Queen:
                        return HelperMethods.GetImageSource(Properties.Resources.W_Queen);

                    case ChessPieceType.Bishop:
                        return HelperMethods.GetImageSource(Properties.Resources.W_Bishop);

                    case ChessPieceType.Knight:
                        return HelperMethods.GetImageSource(Properties.Resources.W_Knight);

                    case ChessPieceType.Rook:
                        return HelperMethods.GetImageSource(Properties.Resources.W_Rook);

                    case ChessPieceType.Pawn:
                        return HelperMethods.GetImageSource(Properties.Resources.W_Pawn);
                }
            }

            throw new ArgumentException("ChessPieceColor or ChessPieceType is incorrect.");
        }

        /// <summary>
        /// Raise ChessPieceMoved event
        /// </summary>
        private void UpdateGameStatus(string sillyMessage = null)
        {
            if (ChessPieceMoved != null)
            {
                if (sillyMessage == null)
                    sillyMessage = Properties.Resources.NoMessage;

                EndGameState message = this.CheckEndGame();
                string statusMessage;
                if (message == EndGameState.CheckMate)
                {
                    statusMessage = Properties.Resources.Status_CheckMate;
                }
                else if (message == EndGameState.IsADraw)
                {
                    statusMessage = Properties.Resources.Status_IsADraw;
                }
                else if (message == EndGameState.IsCheck)
                {
                    statusMessage = Properties.Resources.Status_IsCheck;
                }
                else
                {
                    if (this.PlayerColor == ChessPieceColor.Black)
                    {
                        statusMessage = Properties.Resources.Status_PlayersTurn_Black;
                    }
                    else
                    {
                        statusMessage = Properties.Resources.Status_PlayersTurn_White;
                    }
                }

                // Raise event
                ChessPieceMoved(this, new ChessMoveEventArgs(this.PlayerColor, statusMessage, sillyMessage, this.BoardState));
            }
        }

        /// <summary>
        /// Return chess piece located in specified location
        /// </summary>
        private ChessPieceControl GetPiece(ColPos colPos, RowPos rowPos)
        {
            foreach (ChessPieceControl piece in chessPieceControls.Values)
            {
                if (piece.PieceColumnPosition == colPos && piece.PieceRowPosition == rowPos)
                {
                    return piece;
                }
            }

            throw new NotSupportedException("No chess piece found at location (" + colPos.ToString() + ", " + rowPos.ToString() + ")");
        }

        /// <summary>
        /// All chess pieces on board
        /// </summary>
        private Dictionary<short, ChessPieceControl> chessPieceControls { get; set; }

        /// <summary>
        /// Used to rotate columns and row numbers
        /// </summary>
        private TextBlock[] colAndRowNumbers { get; set; }
        #endregion
    }
}
