namespace DemoBackendArchitecture.Application.Common.Model.Email
{
    public class MailSettings
    {
        public MailSettings()
        {
            
        }
        public MailSettings(string mail, string displayName, string password, string host, int port)
        {
            Mail = mail;
            DisplayName = displayName;
            Password = password;
            Host = host;
            Port = port;
        }

        public string Mail { get; init; }
        public string DisplayName { get; init; }
        public string Password { get; init; }
        public string Host { get; init;}
        public int Port { get; init; }
    }
}

