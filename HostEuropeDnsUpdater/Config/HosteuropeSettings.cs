using System;

namespace HostEuropeDnsUpdater.Config
{
    public class HosteuropeSettings
    {
        public string CustomerEmail { get; set; }
        public string CustomerNumber { get; set; }
        public string DrBoppAddress { get; set; }
        public string Password { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.CustomerEmail))
            {
                throw new ApplicationException("Customer email is missing!");
            }

            if (string.IsNullOrWhiteSpace(this.CustomerNumber))
            {
                throw new ApplicationException("Customer number is missing!");
            }

            if (string.IsNullOrWhiteSpace(this.DrBoppAddress))
            {
                throw new ApplicationException("Dr.Bopp email is missing!");
            }
        }
    }
}