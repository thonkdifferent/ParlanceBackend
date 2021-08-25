using System.Threading;
using System.Threading.Tasks;
using accounts.DBus;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Tmds.DBus;

namespace ParlanceBackend.Services
{
    public class AccountsService
    {
        private Connection _accountsConnection;
        private IManager _manager;
        private readonly IOptions<ParlanceConfiguration> _parlanceConfiguration;

        internal Connection Bus => _accountsConnection;
        internal IManager AccountsManager => _manager;

        public AccountsService(IOptions<ParlanceConfiguration> parlanceConfiguration)
        {
            _parlanceConfiguration = parlanceConfiguration;

#pragma warning disable 4014
            Initialise().Wait();
#pragma warning restore 4014
        }

        private async Task Initialise()
        {
            _accountsConnection = new Connection(_parlanceConfiguration.Value.AccountsBus);
            await _accountsConnection.ConnectAsync();
            
            _manager = _accountsConnection.CreateProxy<IManager>("com.vicr123.accounts", "/com/vicr123/accounts");
        }
    }
}