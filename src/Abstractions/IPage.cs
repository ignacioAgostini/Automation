namespace AutomationLibrary.Abstractions
{
    public interface IPage <TMap> 
        where TMap : IElementMap, new()
    {
        string Url { get; }

        IDriver Driver { get; }

        TMap Map { get; }

        void Navigate();
    }

    public interface IPage <TMap, TValidator> : IPage <TMap> 
        where TMap : IElementMap, new()
        where TValidator : IPageValidator<TMap>, new()
    {
        TValidator Validate();
    }
}