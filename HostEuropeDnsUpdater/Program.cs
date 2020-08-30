using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.RegularExpressions;

using HostEuropeDnsUpdater.Config;

using Microsoft.Extensions.Configuration;

using NLog;

namespace HostEuropeDnsUpdater
{
    class Program
    {
        static int Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json");
                var configuration = builder.Build();
                var smtpSettings = configuration.GetSection(nameof(SmtpSettings)).Get<SmtpSettings>();
                var hosteuropeSettings = configuration.GetSection(nameof(HosteuropeSettings)).Get<HosteuropeSettings>();
                var domainSettings = configuration.GetSection(nameof(DomainSettings)).Get<DomainSettings>();
                var ipCheckUrl = configuration["IpCheckUrl"];

                try
                {
                    smtpSettings.Validate();
                    hosteuropeSettings.Validate();
                    domainSettings.Validate();
                }
                catch (ApplicationException e)
                {
                    logger.Error($"Configuration error: {e.Message}");
                    return -1;
                }

                var ipHelper = new IpHelper(ipCheckUrl, logger);
                var currentIp = ipHelper.GetCurrentIp();

                if (ipHelper.IsNewIp(currentIp))
                {
                    logger.Info($"Send DNS update for ip {currentIp}.");
                    var dnsUpdater = new DnsUpdater(smtpSettings, hosteuropeSettings, domainSettings);
                    dnsUpdater.UpdateDns(currentIp);
                    ipHelper.StoreCurrentIp(currentIp);
                }
                else
                {
                    logger.Info($"Current IP {currentIp} is not new, exit.");
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Unknown error");
                return -1;
            }

            return 0;
        }
    }
}
