using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ExtensionMethods
{
    public class Extensions
    {
        private static byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

        public static string GetNumbers(string Text)
        {
            Text = Text ?? string.Empty;
            return new string(Text.Where(p => char.IsDigit(p)).ToArray());
        }

        public static string GetFormattedPhone(string phone)
        {
            var formattedPhone = GetNumbers(phone);
            if (formattedPhone != null && formattedPhone.Trim().Length == 10)
                return string.Format("({0}) {1}-{2}", formattedPhone.Substring(0, 3), formattedPhone.Substring(3, 3), formattedPhone.Substring(6, 4));
            return formattedPhone;
        }


        public static string Crypt(string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Convert.ToBase64String(outputBuffer);
        }

        public static string Decrypt(string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            byte[] inputbuffer = Convert.FromBase64String(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Encoding.Unicode.GetString(outputBuffer);
        }

        public class CheckSession : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext context)
            {

                if (context.HttpContext.User.IsInRole("Agent") || context.HttpContext.User.IsInRole("Account Manager"))
                {
                    if (context.HttpContext.Session["CampaignId"] == null || context.HttpContext.Session["TimeZoneId"] == null || context.HttpContext.Session["ProjectId"] == null)
                    {
                        context.Result = new RedirectResult("~/Account/SelectCampaign");
                    }
                }
                base.OnActionExecuting(context);
            }
        }

    }
}