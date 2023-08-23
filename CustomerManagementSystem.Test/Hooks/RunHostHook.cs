using CustomerManagementSystem.Test.Tools;
using CustomerManagementSystem.Test.Tools.NetCoreHosting;
using TechTalk.SpecFlow;

namespace CustomerManagementSystem.Test.Hooks
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