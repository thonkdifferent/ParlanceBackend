namespace ParlanceBackend.Models
{
    public class SshKeyData
    {
        public string SshKeyContents { get; set; }
    }

    public class AcquireSshHostKeyData
    {
        public string Host { get; set; }
    }
}