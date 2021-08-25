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

        public AccountsService(IOptions<ParlanceConfiguration> parlanceConfiguration, Connection accountsConnection)
        {
            _parlanceConfiguration = parlanceConfiguration;
            _accountsConnection = accountsConnection;

            _manager = _accountsConnection.CreateProxy<IManager>("com.vicr123.accounts", "/com/vicr123/accounts");
        }

    }
}