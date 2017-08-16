using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ConGun.Models;
using DataAccessLayer;
using System.Data;
using BusinessAccessLayer;
using System.Net.Mail;
using System.Net;
using System.Configuration;
using System.Net.Configuration;

namespace ConGun.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        AccountDAL objAccountDAL = new AccountDAL();

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(BusinessAccessLayer.AccountModel.SignUpViewModel model, string returnUrl)
        {
            DataTable dtData = objAccountDAL.LoginDetail(model.LoginModel);
            if (dtData.Rows.Count > 0)
            {
                Session["UserID"] = dtData.Rows[0]["UserID"];
                Session["UserIcon"] = dtData.Rows[0]["EmailID"].ToString().ToUpper().Trim()[0];
                Session["EmailID"] = dtData.Rows[0]["EmailID"].ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                AccountModel.RegisterViewModel objRegister = new AccountModel.RegisterViewModel();
                model.RegisterModel = objRegister;
                ViewBag.ErrorMessageLogin = "Invalid Login. Please enter valid credentails and try again.";
                return View("Signup", model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            BusinessAccessLayer.AccountModel.RegisterViewModel model = new BusinessAccessLayer.AccountModel.RegisterViewModel();
            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(BusinessAccessLayer.AccountModel.SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DataTable dtData = objAccountDAL.RegisterUserDetail(model.RegisterModel);
                    if (dtData.Rows.Count > 0)
                    {
                        if (Convert.ToString(dtData.Rows[0]["ErrorMessage"]) == "")
                        {
                            Session["UserID"] = dtData.Rows[0]["UserID"];
                            Session["UserIcon"] = dtData.Rows[0]["EmailID"].ToString().ToUpper().Trim()[0];
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            if (Convert.ToString(dtData.Rows[0]["ErrorMessage"]).ToUpper() == "EMAIL EXISTS")
                                model.RegisterModel.EmailErrorMessage = Convert.ToString(dtData.Rows[0]["ErrorMessage"]);
                            if (Convert.ToString(dtData.Rows[0]["ErrorMessage"]).ToUpper() == "CONTACT EXISTS")
                                model.RegisterModel.ErrorMessage = Convert.ToString(dtData.Rows[0]["ErrorMessage"]);
                            return View("Signup", model);
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DataTable dtData = objAccountDAL.PasswordReset(model.Email);
                    if (Convert.ToBoolean(dtData.Rows[0]["IsValid"].ToString()))
                    {
                        var smtpSection = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                        MailMessage mail = new MailMessage();
                        mail.To.Add(model.Email);
                        mail.From = new MailAddress(smtpSection.Network.UserName);
                        mail.Subject = "Congun Password Reset";
                        string Body = "<div><h4>Password Reset.</h4><hr />Dear user please use the password <b>" + dtData.Rows[0]["PasswordReset"].ToString() + "</b> to login into your account. You can change your password once you logged in.<br /><br /><br />Regards<br />Congun Team.</div>";
                        mail.Body = Body;
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = smtpSection.Network.Host;
                        smtp.Port = smtpSection.Network.Port;
                        smtp.EnableSsl = smtpSection.Network.EnableSsl;
                        //smtp.UseDefaultCredentials = smtpSection.Network.DefaultCredentials;
                        smtp.Credentials = new System.Net.NetworkCredential
                        (smtpSection.Network.UserName, smtpSection.Network.Password);// Enter seders User name and password  
                        smtp.Send(mail);
                    }
                }
                else
                {
                    model.ErrorMessage = "Email does not exists. Please enter a valid email.";
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/Logoff
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            Session["UserID"] = null;
            AccountModel.RegisterViewModel objRegister = new AccountModel.RegisterViewModel();
            AccountModel.SignUpViewModel model = new AccountModel.SignUpViewModel();
            AccountModel.LoginViewModel objLogin = new AccountModel.LoginViewModel();
            model.RegisterModel = objRegister;
            model.LoginModel = objLogin;
            return View("Signup", model);
        }

        [AllowAnonymous]
        public ActionResult MyProfile()
        {
            if (Session["UserID"] != null)
            {
                DataSet dtSet = objAccountDAL.AccountDetail(Convert.ToInt16(Session["UserID"].ToString()));
                ViewBag.ProfileDetail = dtSet.Tables[0];
                if (dtSet.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow item in dtSet.Tables[1].Rows)
                    {
                        if (Convert.ToString(item["FileName1"]) != "")
                        {
                            item["ImagePath"] = ConfigurationManager.AppSettings["UsedEquipmentImageLoc"].ToString().Replace("~", "") + item["ContactNumber"] + "/" + Convert.ToString(item["FileName1"]);
                            dtSet.Tables[1].AcceptChanges();
                        }
                        if (Convert.ToString(item["Price"]) != "")
                        {
                            string fare = item["Price"].ToString();
                            decimal parsed = decimal.Parse(fare, CultureInfo.InvariantCulture);
                            CultureInfo hindi = new CultureInfo("hi-IN");
                            string text = string.Format(hindi, "{0:#,#}", parsed);

                            item["Price"] = text;
                        }
                        else
                        {
                            item["Price"] = "NA";
                        }
                        dtSet.Tables[1].AcceptChanges();
                    }

                    ViewBag.ProfileUsedDetail = dtSet.Tables[1];
                }
                if (dtSet.Tables[2].Rows.Count > 0)
                {
                    ViewBag.ProfileRentalDetail = dtSet.Tables[2];
                }
                return View();
            }
            AccountModel.SignUpViewModel objSignup = new AccountModel.SignUpViewModel();
            AccountModel.RegisterViewModel objRegister = new AccountModel.RegisterViewModel();
            AccountModel.LoginViewModel objLogin = new AccountModel.LoginViewModel();
            objSignup.RegisterModel = objRegister;
            objSignup.LoginModel = objLogin;
            return View("Signup", objSignup);
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        [AllowAnonymous]
        public ActionResult Signup(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            AccountModel.SignUpViewModel objSignup = new AccountModel.SignUpViewModel();
            AccountModel.RegisterViewModel objRegister = new AccountModel.RegisterViewModel();
            AccountModel.LoginViewModel objLogin = new AccountModel.LoginViewModel();
            objSignup.RegisterModel = objRegister;
            objSignup.LoginModel = objLogin;
            return View(objSignup);
        }

        [AllowAnonymous]
        public ActionResult Exception()
        {
            return View();
        }
        #endregion
    }
}