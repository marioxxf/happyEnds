using Business;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace gioiasFlix.Controllers
{
    public class Hash
    {
        private HashAlgorithm _algoritmo;

        public Hash(HashAlgorithm algorithm)
        {
            _algoritmo = algorithm;
        }

        public string PassEncrypt(string pass)
        {
            var encodedValue = Encoding.UTF8.GetBytes(pass);
            var encryptedPassword = _algoritmo.ComputeHash(encodedValue);

            var sb = new StringBuilder();
            foreach (var caracter in encryptedPassword)
            {
                sb.Append(caracter.ToString("X2"));
            }

            return sb.ToString();
        }
    }
    public class AuthController : Controller
    {
        public ActionResult RegisterUserAccountInterface()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public void RegisterUserAccount()
        {
            try
            {
                var userAccount = new UserAccount();
                userAccount.accessLevel = 1;
                userAccount.accountStatus = 1;
                userAccount.birthDate = "";
                userAccount.cpf = "";
                userAccount.email = Request["txtEmail"];
                userAccount.shortName = Request["txtName"] + " " + Request["txtNameTwo"];
                userAccount.fullName = "";
                userAccount.gender = "";
                userAccount.loginStatus = 1;
                userAccount.nickname = Request["txtNickname"];
                userAccount.number = Request["txtNumber"];

                var hash = new Hash(SHA512.Create());
                userAccount.pass = hash.PassEncrypt(Request["txtPass"]);

                userAccount.sessionId = Request["sessionId"];
                userAccount.Save();
                Response.Redirect("/");
            }
            catch (Exception erro)
            {
                TempData["newAccountUserError"] = "A conta não pode ser criada (" + erro.Message + ")!";
            }
        }

        public ActionResult CheckEmail(string email)
        {
            var userAccountWithReportedEmail = new UserAccount().SearchUserAccountByEmail(email);
            var userAccount = new UserAccount();

            if (userAccountWithReportedEmail.Count > 0)
            {
                userAccount = userAccountWithReportedEmail[0];
            }
            else
            {
            }

            var resultOfTheSearch = "";
            if(userAccount.email == "" || userAccount.email == null)
            {
                resultOfTheSearch = "userAccountNotFounded";
            }
            else
            {
                resultOfTheSearch = "userAccountFounded";
            }

            JsonResult result = new JsonResult();
            result = this.Json(JsonConvert.SerializeObject(resultOfTheSearch), JsonRequestBehavior.AllowGet);
            return result;
        }

        public ActionResult CheckPass(string email, string pass)
        {
            var userAccountWithReportedPass = new UserAccount().SearchUserAccountByPass(email, pass);
            var userAccount = new UserAccount();

            if (userAccountWithReportedPass.Count > 0)
            {
                userAccount = userAccountWithReportedPass[0];
            }
            else
            {
            }

            var resultOfTheSearch = "";
            if (userAccount.email == "" || userAccount.email == null)
            {
                resultOfTheSearch = "nokPass";
            }
            else
            {
                resultOfTheSearch = "okPass";
            }

            JsonResult result = new JsonResult();
            result = this.Json(JsonConvert.SerializeObject(resultOfTheSearch), JsonRequestBehavior.AllowGet);
            return result;
        }

        [HttpPost]
        [ValidateInput(false)]
        public void UserAccountLogin()
        {
            try
            {
                var userAccount = new UserAccount();
                userAccount.email = Request["txtEmailLoginForm"];
                userAccount.pass = Request["txtPassLoginForm"];
                userAccount.sessionId = Request["sessionIdLoginForm"];
                userAccount.Login();
                Response.Redirect("/auth/chooseuser");
            }
            catch (Exception error)
            {
                TempData["contaNaoCriada"] = "User Account's can't be logged (" + error.Message + ")!";
            }
        }

        [ChildActionOnly]
        public ActionResult UserAccountLoggedShortName()
        {
            var userAccount = Business.UserAccount.FindLoginWithSessionId(Session.SessionID);
            Business.UserAccount userAccountLogged = (UserAccount)userAccount;
            string shortNameOfUserAccountLogged = userAccountLogged.shortName;
            return new ContentResult { Content = shortNameOfUserAccountLogged };
        }

        [ChildActionOnly]
        public ActionResult UserAccountLoggedId()
        {
            var userAccount = Business.UserAccount.FindLoginWithSessionId(Session.SessionID);
            Business.UserAccount userAccountLogged = (UserAccount)userAccount;
            int idOfUserAccountLogged = userAccountLogged.id;
            return new ContentResult { Content = idOfUserAccountLogged.ToString() };
        }

        [HttpPost]
        public ActionResult UserAccountDisconnect()
        {
            var userAccount = new UserAccount();
            userAccount.Disconnect(Session.SessionID);
            /*ViewBag.PosDesconexao = "Foi";
            string message = "Sucesso!";*/
            return View();
        }

        public ActionResult ChooseUserOfUserAccount()
        {
            return View();
        }

        public ActionResult UserOfUserAccountLogin(string sessionId, string userNickname)
        {
            var userAccount = Business.UserAccount.FindLoginWithSessionId(sessionId);
            Business.UserAccount userAccountLogged = (UserAccount)userAccount;
            userAccountLogged.UserLogin(userNickname, sessionId);

            var nothing = "nothing";

            JsonResult result = new JsonResult();
            result = this.Json(JsonConvert.SerializeObject(nothing), JsonRequestBehavior.AllowGet);
            return result;
        }

        [ChildActionOnly]
        public ActionResult UserOfUserAccountLoggedNickname()
        {
            var userAccount = Business.UserAccount.FindLoginWithSessionId(Session.SessionID);
            Business.UserAccount userAccountLogged = (UserAccount)userAccount;
            string userOfUserAccountLoggedNickname = userAccountLogged.userLoggedNickname;
            return new ContentResult { Content = userOfUserAccountLoggedNickname };
        }

        [ChildActionOnly]
        public ActionResult UserOfUserAccountLoggedId()
        {
            var userAccount = Business.UserAccount.FindLoginWithSessionId(Session.SessionID);
            Business.UserAccount userAccountLogged = (UserAccount)userAccount;

            var userOfUserAccountSelected = Business.User.FindUserOfUserAccountDataByNicknameAndUserAccountId(userAccountLogged.userLoggedNickname, userAccountLogged.id);
            Business.User userOfUserAccountLogged = (User)userOfUserAccountSelected;

            string userOfUserAccountLoggedId = userOfUserAccountLogged.id.ToString();
            return new ContentResult { Content = userOfUserAccountLoggedId };
        }

        public ActionResult MyAccount()
        {
            return View();
        }
    }
}