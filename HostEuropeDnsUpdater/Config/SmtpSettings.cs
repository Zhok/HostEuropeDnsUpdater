using System;

namespace HostEuropeDnsUpdater.Config
{
    public class SmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool EnableSsl { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Server))
            {
                throw new ApplicationException("Smtp server is missing!");
            }

            if (this.Port == 0)
            {
                throw new ApplicationException("Smtp port is missing!");
            }
        }
    }
}