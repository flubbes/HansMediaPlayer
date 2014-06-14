using CsQuery.Utility;
using Hans.General;
using Hans.Modules;
using Newtonsoft.Json;
using Ninject;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Hans
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : IDisposable
    {
        private ExitAppTrigger _exitAppTrigger;
        private IKernel _kernel;

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool cleanAll)
        {
            if (cleanAll)
            {
            }
            _kernel.Dispose();
        }

        /// <summary>
        /// On program start up
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            HookExceptions();
            BuildKernel();
            BuildForm();
            Current.MainWindow.Show();
        }

        private static void HandleError(string jsonException)
        {
            var now = DateTime.Now;
            var fileName = string.Format("error_{0}-{1}-{2}_{3}-{4}-{5}-{6}", now.Year, now.Month, now.Day, now.Hour, now.Minute,
                now.Second, now.Millisecond);
            using (var sw = new StreamWriter(fileName))
            {
                sw.Write(jsonException);
            }
        }

        /// <summary>
        /// Binds all triggers
        /// </summary>
        private void BindTriggers()
        {
            _kernel.Bind<ExitAppTrigger>().ToMethod<ExitAppTrigger>(a => _exitAppTrigger);
        }

        /// <summary>
        /// builds the form
        /// </summary>
        private void BuildForm()
        {
            Current.MainWindow = _kernel.Get<MainWindow>();
            Current.MainWindow.Title = "HansAudioPlayer";
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

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            var jsonException = JsonConvert.SerializeObject(e.Exception, Formatting.Indented);
            HandleError(jsonException);
        }

        private void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            var jsonException = JsonConvert.SerializeObject(e.Exception, Formatting.Indented);
            HandleError(jsonException);
        }

        /// <summary>
        /// Get triggered when an unhandled exception occures
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var jsonException = JsonConvert.SerializeObject(e.ExceptionObject, Formatting.Indented);
            HandleError(jsonException);
        }

        private void HookExceptions()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        /// <summary>
        /// Initializes the triggers
        /// </summary>
        private void InitializeTriggers()
        {
            _exitAppTrigger = new ExitAppTrigger();
        }

        /// <summary>
        /// Loads all the modules
        /// </summary>
        private void LoadModules()
        {
            _kernel.Load<DatabaseModule>();
            _kernel.Load<AudioModule>();
            _kernel.Load<SongDataModule>();
            _kernel.Load<FileSystemModule>();
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            e.SetObserved();
            var jsonException = JsonConvert.SerializeObject(e.Exception, Formatting.Indented);
            HandleError(jsonException);
        }
    }
}