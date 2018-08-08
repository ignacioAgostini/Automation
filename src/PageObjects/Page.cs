using AutomationLibrary.Utils;
using AutomationLibrary.Abstractions;

namespace AutomationLibrary.PageObjects
{
    public abstract class Page<TMap> : IPage<TMap>
        where TMap : IElementMap, new()
    {
        public string Url { get; }
        public IDriver Driver { get; }

        protected Wait Wait { get; }

        public Page(IDriver driver)
        {
            Url = "";
            Driver = driver;
            Wait = new Wait(driver.Browser);
        }

        public Page(string url, IDriver driver)
        {
            Url = url;
            Driver = driver;
            Wait = new Wait(driver.Browser);
        }

        public TMap Map
        {
            get
            {
                var _map = new TMap();
                _map.SetBrowser(Driver);
                return _map;
            }
        }

        public void Navigate()
        {
            Driver.NavigateToUrl(Url);
        }
    }

    public abstract class Page<TMap, TValidator> : Page<TMap>, IPage<TMap, TValidator>
        where TMap : IElementMap, new()
        where TValidator : IPageValidator<TMap>, new()
    {
        public Page(IDriver driver)
            : base(driver)
        { }

        public Page(string url, IDriver driver)
            : base(url, driver)
        { }

        public TValidator Validate()
        {
            var validator = new TValidator();
            validator.SetBrowser(Driver);
            return validator;
        }
    }
}