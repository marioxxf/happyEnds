using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Security.Policy;

namespace Database
{
    public class User
    {
        private string sqlConn()
        {
            return ConfigurationManager.AppSettings["sqlConn"];
        }
        public DataTable SearchUserByUserAccountNickname(int userAccountId)
        {
            using (SqlConnection connection = new SqlConnection(sqlConn()))
            {
                string queryString = "select * from users where accountId = " + userAccountId;
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;

                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public DataTable FindUserOfUserAccountDataByNickname(string nickname, int id)
        {
            using (SqlConnection connection = new SqlConnection(sqlConn()))
            {
                string queryString = "select * from users where nickname = '" + nickname + "' and accountId = " + id;
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;

                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public void PostUserActivity(string userAccountLoggedId, string userOfUserAccountLoggedId, string bookCategory, string contentsType)
        {
            using (SqlConnection connection = new SqlConnection(sqlConn()))
            {
                string queryString = "insert into userActivity (idUserAccount, idUser, category, visitDate, contentsType)" +
                    "values (" + userAccountLoggedId + ", " + userOfUserAccountLoggedId + ", '" + bookCategory + "', getdate(), '" + contentsType + "')";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
