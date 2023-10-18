using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using BookingRoomHotel.Models.ModelsInterface;

namespace BookingRoomHotel.Models
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void SendRegisterMail(string recip, string name, string id, string pw)
        {
            string message = string.Format("<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    " +
                    "<meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    " +
                    "<title>Registor Successful! Booking Room Hotel, KIOT!</title>\r\n</head>\r\n<body>\r\n    <h2>Registor Successful!!</h2>\r\n    " +
                    "<p>Hi <strong>{0},</strong></p>\r\n    " +
                    "<p>Congratulations on your successful registration!</p>\r\n    " +
                    "<p>Your ID:<strong> {1}</strong></p>\r\n    " +
                    "<p>Your Password:<strong> {2}</strong></p>\r\n    " +
                    "<p>Thank you for using our service!</p>\r\n    <p>Best Regards,</p>\r\n    " +
                    "<p style=\"font-style: italic; font-weight: bold;\">KIOT Team</p>\r\n</body>\r\n</html>\r\n", name, id, pw);
            try
            {
                checkEmailValid(recip);
                SendMail("Registor Successful! Booking Room Hotel, KIOT!", recip, message);
            }
            catch (Exception ex)
            {
                throw new Exception("Email could not be sent. " + ex.Message);
            }
        }
        public void SendConfirmQ(string recip, string name, string title)
        {
            string message = string.Format("<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    " +
                    "<meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    " +
                    "<title>Send Request Successful! Booking Room Hotel, KIOT!</title>\r\n</head>\r\n<body>\r\n    <h2>Send Request Successful!!</h2>\r\n    " +
                    "<p>Hi <strong>{0},</strong></p>\r\n    " +
                    "<p>Your request: <span style=\"color:red; font-style: italic;\">{1}</span> is being processed. We will respond to you within 24 hours!</p>\r\n    " +
                    "<p>Thank you for using our service!</p>\r\n    <p>Best Regards,</p>\r\n    " +
                    "<p style=\"font-style: italic; font-weight: bold;\">KIOT Team</p>\r\n</body>\r\n</html>\r\n", name, title);
            try
            {
                checkEmailValid(recip);
                SendMail("Send Request Successful! Booking Room Hotel, KIOT!", recip, message);
            }
            catch (Exception ex)
            {
                throw new Exception("Email could not be sent. " + ex.Message);
            }
        }
        
        public void SendResponseQ(string recip, string name, string title, string messageQ, string response)
        {
            string message = string.Format("<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    " +
                    "<meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    " +
                    "<title>Response your problem from Booking Room Hotel, KIOT!</title>\r\n</head>\r\n<body>\r\n    <h2>Response for your problem,</h2>\r\n    " +
                    "<p>Hi <strong>{0},</strong></p>\r\n    " +
                    "<p>We have received your request. Your request: <span style=\"color:red; font-style: italic;\">{1}</span></p>\r\n    " +
                    "<p>Message: <span style=\"color:red; font-style: italic;\">{2}</span> </p>\r\n    " +
                    "<p>Response: <span style=\"color:red; font-style: italic;\">{3}</span> </p>\r\n    " +
                    "<p>Thank you for using our service!</p>\r\n    <p>Best Regards,</p>\r\n    " +
                    "<p style=\"font-style: italic; font-weight: bold;\">KIOT Team</p>\r\n</body>\r\n</html>\r\n", name, title, messageQ, response);
            try
            {
                checkEmailValid(recip);
                SendMail("Send Request Successful! Booking Room Hotel, KIOT!", recip, message);
            }
            catch (Exception ex)
            {
                throw new Exception("Email could not be sent. " + ex.Message);
            }
        }

        public void SendChangePasswordMail(string recip, string name, string pw)
        {
            string message = string.Format("<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    " +
                    "<meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    " +
                    "<title>Change Password Success! Booking Room Hotel, KIOT!</title>\r\n</head>\r\n<body>\r\n    <h2>Change Password Successful!!</h2>\r\n    " +
                    "<p>Hi <strong>{0},</strong></p>\r\n    " +
                    "<p>Your New Password: <strong>{1}</strong></p>\r\n    " +
                    "<p>Thank you for using our service!</p>\r\n    <p>Best Regards,</p>\r\n    " +
                    "<p style=\"font-style: italic; font-weight: bold;\">KIOT Team</p>\r\n</body>\r\n</html>\r\n", name, pw);
            try
            {
                SendMail("Change Password Success! Booking Room Hotel, KIOT!", recip, message);
            }
            catch (Exception ex)
            {
                throw new Exception("Email could not be sent. " + ex.Message);
            }
        }

        public void SendForgotPasswordMail(string recip, string name, string pw)
        {
            string message = string.Format("<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    " +
                    "<meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    " +
                    "<title>Forgot Password! Booking Room Hotel, KIOT!</title>\r\n</head>\r\n<body>\r\n    <h2>Get Your Password Successful!!</h2>\r\n    " +
                    "<p>Hi <strong>{0},</strong></p>\r\n    " +
                    "<p>Your Old Password:<strong> {1}</strong></p>\r\n    " +
                    "<p>Please change your password when you receive this email</p>\r\n    " +
                    "<p>Thank you for using our service!</p>\r\n    <p>Best Regards,</p>\r\n    " +
                    "<p style=\"font-style: italic; font-weight: bold;\">KIOT Team</p>\r\n</body>\r\n</html>\r\n", name, pw);
            try
            {
                SendMail("Forgot Password! Booking Room Hotel, KIOT!", recip, message);
            }
            catch (Exception ex)
            {
                throw new Exception("Email could not be sent. " + ex.Message);
            }
        }
        public async void SendMail(string title, string recip, string s)
        {
            try
            {
                string fromMail = _configuration["EmailSetting:EmailID"];
                string fromPassword = _configuration["EmailSetting:AppPassword"];
                MailMessage message = new MailMessage();
                message.From = new MailAddress(fromMail);
                message.Subject = title;
                message.To.Add(new MailAddress(recip));
                message.Body = s;
                message.IsBodyHtml = true;

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true,
                };

                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void checkEmailValid(string email)
        {
            try
            {
                string pattern = @"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$";
                if (!Regex.IsMatch(email, pattern))
                {
                    throw new Exception("Email invalid!");
                }
                string[] domain = email.Split('@');
                if (domain.Length >= 2)
                {
                    IPHostEntry emailEntry = Dns.GetHostEntry(domain[domain.Length - 1]);
                    if (emailEntry == null || emailEntry.AddressList.Length == 0)
                    {
                        throw new Exception("Email invalid!");
                    }
                }
                else
                {
                    throw new Exception("Email invalid!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
