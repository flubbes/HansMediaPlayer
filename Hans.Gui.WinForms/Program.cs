using System;
using System.Windows.Forms;
using Hans.Core.General;
using Hans.Gui.WinForms.Modules;
using Ninject;

namespace Hans.Gui.WinForms
{
    static class Program
    {
        private static IKernel _kernel;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            BuildKernel();
            var form = _kernel.Get<FormMain>();
            Application.Run(form);
        }

        private static void BuildKernel()
        {
            _kernel = new StandardKernel();
            LoadModules();
            _kernel.Bind<ExitAppTrigger>().ToSelf().InSingletonScope();
        }

        private static void LoadModules()
        {
            _kernel.Load<DatabaseModule>();
            _kernel.Load<SongDataModule>();
            _kernel.Load<AudioModule>();
            _kernel.Load<FileSystemModule>();
            _kernel.Load<FormModule>();
        }
    }
}
