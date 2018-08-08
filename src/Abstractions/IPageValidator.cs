namespace AutomationLibrary.Abstractions
{
    public interface IPageValidator<TMap>
        where TMap : IElementMap, new()
    {
        TMap Map { get; }

        void SetBrowser(IDriver driver);
    }
}