using System.Collections;
using System.Collections.Generic;
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
        private ExitAppTrigger _exitAppTrigger;
        private StartUpTrigger _startUpTrigger;

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

        /// <summary>
        /// Gets triggered when the app exits
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            _exitAppTrigger.Trigger();
        }

        /// <summary>
        /// Builds the kernel
        /// </summary>
        private void BuildKernel()
        {
            InitializeTriggers();
            _kernel = new StandardKernel();
            BindTriggers();
            LoadModules();
            
        }

        /// <summary>
        /// Loads all the modules
        /// </summary>
        private void LoadModules()
        {
            _kernel.Load<DatabaseModule>();
        }

        /// <summary>
        /// Binds all triggers
        /// </summary>
        private void BindTriggers()
        {
            _kernel.Bind<ExitAppTrigger>().ToMethod<ExitAppTrigger>(a => _exitAppTrigger);
            _kernel.Bind<StartUpTrigger>().ToMethod<StartUpTrigger>(a => _startUpTrigger);
        }


        /// <summary>
        /// Initializes the triggers
        /// </summary>
        private void InitializeTriggers()
        {
            _exitAppTrigger = new ExitAppTrigger();
            _startUpTrigger = new StartUpTrigger();
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
