using System;
using System.Net;
using System.Net.Mail;

using HostEuropeDnsUpdater.Config;
using HostEuropeDnsUpdater.Templates;

namespace HostEuropeDnsUpdater
{
    public class DnsUpdater
    {

        private readonly SmtpSettings smtpSettings;

        private readonly HosteuropeSettings hosteuropeSettings;

        private readonly DomainSettings domainSettings;

        public DnsUpdater(SmtpSettings smtpSettings, HosteuropeSettings hosteuropeSettings, DomainSettings domainSettings)
        {
            this.smtpSettings = smtpSettings;
            this.hosteuropeSettings = hosteuropeSettings;
            this.domainSettings = domainSettings;
        }

        public void UpdateDns(IPAddress currentIp)
        {
            var mailContent = this.GetMailHeader();

            if (!string.IsNullOrEmpty(this.domainSettings.DnsDefault))
            {
                if (this.domainSettings.DnsDefault == Consts.CURRENT_IP)
                {
                    this.domainSettings.DnsDefault = currentIp.ToString();
                }
                mailContent += $"{Environment.NewLine}9a. autons-default.............: {this.domainSettings.DnsDefault}";
            }

            foreach (var entry in this.domainSettings.DnsAEntries)
            {
                mailContent += entry.AsMailEntry(currentIp, AutonsType.A);
            }

            foreach (var entry in this.domainSettings.DnsMxEntries)
            {
                mailContent += entry.AsMailEntry(currentIp);
            }

            foreach (var entry in this.domainSettings.DnsCnameEntries)
            {
                mailContent += entry.AsMailEntry(currentIp, AutonsType.Cname);
            }

            foreach (var entry in this.domainSettings.DnsTxtEntries)
            {
                mailContent += entry.AsMailEntry(currentIp, AutonsType.Txt);
            }

            this.SendUpdateMail(mailContent);
        }

        private void SendUpdateMail(string mailContent)
        {
            using (var smtpClient = new SmtpClient(this.smtpSettings.Server, this.smtpSettings.Port))
            {
                if (!string.IsNullOrWhiteSpace(this.smtpSettings.User))
                {
                    smtpClient.Credentials = new NetworkCredential(this.smtpSettings.User, this.smtpSettings.Password);
                    smtpClient.EnableSsl = this.smtpSettings.EnableSsl;
                }

                var message = new MailMessage();
                message.From = new MailAddress(this.hosteuropeSettings.CustomerEmail);
                message.To.Add(this.hosteuropeSettings.DrBoppAddress);
                message.IsBodyHtml = false;
                message.Body = mailContent;

                smtpClient.Send(message);
            }
        }

        private string GetMailHeader()
        {
            var template = MailTemplates.UpdateDnsTemplateHeader;
            template = template.Replace(Consts.CUSTOMER_NUMBER, this.hosteuropeSettings.CustomerNumber);
            template = template.Replace(Consts.CUSTOMER_EMAIL, this.hosteuropeSettings.CustomerEmail);
            template = template.Replace(Consts.DOMAIN, this.domainSettings.Domain);
            template = template.Replace(Consts.TLD, this.domainSettings.Tld);

            if (string.IsNullOrWhiteSpace(this.hosteuropeSettings.Password))
            {
                template = template.Replace(Consts.PASSWORD, "");
            }
            else
            {
                template = template.Replace(Consts.PASSWORD, 
                    MailTemplates.UpdateDnsTemplatePassword.
                        Replace(Consts.PASSWORD, this.hosteuropeSettings.Password));

            }

            return template;
        }
    }
}