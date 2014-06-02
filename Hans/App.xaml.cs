using System.Windows;
using Hans.Database;
using Hans.General;
using Hans.Library;
using Hans.Modules;
using Ninject;

namespace Hans
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App
    {
        private IKernel _kernel;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            BuildKernel();
            BuildForm();
            Current.MainWindow.Show();
        }

        private void BuildKernel()
        {
            _kernel = new StandardKernel();
            _kernel.Load<DatabaseModule>();
           
        }

        private void BuildForm()
        {
            Current.MainWindow = _kernel.Get<MainWindow>();
            Current.MainWindow.Title = "HansAudioPlayer";
        }
    }
}
