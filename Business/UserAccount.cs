using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class UserAccount
    {
        public int id { get; set; }
        public int accessLevel { get; set; }
        public int accountStatus { get; set; }
        public string birthDate { get; set; }
        public string cpf { get; set; }
        public string email { get; set; }
        public string shortName { get; set; }
        public string fullName { get; set; }
        public string gender { get; set; }
        public int loginStatus { get; set; }
        public string nickname { get; set; }
        public string number { get; set; }
        public string pass { get; set; }
        public string sessionId { get; set; }
        public string signUpDate { get; set; }
        public string userLoggedNickname { get; set; }

        public void Save()
        {
            new Database.UserAccount().NewAccount(this.accessLevel, this.accountStatus, 
                this.birthDate, this.cpf, this.email, this.shortName, this.fullName, 
                this.gender, this.loginStatus, this.nickname, this.number, this.pass, this.sessionId);
        }

        public static object FindLoginWithSessionId(string sessionID)
        {
            var userAccount = new UserAccount();
            var dataBaseUserAccount = new Database.UserAccount();
            foreach (DataRow row in dataBaseUserAccount.FindLoginBySessionId(sessionID).Rows)
            {
                userAccount.id                  = Convert.ToInt32(row["id"]);
                userAccount.accessLevel         = Convert.ToInt32(row["accessLevel"]);
                userAccount.accountStatus       = Convert.ToInt32(row["accountStatus"]);
                userAccount.birthDate           = row["birthDate"].ToString();
                userAccount.cpf                 = row["cpf"].ToString();
                userAccount.email               = row["email"].ToString();
                userAccount.fullName            = row["fullName"].ToString();
                userAccount.gender              = row["gender"].ToString();
                userAccount.loginStatus         = Convert.ToInt32(row["loginStatus"]);
                userAccount.nickname            = row["nickname"].ToString();
                userAccount.number              = row["number"].ToString();
                userAccount.pass                = row["pass"].ToString();
                userAccount.sessionId           = row["sessionId"].ToString(); 
                userAccount.shortName           = row["shortName"].ToString();
                userAccount.signUpDate          = row["signUpDate"].ToString();
                userAccount.userLoggedNickname  = row["userLoggedNickname"].ToString();
            }
            return userAccount;
        }

        public void Disconnect(string sessionId)
        {
            new Database.UserAccount().Disconnect(sessionId);
        }

        public void Login()
        {
            new Database.UserAccount().Login(this.email, this.pass, this.sessionId);
        }

        public List<UserAccount> SearchUserAccountByEmail(string email)
        {
            var userAccountList = new List<UserAccount>();
            var databaseUserAccount = new Database.UserAccount();
            foreach (DataRow row in databaseUserAccount.SearchUserAccountByEmail(email).Rows)
            {
                var userAccount = new UserAccount();
                userAccount.email = row["email"].ToString();
                userAccountList.Add(userAccount);
            }
            return userAccountList;
        }

        public List<UserAccount> SearchUserAccountByPass(string email, string pass)
        {
            var userAccountList = new List<UserAccount>();
            var databaseUserAccount = new Database.UserAccount();
            foreach (DataRow row in databaseUserAccount.SearchUserAccountByPass(email, pass).Rows)
            {
                var userAccount = new UserAccount();
                userAccount.email = row["email"].ToString();
                userAccount.pass    = row["pass"].ToString();
                userAccountList.Add(userAccount);
            }
            return userAccountList;
        }

        public void UserLogin(string sessionId, string userNickname)
        {
            new Database.UserAccount().UserLogin(userNickname, sessionId);
        }
    }
}
