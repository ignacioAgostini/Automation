using AutomationLibrary.Utils;
using AutomationLibrary.Abstractions;

namespace AutomationLibrary.PageObjects
{
    public abstract class PageValidator<TMap> : IPageValidator<TMap>
        where TMap : IElementMap, new()
    {
        private IDriver Driver;
        protected Wait Wait { get; private set; }
        
        public TMap Map
        {
            get
            {
                TMap map = new TMap();
                map.SetBrowser(Driver);
                return map;
            }
        }

        public void SetBrowser(IDriver driver)
        {
            Driver = driver;
            Wait = new Wait(driver.Browser);
        }
    }
}