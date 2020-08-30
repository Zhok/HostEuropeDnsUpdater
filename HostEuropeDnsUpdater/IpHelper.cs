using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

using NLog;

namespace HostEuropeDnsUpdater
{
    public class IpHelper
    {
        private readonly string ipCheckUrl;

        private readonly ILogger logger;

        private const string LAST_IP_PATH = "last_ip.txt";

        public IpHelper(string ipCheckUrl, ILogger logger)
        {
            this.ipCheckUrl = ipCheckUrl;
            this.logger = logger;
        }

        public bool IsNewIp(IPAddress currentIp)
        {
            if (File.Exists(IpHelper.LAST_IP_PATH))
            {
                var content = File.ReadAllText(IpHelper.LAST_IP_PATH);
                IPAddress lastIp;
                if (!IPAddress.TryParse(content, out lastIp))
                {
                    logger.Warn($"Can't read ip from {IpHelper.LAST_IP_PATH}, unknown if current ip is new.");
                    return false;
                }

                if (lastIp.ToString() == currentIp.ToString())
                {
                    return false;
                }
            }

            return true;
        }

        public void StoreCurrentIp(IPAddress currentIp)
        {
            try
            {
                File.WriteAllText(LAST_IP_PATH, currentIp.ToString());
            }
            catch (Exception e)
            {
                logger.Error(e, $"Can't store current ip address");
            }
        }

        public IPAddress GetCurrentIp()
        {
            string urlContent;
            using (var httpClient = new HttpClient())
            {
                using (var httpResonse = httpClient.GetAsync(this.ipCheckUrl).Result)
                {
                    urlContent = httpResonse.Content.ReadAsStringAsync().Result;
                }
            }
            
            var urlString = Regex.Replace(urlContent, @"\t|\n|\r", "");

            return IPAddress.Parse(urlString);
        }
    }
}