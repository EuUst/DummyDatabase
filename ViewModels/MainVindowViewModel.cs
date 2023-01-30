using DummyDatabase.Core.DataWork;
using DummyDatabase.Core.Models;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Data;

namespace DummyDatabase.Desktop
{
    public class MainVindowViewModel
    {
        private Database database;
        private DataWork dataWork;
        private DataGrid dataGrid;
        public MainVindowViewModel(string databasePath)
        {
            dataWork = new DataWork(new DesktopLoggerService(), new SchemaValidator());
            database = dataWork.CreateDatabaseByFile(databasePath);
        }

        public MainVindowViewModel(string databasePath, DataGrid datagrid)
        {
            dataWork = new DataWork(new DesktopLoggerService(), new SchemaValidator());
            database = dataWork.CreateDatabaseByFile(databasePath);
            dataGrid = datagrid;
        }

        public void VisualizeDbMetadata(TreeView treeView)
        {
            TreeViewItem databaseView = new TreeViewItem();
            databaseView.Header = database.Name;

            TreeViewItem tables = new TreeViewItem();
            tables.Header = "Tables";
            
            foreach(var table in database.Tables)
            {
                TreeViewItem tableView = new TreeViewItem();
                tableView.Header = table.Schema.Name;
                tableView.Selected += TableViewSelected;
                tableView.Unselected += TableViewUnselected;

                foreach(var column in table.Schema.Columns)
                {
                    tableView.Items.Add($"{column.Name} - {column.Type} - isPrimary: {column.IsPrimary}");
                }

                tables.Items.Add(tableView);
            }

            databaseView.Items.Add(tables);

            treeView.Items.Add(databaseView);
        }

        private void TableViewSelected(object sender, System.Windows.RoutedEventArgs e)
        {
            dataGrid.Columns.Clear();
            //dataGrid.Items.Clear();
            dataGrid.ItemsSource = null;
            string tableName = ((TreeViewItem)sender).Header.ToString();
            VisualizeDbData(dataGrid, tableName);
        }
        private void TableViewUnselected(object sender, System.Windows.RoutedEventArgs e)
        {
            dataGrid.Columns.Clear();
              
        }

        private void VisualizeDbData(DataGrid dataGrid, string tableName)
        {
            Table table = database.Tables.Where(x => tableName == x.Name).First();
            dataGrid.AutoGenerateColumns = false;
            /*foreach (Column column in table.Schema.Columns)
            {
                DataGridTextColumn textColumn = new DataGridTextColumn();
                textColumn.Width = DataGridLength.Auto;
                textColumn.Header = column.Name;
                dataGrid.Columns.Add(textColumn);
            }*/

            dataGrid.ItemsSource = AdaptAllRows(table.Rows);

            for(int i = 0; i < table.Schema.Columns.Count; i++)
            {
                dataGrid.Columns.Add(new DataGridTextColumn()
                {
                    Header = table.Schema.Columns[i].Name,
                    Binding = new Binding($"Data[{i}]")
                });
            }          
        }
        private List<RowAdapter> AdaptAllRows(List<Row> rows)
        {
            List<RowAdapter> resList = new List<RowAdapter>();
            foreach (var item in rows)
            {
                resList.Add(new RowAdapter());
                resList[resList.Count - 1].Data = new List<object>();
                foreach (var data in item.Data)
                {
                    resList[resList.Count - 1].Data.Add(data.Value);
                }
            }
            return resList;
        }

        private class RowAdapter
        {
            public List<object> Data { get; set; }
        }
    }
}
