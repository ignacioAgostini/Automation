using AutomationLibrary.Utils;
using AutomationLibrary.Abstractions;
using OpenQA.Selenium;

namespace AutomationLibrary.PageObjects
{
    public abstract class ElementMap : IElementMap
    {
        protected Wait Wait { get; private set; }

        public IWebDriver Browser { get; private set; }

        public ElementMap()
        {
        }

        public void SetBrowser(IDriver driver)
        {
            Browser = driver.Browser;
            Wait = new Wait(driver.Browser);
        }
    }
}