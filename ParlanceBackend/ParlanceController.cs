using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ParlanceBackend
{
    public class ParlanceController : Controller
    {
        private readonly ParlanceConfiguration _parlanceConfiguration;

        public ParlanceController(IOptions<ParlanceConfiguration> parlanceConfiguration)
        {
            _parlanceConfiguration = parlanceConfiguration.Value;
        }
    }
}
