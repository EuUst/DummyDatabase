using System.Windows;
using DummyDatabase.Core.DataWork;

namespace DummyDatabase.Desktop
{
    public class DesktopLoggerService : ILoggerService
    {
        public void DebugError(string message)
        {
            MessageBox.Show(message);
        }
    }
}
