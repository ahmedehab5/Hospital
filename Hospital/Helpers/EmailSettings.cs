using Hospital.Models;
using System.Net;
using System.Net.Mail;

namespace Hospital.Helpers
{
	public static class EmailSettings
	{
		public static void SendEmail(Email email)
		{
			var Client = new SmtpClient("smtp.gmail.com", 587);
			Client.EnableSsl = true;
			Client.Credentials = new NetworkCredential("nourtamer202@gmail.com", "nzzvnglpjrdkodnh");
			Client.Send("nourtamer202@gmail.com", email.To, email.Subject, email.Body);
		}
	}
}
