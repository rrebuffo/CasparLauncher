using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CasparLauncher.Properties;
using S = CasparLauncher.Properties.Settings;

namespace CasparLauncher
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        static Mutex Mutex;
        private EventWaitHandle eventWaitHandle;
        private string EventName = "bd8e7274f5de4f00b6793bea0928b584";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Mutex = new Mutex(true, "{4D1BD577-550D-40BF-A10C-255C4B2FA406}", out bool owned);
            eventWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset, EventName);
            GC.KeepAlive(Mutex);
            if (owned)
            {
                var thread = new Thread(() =>
                {
                    while (eventWaitHandle.WaitOne())
                    {
                        Current.Dispatcher.BeginInvoke((Action)(() => ((Launcher)Current.MainWindow).ActivateInstance()));
                    }
                });
                thread.IsBackground = true;
                thread.Start();
                SetLanguageDictionary();
                return;
            }
            eventWaitHandle.Set();
            Process.GetCurrentProcess().Kill();
        }

        public void SetLanguageDictionary()
        {
            try
            {
                switch ((Languages)S.Default.ForcedLanguage)
                {
                    case Languages.en:
                        CasparLauncher.Properties.Resources.Culture = new System.Globalization.CultureInfo("en-US");
                        break;
                    case Languages.es:
                        CasparLauncher.Properties.Resources.Culture = new System.Globalization.CultureInfo("es-ES");
                        break;
                    case Languages.none:
                    default:
                        switch (Thread.CurrentThread.CurrentCulture.ToString())
                        {
                            case "es-ES":
                            case "es-MX":
                            case "es-AR":
                                CasparLauncher.Properties.Resources.Culture = new System.Globalization.CultureInfo("es-ES");
                                break;
                            default:
                                CasparLauncher.Properties.Resources.Culture = new System.Globalization.CultureInfo("en-US");
                                break;
                        }
                        break;
                }
            }
            catch(Exception)
            {
                CasparLauncher.Properties.Resources.Culture = new System.Globalization.CultureInfo("es-ES");
            }
        }

        private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            Current.Dispatcher.BeginInvoke((Action)(() => ((Launcher)Current.MainWindow).Shutdown()));
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var comException = e.Exception as System.Runtime.InteropServices.COMException;

            if (comException != null && comException.ErrorCode == -2147221040) e.Handled = true;
        }
    }

    public enum Languages
    {
        [Description("Ninguno")]
        none,

        [Description("English")]
        en,

        [Description("Español")]
        es
    }
}
