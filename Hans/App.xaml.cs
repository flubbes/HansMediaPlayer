using System.Windows;
using Hans.General;
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
            _exitAppTrigger.Trigger();
            base.OnExit(e);
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
        /// Binds all triggers
        /// </summary>
        private void BindTriggers()
        {
            _kernel.Bind<ExitAppTrigger>().ToMethod<ExitAppTrigger>(a => _exitAppTrigger);
        }

        /// <summary>
        /// Loads all the modules
        /// </summary>
        private void LoadModules()
        {
            _kernel.Load<DatabaseModule>();
            _kernel.Load<AudioModule>();
            _kernel.Load<SongDataModule>();
        }


        /// <summary>
        /// Initializes the triggers
        /// </summary>
        private void InitializeTriggers()
        {
            _exitAppTrigger = new ExitAppTrigger();
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
