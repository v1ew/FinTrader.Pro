namespace FinTrader.Pro.Iss.Requests
{
    public abstract class RequestBase
    {
        protected readonly IIssClient IssClient;
        
        protected RequestBase(IIssClient client)
        {
            IssClient = client;
        }
    }
}