using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace MyBusinessCardSite.Models
{
    public class ContactModel
    {
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please enter your email address")]
        [RegularExpression(@".+\@.+\..+", ErrorMessage = "Please enter a valid email")]
        public string Email { get; set; }
        public string Title { get; set; }
        [Required(ErrorMessage = "Please enter some text")]
        public string Text { get; set; }

        public bool SendMessage()
        {
            int steps = 0;
            while (steps < 10) try
            {
                var message = new MailMessage();
                message.To.Add(new MailAddress("Anovi-soft@hotmail.com"));  
                message.From = new MailAddress("L0dom@mail.ru");  
                message.Subject = $"AnoviUserEmail - {Title ?? "Unknown"}";
                message.Body = $"<p>Name: {Name ?? "Unknown"}</p>" +
                               $"<p>Phone: {Phone ?? "Unknown"}</p>" +
                               $"<p>Email: {Email ?? "Unknown"}</p>" +
                               $"<br/>" +
                               $"<p>Text:</p>" +
                               $"<dir>{Text ?? "EmptyBody"}</dir>";
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "L0dom@mail.ru", 
                        Password = "1@3$qWeR"  
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.mail.ru";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                }
                return true;
            }
            catch (Exception)
            {
                steps++;
            }
                return false;
        }
    }
}
