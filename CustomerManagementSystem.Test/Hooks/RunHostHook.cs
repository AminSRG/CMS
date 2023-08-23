using Inventory.Tests.E2E.Tools;
using Inventory.Tests.E2E.Tools.NetCoreHosting;
using TechTalk.SpecFlow;

namespace Inventory.Tests.E2E.Hooks
{
    [Binding]
    public sealed class RunHostHook
    {
        private static readonly DotNetCoreHost Host =
            new DotNetCoreHost(new DotNetCoreHostOptions
            {
                Port = HostConstants.Port,
                CsProjectPath = HostConstants.CsProjectPath
            });

        [BeforeFeature]
        public static void StartHost()
        {
            Host.Start();
        }

        [AfterFeature]
        public static void ShutdownHost()
        {
            Host.Stop();
        }
    }
}