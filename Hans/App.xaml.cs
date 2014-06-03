using System.Collections;
using System.Collections.Generic;
using System.Windows;
using Hans.Database;
using Hans.General;
using Hans.GeneralApp;
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
        private ExitTrigger _exitTrigger;

        /// <summary>
        /// On program start up
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            BuildKernel();
            BuildForm();
            Current.MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _exitTrigger.TriggerExit();
        }

        /// <summary>
        /// Builds the kernel
        /// </summary>
        private void BuildKernel()
        {
            _exitTrigger = new ExitTrigger();
            _kernel = new StandardKernel();
            _kernel.Bind<ExitTrigger>().ToMethod<ExitTrigger>(a => _exitTrigger);
            _kernel.Load<DatabaseModule>();
            
        }

        /// <summary>
        /// builds the form
        /// </summary>
        private void BuildForm()
        {
            Current.MainWindow = _kernel.Get<MainWindow>();
            Current.MainWindow.Title = "HansAudioPlayer";
        }
    }
}
