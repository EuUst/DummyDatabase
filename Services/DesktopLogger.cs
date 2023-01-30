using System.Windows;
using DummyDatabase.Core.DataWork;

namespace DummyDatabase
{
    public class DesktopLogger : ILogger
    {
        public void DebugError(string message)
        {
            MessageBox.Show(message);
        }
    }
}
