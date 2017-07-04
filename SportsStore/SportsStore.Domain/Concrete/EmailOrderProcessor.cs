using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SportsStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "183900112@qq.com";
        public string MailFromAddress = "liaojin_work@163.com";
        public bool UseSsl = false;
        public string UserName = "liaojin_work";
        public string Password = "授权码";
        public string ServerName = " smtp.163.com";
        public int ServerPort = 25;
        public bool WriteAsFile = false;
        public string FileLocation = @"c:\sports_store_emails";
    }
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;
        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }
        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.UserName, emailSettings.Password);
                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }
                StringBuilder body = new StringBuilder()
                .AppendLine("A new order has been submitted")
                .AppendLine("---")
                .AppendLine("Items:");
                foreach (var item in cart.Lines)
                {
                    var subTotal = item.Product.Price * item.Quantity;
                    body.AppendFormat("{0}*{1}(subtotal:{2:c})", item.Quantity, item.Product.Name, subTotal);
                }
                body.AppendFormat("Total order value:{0:c}", cart.ComputeTotalValue())
                    .AppendLine("---")
                    .AppendLine("Ship to:")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Line1)
                    .AppendLine(shippingDetails.Line2 ?? "")
                    .AppendLine(shippingDetails.Line3 ?? "")
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.State ?? "")
                    .AppendLine(shippingDetails.Country)
                    .AppendLine(shippingDetails.Zip)
                    .AppendLine("---")
                    .AppendFormat("Gift wrap:{0}", shippingDetails.GiftWrap ? "Yes" : "No");
                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress,//from
                    emailSettings.MailToAddress,//to
                    "sssssss!",//Subject
                   "gedefgsdfds"//body.ToString()//body
                    );
                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
