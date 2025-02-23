using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NeuroSky.ThinkGear;

namespace MindWaveReaderWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Boilerplate summary OnStartup method
        /// </summary>
        /// <param name="e">StartupEventArgs e</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            new WindowMain().Show();
        }

        /// <summary>
        /// Boilerplate summary OnExit method
        /// </summary>
        /// <param name="e">ExitEventArgs e</param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
