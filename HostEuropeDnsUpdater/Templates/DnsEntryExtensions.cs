using System;
using System.Net;

using HostEuropeDnsUpdater.Config;

namespace HostEuropeDnsUpdater.Templates
{
    public static class DnsEntryExtensions
    {
        public static string AsMailEntry(this DnsEntry entry, IPAddress currentIp, AutonsType type)
        {
            var value = entry.Value == Consts.CURRENT_IP ? currentIp.ToString() : entry.Value;

            switch (type)
            {
                case AutonsType.A:
                    return $"{Environment.NewLine}9b. autons-a...................: {entry.Prefix} {value}";
                case AutonsType.Cname:
                    return $"{Environment.NewLine}9d. autons-cname...............: {entry.Prefix} {value}";
                case AutonsType.Txt:
                    return $"{Environment.NewLine}9f. autons-txt.................: {entry.Prefix} {value}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static string AsMailEntry(this DnsMxEntry entry, IPAddress currentIp)
        {
            var value = entry.Value == Consts.CURRENT_IP ? currentIp.ToString() : entry.Value;
            return $"{Environment.NewLine}9c. autons-mx..................: {entry.Prefix} {value} {entry.Priority}";
        }
    }
}