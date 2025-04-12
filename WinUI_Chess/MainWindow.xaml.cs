// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.
namespace WinUI_Chess;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainWindow : Window
{
    public double InfoBadgeOpacity { get; set; }

    public MainWindow()
    {
        InitializeComponent();

        MenuFileNew.Text = "New";
        MenuFileNew.Icon = new SymbolIcon(Symbol.NewFolder);
        MenuFilePrint.Text = "Print";
        MenuFilePrint.Icon = new SymbolIcon(Symbol.Print);
        MenuFileExit.Text = "Exit";
        MenuFileExit.Icon = new SymbolIcon(Symbol.Emoji);

        MenuViewToolbar.Text = "Show toolbar";
        MenuViewToolbar.Icon = new SymbolIcon(Symbol.ShowResults);
        MenuViewStatusBar.Text = "Show status bar";
        MenuViewStatusBar.Icon = new SymbolIcon(Symbol.View);
        MenuViewRotate.Text = "Rotate board";
        MenuViewRotate.Icon = new SymbolIcon(Symbol.Rotate);

        MenuChessPlayAgainstComputer.Text = "Play against computer";
        MenuChessPlayAgainstComputer.Icon = new SymbolIcon(Symbol.Play);

        MenuToolsShowChessCodes.Text = "Show chess codes";
        MenuToolsShowChessCodes.Icon = new SymbolIcon(Symbol.Copy);

        MenuHelpTopics.Text = "Help topics";
        MenuHelpTopics.Icon = new SymbolIcon(Symbol.Help);
        MenuHelpAbout.Text = "About";
        MenuHelpAbout.Icon = new SymbolIcon(Symbol.Help);

        //ExtendsContentIntoTitleBar = true;

        // InfoBadgeOpacity = 0.5;
    }

    private void MainWindow1_Activated(object sender, WindowActivatedEventArgs args)
    {

    }

    private void MainWindow1_SizeChanged(object sender, WindowSizeChangedEventArgs args)
    {
        args.Handled = true;
    }
}
