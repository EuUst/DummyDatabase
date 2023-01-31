using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DummyDatabase.Desktop;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();                
    }
    private void OpenDatabase_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new CommonOpenFileDialog();
        dialog.IsFolderPicker = true;
 
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            string folder = dialog.FileName;

            MainVindowViewModel viewModel = new MainVindowViewModel(folder, databaseGrid);

            viewModel.VisualizeDbMetadata(databaseMetadata);
        }
    }
}
