using System.Threading.Tasks;
using Impostor.Api.Plugins;
using Microsoft.Extensions.Logging;

namespace Boot.Codes
{
    [ImpostorPlugin(
            "at.duikbo.codes",
            "BootCodes",
            "miniduikboot",
            "0.1")]
    public class BootCodesPlugin : PluginBase
    {
        readonly ILogger<BootCodesPlugin> logger;

        public BootCodesPlugin(ILogger<BootCodesPlugin> logger)
        {
            this.logger = logger;
        }

        public override ValueTask EnableAsync()
        {
            logger.LogInformation("Boot.Codes initializing");
            return default;
        }

        public override ValueTask DisableAsync()
        {
            return default;
        }
    }
}
