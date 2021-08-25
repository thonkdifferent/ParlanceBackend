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

            initialiseConnection().Wait();
        }

        async Task initialiseConnection()
        {
            _accountsConnection = new Connection(_parlanceConfiguration.Value.AccountsBus);
            _accountsConnection.StateChanged += async (sender, args) =>
            {
                if (args.State == ConnectionState.Disconnected)
                {
                    //Attempt to reconnect
                    await initialiseConnection();
                }
            };
            await _accountsConnection.ConnectAsync();
            _manager = _accountsConnection.CreateProxy<IManager>("com.vicr123.accounts", "/com/vicr123/accounts");
        }

    }
}