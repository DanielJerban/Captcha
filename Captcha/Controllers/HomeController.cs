using Captcha.Captcha;
using Captcha.Captcha.Encryption;
using Captcha.Captcha.Models;
using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.Web.Mvc;

namespace Captcha.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public const string CaptchaMainKey = "f[Vun-'Z{Rqw#z2J";
        public ActionResult CreateCaptcha()
        {
            (string code, string encryptedValue) New()
            {
                var encryption = new AesEncryption();
                Random random = new();
                var code = random.Next(1001, 9999).ToString();

                //encrypt code
                //generate salt for each request
                var salt = KeyProvider.CreateNewSalt();

                //generate captcha model
                dynamic captchaModel = new ExpandoObject();
                captchaModel.Captcha = encryption.Encrypt(code, salt);
                captchaModel.Salt = salt;

                //encrypt captcha model
                string encrypted = encryption.Encrypt(JsonConvert.SerializeObject(captchaModel), CaptchaMainKey);

                return (code, encrypted);
            }
            var (captchaCode, encryptedValue) = New();

            // todo: add "encryptedValue" to cache 

            // todo: read captcha option from web.config
            CaptchaGenerator captchaGenerator = new CaptchaGenerator(new CaptchaOptions());
            var captchaImage = captchaGenerator.GenerateImageAsStream(captchaCode);

            return new FileStreamResult(captchaImage, "image/png");
        }
    }
}