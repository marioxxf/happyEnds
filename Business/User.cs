using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public class User
    {
        public int id { get; set; }
        public int accountId { get; set; }
        public DateTime birthDate { get; set; }
        public int moviesListId { get; set; }
        public string nickname { get; set; }

        public static object FindUserOfUserAccountDataByNicknameAndUserAccountId(string nickname, int id)
        {
            var userOfUserAccount = new User();
            var dataBaseUser = new Database.User();
            foreach (DataRow row in dataBaseUser.FindUserOfUserAccountDataByNickname(nickname, id).Rows)
            {
                userOfUserAccount.id = Convert.ToInt32(row["id"]);
                userOfUserAccount.accountId = Convert.ToInt32(row["accountId"]);
                userOfUserAccount.birthDate= Convert.ToDateTime(row["birthDate"]);
                userOfUserAccount.moviesListId = Convert.ToInt32(row["moviesListId"]);
                userOfUserAccount.nickname = row["nickname"].ToString();
            }
            return userOfUserAccount;
        }

        public void PostActivity(string userAccountLoggedId, string userOfUserAccountLoggedId, string bookCategory, string contentsType)
        {
            new Database.User().PostUserActivity(userAccountLoggedId, userOfUserAccountLoggedId, bookCategory, contentsType);
        }

        public List<User> SearchUsersByUserAccountNickname(string userAccountId)
        {
            var usersList = new List<User>();
            var userDatabase = new Database.User();
            foreach (DataRow row in userDatabase.SearchUserByUserAccountNickname(int.Parse(userAccountId)).Rows)
            {
                var user = new User();
                user.id = Convert.ToInt32(row["id"]);
                user.accountId = Convert.ToInt32(row["accountId"]);
                user.birthDate = Convert.ToDateTime(row["birthDate"]);
                user.moviesListId = Convert.ToInt32(row["moviesListId"]);
                user.nickname = row["nickname"].ToString();
                usersList.Add(user);
            }
            return usersList;
        }
    }

    
}
