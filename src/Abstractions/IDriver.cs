using OpenQA.Selenium;

namespace AutomationLibrary.Abstractions
{
    public interface IDriver
    {
        IWebDriver Browser { get; }

        void NavigateToUrl(string url);

        void StartBrowser(BrowserType browser, string[] driverOptions = null, string configuration = null);

        void StopBrowser();
    }
}