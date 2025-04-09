// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace CyberFeedForward.WinUI_Chess.Controls;

using CyberFeedForward.WinUI_Chess.HelperClasses.ChessClasses;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

public sealed partial class ChessBoardUi : UserControl
{
    public ChessBoardUi()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Event for when chess piece successfully moved
    /// </summary>
    public event ChessMoveHandler ChessPieceMoved;

    private void TheChessBoard_Loaded(object sender, RoutedEventArgs e)
    {

    }
}
