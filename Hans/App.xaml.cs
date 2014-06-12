﻿using Hans.General;
using Hans.Modules;
using Ninject;
using System;
using System.Windows;

namespace Hans
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App
    {
        private ExitAppTrigger _exitAppTrigger;
        private IKernel _kernel;

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
        /// On program start up
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            BuildKernel();
            BuildForm();
            Current.MainWindow.Show();
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
            MessageBox.Show(e.Exception.Message, e.Exception.Source);
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
        }
    }
}