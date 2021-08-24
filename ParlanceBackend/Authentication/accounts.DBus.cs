using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Tmds.DBus;

[assembly: InternalsVisibleTo(Tmds.DBus.Connection.DynamicAssemblyName)]
namespace accounts.DBus
{
    [DBusInterface("com.vicr123.accounts.Manager")]
    interface IManager : IDBusObject
    {
        Task<ObjectPath> CreateUserAsync(string Username, string Password, string Email);
        Task<ObjectPath> UserByIdAsync(ulong Id);
        Task<ulong> UserIdByUsernameAsync(string Username);
        Task<string> ProvisionTokenAsync(string Username, string Password, string Application, IDictionary<string, object> ExtraOptions);
        Task<ObjectPath> UserForTokenAsync(string Token);
        Task<ulong[]> AllUsersAsync();
    }

    [DBusInterface("com.vicr123.accounts.PasswordReset")]
    interface IPasswordReset : IDBusObject
    {
        Task<(string, IDictionary<string, object>)[]> ResetMethodsAsync();
        Task ResetPasswordAsync(string Type, IDictionary<string, object> Challenge);
    }

    [DBusInterface("com.vicr123.accounts.TwoFactor")]
    interface ITwoFactor : IDBusObject
    {
        Task<string> GenerateTwoFactorKeyAsync();
        Task EnableTwoFactorAuthenticationAsync(string OtpKey);
        Task DisableTwoFactorAuthenticationAsync();
        Task RegenerateBackupKeysAsync();
        Task<IDisposable> WatchTwoFactorEnabledChangedAsync(Action<bool> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchSecretKeyChangedAsync(Action<string> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchBackupKeysChangedAsync(Action<(string, bool)[]> handler, Action<Exception> onError = null);
        Task<T> GetAsync<T>(string prop);
        Task<TwoFactorProperties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class TwoFactorProperties
    {
        private bool _TwoFactorEnabled = default(bool);
        public bool TwoFactorEnabled
        {
            get
            {
                return _TwoFactorEnabled;
            }

            set
            {
                _TwoFactorEnabled = (value);
            }
        }

        private string _SecretKey = default(string);
        public string SecretKey
        {
            get
            {
                return _SecretKey;
            }

            set
            {
                _SecretKey = (value);
            }
        }

        private (string, bool)[] _BackupKeys = default((string, bool)[]);
        public (string, bool)[] BackupKeys
        {
            get
            {
                return _BackupKeys;
            }

            set
            {
                _BackupKeys = (value);
            }
        }
    }

    static class TwoFactorExtensions
    {
        public static Task<bool> GetTwoFactorEnabledAsync(this ITwoFactor o) => o.GetAsync<bool>("TwoFactorEnabled");
        public static Task<string> GetSecretKeyAsync(this ITwoFactor o) => o.GetAsync<string>("SecretKey");
        public static Task<(string, bool)[]> GetBackupKeysAsync(this ITwoFactor o) => o.GetAsync<(string, bool)[]>("BackupKeys");
    }

    [DBusInterface("com.vicr123.accounts.User")]
    interface IUser : IDBusObject
    {
        Task SetUsernameAsync(string Username);
        Task SetPasswordAsync(string Password);
        Task SetEmailAsync(string Email);
        Task<IDisposable> WatchUsernameChangedAsync(Action<(string oldUsername, string newUsername)> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchEmailChangedAsync(Action<string> handler, Action<Exception> onError = null);
        Task<IDisposable> WatchVerifiedChangedAsync(Action<bool> handler, Action<Exception> onError = null);
        Task<T> GetAsync<T>(string prop);
        Task<UserProperties> GetAllAsync();
        Task SetAsync(string prop, object val);
        Task<IDisposable> WatchPropertiesAsync(Action<PropertyChanges> handler);
    }

    [Dictionary]
    class UserProperties
    {
        private ulong _Id = default(ulong);
        public ulong Id
        {
            get
            {
                return _Id;
            }

            set
            {
                _Id = (value);
            }
        }

        private string _Username = default(string);
        public string Username
        {
            get
            {
                return _Username;
            }

            set
            {
                _Username = (value);
            }
        }

        private string _Email = default(string);
        public string Email
        {
            get
            {
                return _Email;
            }

            set
            {
                _Email = (value);
            }
        }

        private bool _Verified = default(bool);
        public bool Verified
        {
            get
            {
                return _Verified;
            }

            set
            {
                _Verified = (value);
            }
        }
    }

    static class UserExtensions
    {
        public static Task<ulong> GetIdAsync(this IUser o) => o.GetAsync<ulong>("Id");
        public static Task<string> GetUsernameAsync(this IUser o) => o.GetAsync<string>("Username");
        public static Task<string> GetEmailAsync(this IUser o) => o.GetAsync<string>("Email");
        public static Task<bool> GetVerifiedAsync(this IUser o) => o.GetAsync<bool>("Verified");
    }
}