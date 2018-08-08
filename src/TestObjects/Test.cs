using AutomationLibrary.Utils;
using AutomationLibrary.Utils.Driver;
using AutomationLibrary.Abstractions;
using System;
using System.Diagnostics;
using System.Net;
using System.Linq;

namespace AutomationLibrary.TestObjects
{
    public abstract class BaseTest : ITest, IDisposable
    {
        public SeleniumProcessImpersonator Impersonator { get; set; }

        public IDriver Driver { get; private set; }

        public BaseTest()
        {
            Driver = new Driver();

            Driver.StartBrowser(BrowserType.Chrome);
        }

        public BaseTest(BrowserType browser)
        {
            Driver = new Driver();

            Driver.StartBrowser(browser);
        }

        public BaseTest(BrowserType browser, string configurationPath, NetworkCredential networkCredential)
        {
            Impersonator = new SeleniumProcessImpersonator(networkCredential, System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GetProcessNameByBrwserType(browser)).ToString());

            Driver = new Driver();

            if (Impersonator.WaitForWebDriverStarted(3000))
            {
                Driver.StartBrowser(browser, null, configurationPath);
            }
        }

        public BaseTest(BrowserType browser, string[] driverOptions, NetworkCredential networkCredential)
        {
            Impersonator = new SeleniumProcessImpersonator(networkCredential, System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GetProcessNameByBrwserType(browser)).ToString());

            Driver = new Driver();

            if (Impersonator.WaitForWebDriverStarted(3000))
            {
                Driver.StartBrowser(browser, driverOptions);
            }
        }

        public BaseTest(BrowserType browser, string configurationPath, string[] driverOptions)
        {
            Driver = new Driver();

            Driver.StartBrowser(browser, driverOptions, configurationPath);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Driver.StopBrowser();
                    KillOrphanedInternetDrivers();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~BaseTest() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        private void KillOrphanedInternetDrivers()
        {
            var processes = Process.GetProcessesByName("chromedriver").ToList();
            processes.ForEach(p => p.Kill());
        }

        private string GetProcessNameByBrwserType(BrowserType browserType)
        {
            var processName = "";

            switch (browserType)
            {
                case BrowserType.Chrome:
                    processName = "chromedriver.exe";
                    break;

                case BrowserType.Edge:
                    processName = "";
                    break;

                case BrowserType.Firefox:
                    processName = "geckodriver.exe";
                    break;
            }

            return processName;
        }
    }
}