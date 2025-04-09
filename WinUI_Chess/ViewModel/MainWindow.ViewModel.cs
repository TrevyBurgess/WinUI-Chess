//
//
namespace CyberFeedForward.WinUI_Chess.ViewModel;

using CyberFeedForward.ChessBoardLogic.ChessPiece;
using CyberFeedForward.ViewModelBase.Commands;
using CyberFeedForward.ViewModelBase.ViewModels;
using CyberFeedForward.WinUI_Chess.HelperClasses.InterfacesClasses;
using CyberFeedForward.WinUI_Chess.Properties;
using Microsoft.UI.Xaml;
using System;

/// <summary>
/// Data source for the main window view
/// </summary>
public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IChessBoardControl chessBoardControl;

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

        StatusMessages = Resources.Status_PlayersTurn_White;
        AiComments = Resources.NoMessage;
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
                        StatusMessages = Resources.Status_PlayersTurn_White;
                        AiComments = Resources.NoMessage;
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
                        Application.Current.Exit();
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
            return Settings.Default.ToolbarVisible;
        }
        set
        {
            if (Settings.Default.ToolbarVisible != value)
            {
                Settings.Default.ToolbarVisible = value;
                Settings.Default.Save();
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
            return Settings.Default.StatusBarVisible;
        }
        set
        {
            if (Settings.Default.StatusBarVisible != value)
            {
                Settings.Default.StatusBarVisible = value;
                Settings.Default.Save();
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
            return Settings.Default.ShowAiComments;
        }
        set
        {
            if (Settings.Default.ShowAiComments != value)
            {
                Settings.Default.ShowAiComments = value;
                Settings.Default.Save();

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
            return Settings.Default.PlayAgainstComputer;
        }
        set
        {
            if (Settings.Default.PlayAgainstComputer != value)
            {
                chessBoardControl.PlayAgainstComputer = value;

                Settings.Default.PlayAgainstComputer = value;
                Settings.Default.Save();

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
            return Settings.Default.Difficulty;
        }
        set
        {
            if (Settings.Default.Difficulty != value)
            {
                chessBoardControl.DifficultyLevel = value;

                Settings.Default.Difficulty = value;
                Settings.Default.Save();

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
            return Settings.Default.ShowChessCodes;
        }
        set
        {
            if (Settings.Default.ShowChessCodes != value)
            {
                Settings.Default.ShowChessCodes = value;
                Settings.Default.Save();

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
            return Settings.Default.PlayWithoutRules;
        }
        set
        {
            if (Settings.Default.PlayWithoutRules != value)
            {
                Settings.Default.PlayWithoutRules = value;
                Settings.Default.Save();
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
