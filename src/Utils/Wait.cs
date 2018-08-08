using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;

namespace AutomationLibrary.Utils
{
    public class Wait
    {
        private int timeout = 120;
        private int pollingInterval = 200;
        private IWebDriver driver;

        private WebDriverWait EstaticWait { get; set; }

        private DefaultWait<IWebDriver> FluentWait { get; set; }

        public Wait(IWebDriver driver)
        {
            this.driver = driver;
            EstaticWait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeout));
            FluentWait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(timeout),
                PollingInterval = TimeSpan.FromMilliseconds(pollingInterval)
            };
            FluentWait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            FluentWait.IgnoreExceptionTypes(typeof(StaleElementReferenceException));
        }

        public void ClickOnElement(IWebElement element)
        {
            EstaticWait.Until(d => !(SeleniumExtras.WaitHelpers.ExpectedConditions.StalenessOf(element)(d)));
            EstaticWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element)).Click();
        }
        
        public void ClickOnElement_Javascript(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", EstaticWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element)));
        }

        public void FillElement(IWebElement element, string input)
        {
            if (EstaticWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element)).Enabled)
            {
                element.Clear();
                element.SendKeys(input);
            }
            else
            {
                throw new Exception("Element was not visible and couldn't be selected.");
            }
        }

        public bool ElementPresence(IWebElement element)
        {
            return EstaticWait.Until(d => element.Displayed);
        }

        public bool ElementExists(IWebElement element)
        {
            try
            {
                EstaticWait.Until(d => d.FindElement(By.TagName(element.TagName)));
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }

        public void WaitForPageToLoad()
        {
            FluentWait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").ToString().Equals("complete"));
        }

        public void WatiForLoadScrean()
        {
            EstaticWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath(".//div[contains(@class, 'dx-loadindicator')]")));
        }

        public void WaitForElementToLoad(By locator)
        {
            FluentWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(locator));
        }

        public void FluentClick(IWebElement element)
        {
            FluentWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element)).Click();
        }

        public void FluentFill(IWebElement element, string input)
        {
            if (FluentWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element)).Enabled)
            {
                element.Clear();
                element.SendKeys(input);
            }
            else
            {
                throw new Exception("Element was not visible and couldn't be selected.");
            }
        }

        public void FluentClick_Javascript(IWebElement element)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].click();", FluentWait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element)));
        }

        public bool WaitUntilJSReady()
        {
            var jsExec = (IJavaScriptExecutor)driver;

            //Wait for Javascript to load
            bool jsLoad(IWebDriver drivers) => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").ToString().Equals("complete");

            //Get JS is Ready
            bool jsReady = (Boolean)jsExec.ExecuteScript("return document.readyState").ToString().Equals("complete");

            //Wait Javascript until it is Ready!
            if (!jsReady)
            {
                //Wait for Javascript to load
                jsReady = EstaticWait.Until(jsLoad);
            }

            return jsReady;
        }

        public IWebElement FluentWait_FindElement(By by)
        {
            return FluentWait.Until(x => x.FindElement(by));
        }

        public IEnumerable<IWebElement> FluentWait_FindElements(By by)
        {
            return FluentWait.Until(x => x.FindElements(by));
        }
    }
}
