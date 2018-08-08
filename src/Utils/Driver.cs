using AutomationLibrary.Abstractions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using System;
using System.IO;

namespace AutomationLibrary.Utils.Driver
{
    public class Driver : IDriver
    {
        private string BinPath { get; set; }

        public IWebDriver Browser { get; private set; }
        
        public void NavigateToUrl(string url)
        {
            Browser.Navigate().GoToUrl(url);
        }

        public void StartBrowser(BrowserType browser, string[] driverOptions = null, string configurationPath = null)
        {
            switch (browser)
            {
                case BrowserType.Chrome:
                    RunChrome(configurationPath, driverOptions);
                    break;

                case BrowserType.Firefox:
                    RunFirefox(configurationPath, driverOptions);
                    break;

                case BrowserType.Edge:
                    RunEdge(configurationPath, driverOptions);
                    break;

                default:
                    throw new WrongParameterException($"Parameter Browser is wrong. It has to be 'Chrome' or 'Firefox' or 'Edge' and it was {browser.ToString()}");
            }
        }
        
        public void StopBrowser()
        {
            Browser.Quit();
        }

        private void SetBinPath(string confPath)
        {
            if (System.String.IsNullOrEmpty(confPath))
            {
                BinPath = Directory.GetCurrentDirectory();
            }
            else
            {
                BinPath = confPath;
            }
        }

        private void RunChrome(string configurationPath, string[] driverOptions)
        {
            var chromeOptions = new ChromeOptions();

            if (driverOptions != null && driverOptions.Length != 0)
            {
                chromeOptions.AddArguments(driverOptions);
                Browser = new RemoteWebDriver(new Uri("http://localhost:9515"), chromeOptions);
            }
            else
            {
                Browser = new RemoteWebDriver(new Uri("http://localhost:9515"), chromeOptions.ToCapabilities());
            }
        }

        private void RunFirefox(string configurationPath, string[] driverOptions)
        {
            SetBinPath(configurationPath);

            if (driverOptions != null && driverOptions.Length != 0)
            {
                var firefoxOptions = new FirefoxOptions();
                firefoxOptions.AddArguments(driverOptions);
                Browser = new FirefoxDriver(BinPath, firefoxOptions);
            }
            else
            {
                Browser = new FirefoxDriver(BinPath);
            }
        }

        private void RunEdge(string configurationPath, string[] driverOptions)
        {
            SetBinPath(configurationPath);

            Browser = new EdgeDriver(BinPath);
        }
    }
}

namespace AutomationLibrary
{
    public enum BrowserType
    {
        Chrome,
        Firefox,
        Edge
    }
}