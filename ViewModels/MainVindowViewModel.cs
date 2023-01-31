using DummyDatabase.Core.DataWork;
using DummyDatabase.Core.Models;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Data;

namespace DummyDatabase.Desktop
{
    public class MainVindowViewModel
    {
        private Database currentDatabase;
        private DataWork dataWork;
        private DataGrid dataGrid;
        public MainVindowViewModel(string databasePath, DataGrid datagrid)
        {
            dataWork = new DataWork(new DesktopLoggerService(), new SchemaValidator());
            currentDatabase = dataWork.CreateDatabaseByFile(databasePath);
            dataGrid = datagrid;
        }

        public void VisualizeDbMetadata(TreeView treeView)
        {
            TreeViewItem databaseView = new TreeViewItem();
            databaseView.Header = currentDatabase.Name;

            TreeViewItem tables = new TreeViewItem();
            tables.Header = "Tables";
            
            foreach(var table in currentDatabase.Tables)
            {
                TreeViewItem tableView = new TreeViewItem();
                tableView.Header = table.Schema.Name;
                tableView.Selected += TableViewSelected;
                tableView.Unselected += TableViewUnselected;

                foreach(var column in table.Schema.Columns)
                {
                    tableView.Items.Add($"{column.Name} - {column.Type} - isPrimary: {column.IsPrimary} - isForeignKey: {column.IsForeignKey}");
                }

                tables.Items.Add(tableView);
            }

            databaseView.Items.Add(tables);

            treeView.Items.Add(databaseView);
        }

        private void TableViewSelected(object sender, System.Windows.RoutedEventArgs e)
        {
            dataGrid.Columns.Clear();
            string tableName = ((TreeViewItem)sender).Header.ToString();
            VisualizeDbData(dataGrid, tableName);
        }
        private void TableViewUnselected(object sender, System.Windows.RoutedEventArgs e)
        {
            dataGrid.Columns.Clear();          
        }

        private void VisualizeDbData(DataGrid dataGrid, string tableName)
        {
            Table table = currentDatabase.Tables.Where(x => tableName == x.Name).First();
            dataGrid.AutoGenerateColumns = false;

            dataGrid.ItemsSource = table.GetRowsData();

            for(int i = 0; i < table.Schema.Columns.Count; i++)
            {
                dataGrid.Columns.Add(new DataGridTextColumn()
                {
                    Header = table.Schema.Columns[i].Name,
                    Binding = new Binding($"Data[{i}]")
                });
            }          
        }
    }
}
