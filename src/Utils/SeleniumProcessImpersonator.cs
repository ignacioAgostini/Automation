using System;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace AutomationLibrary.Utils
{
    public class SeleniumProcessImpersonator : IDisposable
    {
        private const int MaxTriesWaitWebdriverStartup = 10;
        private Process _driverProcess = null;

        private bool _started;
        public bool Started => _started;

        public bool WaitForWebDriverStarted(int millisecondsToWait)
        {
            var retryCount = MaxTriesWaitWebdriverStartup;
            var milliSecondWaitInterval = Math.Min(200, Math.Max(1, millisecondsToWait / MaxTriesWaitWebdriverStartup));

            while (!_started && retryCount >= 0)
            {
                Thread.Sleep(milliSecondWaitInterval);
                if (millisecondsToWait < 1) { retryCount--; }
            }

            return _started;
        }

        public SeleniumProcessImpersonator(NetworkCredential credentials, string DriverPath)
        {
            var processStartInfo = new ProcessStartInfo(DriverPath)
            {
                UserName = credentials.UserName,
                Password = credentials.SecurePassword,
                Domain = credentials.Domain,
                UseShellExecute = false,
                LoadUserProfile = true,
                WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory
            };
            var startThread = new Thread(() =>
            {
                _driverProcess = Process.Start(processStartInfo);
                _started = true;
                _driverProcess.WaitForExit();
            })
            { IsBackground = true };
            startThread.Start();
        }

        public void Dispose()
        {
            if (_driverProcess == null) return;

            if (!_driverProcess.HasExited)
            {
                _driverProcess.CloseMainWindow();
                _driverProcess.WaitForExit(5000);
                if (!_driverProcess.HasExited)
                {
                    _driverProcess.Kill();
                }
                _driverProcess.Close();
            }

            _driverProcess.Dispose();
        }
    }
}
