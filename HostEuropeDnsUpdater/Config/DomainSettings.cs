using System;
using System.Collections.Generic;
using System.Linq;

namespace HostEuropeDnsUpdater.Config
{
    public class DomainSettings
    {
        public string Domain { get; set; }
        public string Tld { get; set; }

        public string DnsDefault { get; set; }
        public List<DnsEntry> DnsAEntries { get; set; }
        public List<DnsEntry> DnsCnameEntries { get; set; }
        public List<DnsEntry> DnsTxtEntries { get; set; }
        public List<DnsMxEntry> DnsMxEntries { get; set; }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(this.Domain))
            {
                throw new ApplicationException("Domain is missing!");
            }

            if (string.IsNullOrWhiteSpace(this.Tld))
            {
                throw new ApplicationException("Tld is missing!");
            }

            //Should validate A, MX Entries for "CURRENT_IP" or valid IpAddress
        }
    }
}