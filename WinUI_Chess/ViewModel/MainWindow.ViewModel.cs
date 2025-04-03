//
//
namespace TrevyBurgess.Games.TrevyChess.ChessGameUI
{
    using System;
    using TrevyBurgess.Games.TrevyChess.ChessGameUI.HelperClasses.Interfaces;

    /// <summary>
    /// Data source for the main window view
    /// </summary>
    public partial class MainWindowViewModel : ViewModelBase
    {
        private IChessBoardControl chessBoardControl;

        public MainWindowViewModel(IChessBoardControl chessBoardControl)
        {
            this.chessBoardControl = chessBoardControl;
            this.chessBoardControl.ChessPieceMoved +=
            (chessBoard, e) =>
            {
                UndoMoveCommand.RaiseCanExecuteChanged();
                RedoMoveCommand.RaiseCanExecuteChanged();

                StatusMessages = e.StatusMessage;
                AiComments = e.AiComments;
                ChessCodes = e.ChessCode;
            };

            StatusMessages = WinUI_Chess.Properties.Resources.Status_PlayersTurn_White;
            AiComments = Properties.Resources.NoMessage;
            ComputerColor = ChessPieceColor.Black;

            chessBoardControl.PlayAgainstComputer = PlayAgainstComputer;
            chessBoardControl.DifficultyLevel = DifficultyLevel;

            // Computer started move
            this.chessBoardControl.ComputerMoveStarted += () =>
            {
                RaisePropertyChanged("IsPlayerTurn");
            };

            // Computer finished turn
            this.chessBoardControl.ComputerMoveEnded += () =>
            {
                RaisePropertyChanged("IsPlayerTurn");
            };
        }

        #region File Commands
        /// <summary>
        /// Set new game
        /// </summary>
        public RelayCommand NewGameCommand
        {
            get
            {
                if (_NewGameCommand == null)
                {
                    _NewGameCommand = new RelayCommand(
                        () =>
                        {
                            chessBoardControl.NewGame();
                            StatusMessages = Properties.Resources.Status_PlayersTurn_White;
                            AiComments = Properties.Resources.NoMessage;
                            ChessCodes = chessBoardControl.BoardState;

                            UndoMoveCommand.RaiseCanExecuteChanged();
                            RedoMoveCommand.RaiseCanExecuteChanged();
                        }
                    );
                }

                return _NewGameCommand;
            }
        }
        private RelayCommand _NewGameCommand;

        /// <summary>
        /// Exit game
        /// </summary>
        public RelayCommand ExitGameCommand
        {
            get
            {
                if (_ExitGameCommand == null)
                {
                    _ExitGameCommand = new RelayCommand(
                        () =>
                        {
                            Application.Current.Shutdown();
                        }
                    );
                }

                return _ExitGameCommand;
            }
        }
        public RelayCommand _ExitGameCommand;
        #endregion

        #region View Commands
        /// <summary>
        /// If true, show toolbar
        /// </summary>
        public bool ViewToolBar
        {
            get
            {
                return Properties.Settings.Default.ToolbarVisible;
            }
            set
            {
                if (Properties.Settings.Default.ToolbarVisible != value)
                {
                    Properties.Settings.Default.ToolbarVisible = value;
                    Properties.Settings.Default.Save();
                    RaisePropertyChanged("ViewToolBar");
                }
            }
        }

        /// <summary>
        /// If true, display status
        /// </summary>
        public bool ViewStatus
        {
            get
            {
                return Properties.Settings.Default.StatusBarVisible;
            }
            set
            {
                if (Properties.Settings.Default.StatusBarVisible != value)
                {
                    Properties.Settings.Default.StatusBarVisible = value;
                    Properties.Settings.Default.Save();
                    RaisePropertyChanged("ViewStatus");

                    RaisePropertyChanged("DisplayCommand_ShowAiComments");
                }
            }
        }

        /// <summary>
        /// Display the command to to show AI comments
        /// </summary>
        public bool DisplayCommand_ShowAiComments
        {
            get
            {
                return PlayAgainstComputer && ViewStatus;
            }
        }

        /// <summary>
        /// Game status messages
        /// </summary>
        public string StatusMessages
        {
            get
            {
                return _StatusMessages;
            }
            set
            {
                if (_StatusMessages == null || !_StatusMessages.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    _StatusMessages = value;
                    RaisePropertyChanged("StatusMessages");
                }
            }
        }
        private string _StatusMessages;

        /// <summary>
        /// If true, display funny messages AI gives
        /// </summary>
        public bool ShowAiComments
        {
            get
            {
                return Properties.Settings.Default.ShowAiComments;
            }
            set
            {
                if (Properties.Settings.Default.ShowAiComments != value)
                {
                    Properties.Settings.Default.ShowAiComments = value;
                    Properties.Settings.Default.Save();

                    RaisePropertyChanged("ShowAiComments");
                    RaisePropertyChanged("DisplayCommandShowAiComments");
                }
            }
        }

        /// <summary>
        /// Display messages AI gives
        /// </summary>
        public string AiComments
        {
            get
            {
                return _AiComments;
            }
            set
            {
                if (_AiComments == null || !_AiComments.Equals(value, StringComparison.InvariantCultureIgnoreCase))
                {
                    _AiComments = value;
                    RaisePropertyChanged("AiComments");
                }
            }
        }
        private string _AiComments;

        /// <summary>
        /// Rotate board 180 degrees
        /// </summary>
        public RelayCommand RotateBoardCommand
        {
            get
            {
                if (_RotateBoardCommand == null)
                {
                    _RotateBoardCommand = new RelayCommand(
                        () =>
                        {
                            if (chessBoardControl.Rotate180)
                            {
                                chessBoardControl.Rotate180 = false;
                            }
                            else
                            {
                                chessBoardControl.Rotate180 = true;
                            }
                        }
                    );
                }

                return _RotateBoardCommand;
            }
        }
        private RelayCommand _RotateBoardCommand;
        #endregion

        #region Chess Commands
        /// <summary>
        /// Undo chess move
        /// </summary>
        public RelayCommand UndoMoveCommand
        {
            get
            {
                if (_UndoChessMove == null)
                {
                    _UndoChessMove = new RelayCommand(
                        () =>
                        {
                            chessBoardControl.UndoMove();

                            UndoMoveCommand.RaiseCanExecuteChanged();
                            RedoMoveCommand.RaiseCanExecuteChanged();
                            RaisePropertyChanged("ChessCodes");
                            RaisePropertyChanged("StatusMessages");

                        },
                        () =>
                        {
                            return chessBoardControl.CanUndoMove();
                        }
                    );
                }

                return _UndoChessMove;
            }
        }
        private RelayCommand _UndoChessMove;

        /// <summary>
        /// Redo chess move
        /// </summary>
        public RelayCommand RedoMoveCommand
        {
            get
            {
                if (_RedoChessMove == null)
                {
                    _RedoChessMove = new RelayCommand(
                        () =>
                        {
                            chessBoardControl.RedoMove();

                            UndoMoveCommand.RaiseCanExecuteChanged();
                            RedoMoveCommand.RaiseCanExecuteChanged();

                            RaisePropertyChanged("ChessCodes");
                            RaisePropertyChanged("StatusMessages");

                        },
                        () =>
                        {
                            return chessBoardControl.CanRedoMove();
                        }
                    );
                }

                return _RedoChessMove;
            }
        }
        private RelayCommand _RedoChessMove;

        /// <summary>
        /// Set if user wants to play against computer
        /// </summary>
        public bool PlayAgainstComputer
        {
            get
            {
                return Properties.Settings.Default.PlayAgainstComputer;
            }
            set
            {
                if (Properties.Settings.Default.PlayAgainstComputer != value)
                {
                    chessBoardControl.PlayAgainstComputer = value;

                    Properties.Settings.Default.PlayAgainstComputer = value;
                    Properties.Settings.Default.Save();

                    RaisePropertyChanged("PlayAgainstComputer");
                    RaisePropertyChanged("DisplayCommand_ShowAiComments");
                }
            }
        }

        /// <summary>
        /// Get/Set difficulty level when playing against computer
        /// </summary>
        public int DifficultyLevel
        {
            get
            {
                return Properties.Settings.Default.Difficulty;
            }
            set
            {
                if (Properties.Settings.Default.Difficulty != value)
                {
                    chessBoardControl.DifficultyLevel = value;

                    Properties.Settings.Default.Difficulty = value;
                    Properties.Settings.Default.Save();

                    RaisePropertyChanged("DifficultyLevel");
                }
            }
        }

        /// <summary>
        /// Get/Set if player is white
        /// </summary>
        public ChessPieceColor ComputerColor
        {
            get
            {
                return chessBoardControl.ComputerColor;
            }
            set
            {
                chessBoardControl.ComputerColor = value;
                RaisePropertyChanged("ComputerColor");
            }
        }

        /// <summary>
        /// If true, computer is making move
        /// </summary>
        public bool IsPlayerTurn
        {
            get
            {
                return chessBoardControl.IsPlayerTurn;
            }
        }
        #endregion

        #region Tools Commands
        /// <summary>
        /// Return weather we should show chess codes
        /// </summary>
        public bool ShowChessCodes
        {
            get
            {
                return Properties.Settings.Default.ShowChessCodes;
            }
            set
            {
                if (Properties.Settings.Default.ShowChessCodes != value)
                {
                    Properties.Settings.Default.ShowChessCodes = value;
                    Properties.Settings.Default.Save();

                    RaisePropertyChanged("ShowChessCodes");
                }
            }
        }

        /// <summary>
        /// Code used to describe locations of chess pieces
        /// </summary>
        public string ChessCodes
        {
            get
            {
                return chessBoardControl.BoardState;
            }
            set
            {
                // Updating chess board
                if (chessBoardControl.BoardState != value)
                {
                    chessBoardControl.BoardState = value;

                    RaisePropertyChanged("ChessCodes");
                    UndoMoveCommand.RaiseCanExecuteChanged();
                    RedoMoveCommand.RaiseCanExecuteChanged();
                }
            }
        }

        /// <summary>
        /// Update chess move
        /// </summary>
        public RelayCommand UpdateBoardCommand
        {
            get
            {
                if (_UpdateBoard == null)
                {
                    _UpdateBoard = new RelayCommand(
                        () =>
                        {
                            RaisePropertyChanged("ChessCodes");
                            UndoMoveCommand.RaiseCanExecuteChanged();
                            RedoMoveCommand.RaiseCanExecuteChanged();
                        },
                        () =>
                        {
                            return true;
                        }
                    );
                }

                return _UpdateBoard;
            }
        }
        private RelayCommand _UpdateBoard;

        /// <summary>
        /// If true, let player move pieces, disregarding chess rules
        /// </summary>
        public bool PlayWithoutRules
        {
            get
            {
                return Properties.Settings.Default.PlayWithoutRules;
            }
            set
            {
                if (Properties.Settings.Default.PlayWithoutRules != value)
                {
                    Properties.Settings.Default.PlayWithoutRules = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        /// <summary>
        /// Undo chess move
        /// </summary>
        public RelayCommand RefreshBoardCommand
        {
            get
            {
                if (_RefreshBoard == null)
                {
                    _RefreshBoard = new RelayCommand(
                        () =>
                        {
                            chessBoardControl.RefreshBoard();
                        },
                        () =>
                        {
                            return true;
                        }
                    );
                }

                return _RefreshBoard;
            }
        }
        private RelayCommand _RefreshBoard;
        #endregion
    }
}
