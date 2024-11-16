namespace Store.Web.Contractors
{
    public interface IWebContractorService
    {
        string UniqueCode { get; }

        Uri StartSession(IReadOnlyDictionary<string, object> parameters, Uri returnUri);
    }
}