// 
// 
namespace CyberFeedForward.WinUI_Chess.ViewModel;

using CyberFeedForward.ViewModelBase.Extensions;
using CyberFeedForward.ViewModelBase.Helpers;
using CyberFeedForward.WinUI_Chess.Properties;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

public partial class MainWindowViewModel
{
    public string TrevyChessTitle { get { return Resources.TrevyChessTitle; } }
    public ImageSource TrevyChessIcon { get { return ResourceHelper.GetImageSource(Resources.TrevyChess); } }

    public string UpdateBoardCommand_Name { get { return Resources.UpdateChessBoard; } }

    #region File Command Group
    public string FileGroup_Name { get { return Resources.Menu_File; } }

    public string NewGame_Name { get { return Resources.Menu_File_New; } }
    public string NewGame_Tooltip { get { return Resources.Menu_File_New + " (ctrl + N)"; } }
    public Image NewGame_Image { get { return ResourceHelper.GetImage(Resources.NewGame, 16); } }
    public ImageSource NewGame_ImageSource { get { return ResourceHelper.GetImageSource(Resources.NewGame); } }

    public string PrintGame_Name { get { return Resources.Menu_File_Print; } }
    public string PrintGame_Tooltip { get { return Resources.Menu_File_Print + " (ctrl + P)"; } }
    public Image PrintGame_Image { get { return ResourceHelper.GetImage(Resources.Printer_48x48, 16); } }

    public string ExitGame_Name { get { return Resources.Menu_File_Print; } }
    public Image ExitGame_Image { get { return ResourceHelper.GetImage(Resources.Cose_16x16, 16); } }
    #endregion

    #region View Group
    public string ViewGroup_Name { get { return Resources.Menu_View; } }

    public string ViewToolBar_Name { get { return Resources.Menu_View_ToolBar; } }
    public Image ViewToolBar__Image { get { return ResourceHelper.GetImage(Resources.Toolbars, 16); } }

    public string ViewStatusBar_Name { get { return Resources.Menu_View_StatusBar; } }

    public string AiComments_Name { get { return Resources.Menu_View_AiComments; } }
    public Image AiComments_Image { get { return ResourceHelper.GetImage(Resources.Printer_48x48, 16); } }

    public string RotateBoard_Name { get { return Resources.Menu_View_RotateBoard; } }
    public Image RotateBoard_Image { get { return ResourceHelper.GetImage(Resources.RotateBoard, 16); } }
    public ImageSource RotateBoard_ImageSource { get { return ResourceHelper.GetImageSource(Resources.RotateBoard); } }
    #endregion

    #region Chess Group
    public string ChessGroup_Name { get { return Resources.Menu_Chess; } }

    public string PlayAgainstComputer_Name { get { return Resources.Menu_Chess_PlayAgainstComputer; } }
    public Image PlayAgainstComputer_Image { get { return ResourceHelper.GetImage(Resources.PlayAgainstComputer, 16); } }
    public ImageSource PlayAgainstComputer_ImageSource { get { return ResourceHelper.GetImageSource(Resources.PlayAgainstComputer); } }

    public string DifficultyLevelGroup_Name { get { return Resources.Menu_Chess_Difficulty; } }
    public string DifficultyLevel_1_Name { get { return Resources.Menu_Chess_Difficulty_1; } }
    public SolidColorBrush DifficultyLevel_1_Background { get { return new SolidColorBrush(Resources.Toolbar_Difficulty_1_Color.Parse()); } }
    public string DifficultyLevel_2_Name { get { return Resources.Menu_Chess_Difficulty_2; } }
    public SolidColorBrush DifficultyLevel_2_Background { get { return new SolidColorBrush(Resources.Toolbar_Difficulty_2_Color.Parse()); } }
    public string DifficultyLevel_3_Name { get { return Resources.Menu_Chess_Difficulty_3; } }
    public SolidColorBrush DifficultyLevel_3_Background { get { return new SolidColorBrush(Resources.Toolbar_Difficulty_3_Color.Parse()); } }
    public string DifficultyLevel_4_Name { get { return Resources.Menu_Chess_Difficulty_4; } }
    public SolidColorBrush DifficultyLevel_4_Background { get { return new SolidColorBrush(Resources.Toolbar_Difficulty_4_Color.Parse()); } }
    public string DifficultyLevel_5_Name { get { return Resources.Menu_Chess_Difficulty_5; } }
    public SolidColorBrush DifficultyLevel_5_Background { get { return new SolidColorBrush(Resources.Toolbar_Difficulty_5_Color.Parse()); } }

    public string ComputerColorGroup_Name { get { return Resources.Menu_Chess_ComputerColor; } }
    public string ComputerColor_Black_Name { get { return Resources.Menu_Chess_ComputerColor_Black; } }
    public string ComputerColor_White_Name { get { return Resources.Menu_Chess_ComputerColor_White; } }

    public string UndoMove_Name { get { return Resources.UndoMove_Name; } }
    public string UndoMove_Tooltip { get { return Resources.UndoMove_Name + " (ctrl + Z)"; } }
    public Image UndoMove_Image { get { return ResourceHelper.GetImage(Resources.UndoMoveImage, 16); } }
    public ImageSource UndoMove_ImageSource { get { return ResourceHelper.GetImageSource(Resources.UndoMoveImage); } }
    public string RedoMove_Name { get { return Resources.RedoMove_Name; } }
    public string RedoMove_Tooltip { get { return Resources.RedoMove_Name + " (ctrl + Y)"; } }
    public Image RedoMove_Image { get { return ResourceHelper.GetImage(Resources.RedoMoveImage, 16); } }
    public ImageSource RedoMove_ImageSource { get { return ResourceHelper.GetImageSource(Resources.RedoMoveImage); } }
    #endregion

    #region Tools Group
    public string ToolsGroup_Name { get { return Resources.Menu_Tools; } }

    public string PlayWithoutRules_Name { get { return Resources.Menu_Tools_PlayWithoutRules; } }

    public string ShowChessCodes_Name { get { return Resources.Menu_Tools_ShowChessCodes; } }
    #endregion

    #region Help Group
    public string HelpGroup_Name { get { return Resources.Menu_Help; } }
    public Image HelpGroup_Image { get { return ResourceHelper.GetImage(Resources.Help_16x16, 16); } }

    public string HelpTopics_Name { get { return Resources.Menu_Help_HelpTopics; } }
    public string AboutGame_Name { get { return Resources.Menu_Help_About; } }
    #endregion
}
