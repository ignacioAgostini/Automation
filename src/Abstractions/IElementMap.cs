using OpenQA.Selenium;

namespace AutomationLibrary.Abstractions
{
    public interface IElementMap
    {
        IWebDriver Browser { get; }

        void SetBrowser(IDriver driver);
    }
}