using AsyncReactAwait.Promises;

namespace CCG.Services.Startup
{
    public interface IStartupService
    {
        IPromise Startup();
    }
}