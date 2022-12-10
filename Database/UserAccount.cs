using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Database
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

        public bool VerificarSenha(string typedPass, string registeredPass)
        {
            if (string.IsNullOrEmpty(registeredPass))
                throw new NullReferenceException("Cadastre uma senha.");

            var encryptedPassword = _algoritmo.ComputeHash(Encoding.UTF8.GetBytes(typedPass));

            var sb = new StringBuilder();
            foreach (var caractere in encryptedPassword)
            {
                sb.Append(caractere.ToString("X2"));
            }

            return sb.ToString() == registeredPass;
        }
    }
    public class UserAccount
    {
        private string sqlConn()
        {
            return ConfigurationManager.AppSettings["sqlConn"];
        }

        public void NewAccount(int accessLevel, int accountStatus, string birthDate, string cpf, string email, string shortName, string fullName, string gender, int loginStatus, string nickname, string number, string pass, string sessionId)
        {
            using (SqlConnection connection = new SqlConnection(sqlConn()))
            {
                string queryString = "insert into userAccounts (accessLevel, accountStatus, birthDate, cpf, email, fullName, gender, loginStatus, nickname, number, pass, sessionId, shortName, signUpDate)" +
                                     " values (" + accessLevel + ", " + accountStatus + ", '" + birthDate + "', '" + cpf + "', '" + email + "', '" + fullName + "', '" + gender + "', " + loginStatus + ", '" + nickname + "', '" + number + "', '" + pass + "', '" + sessionId + "', '" + shortName + "', GETDATE())";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public DataTable FindLoginBySessionId(string sessionID)
        {
            using (SqlConnection connection = new SqlConnection(sqlConn()))
            {
                string queryString = "select * from userAccounts where loginStatus = 1 and sessionId = '" + sessionID + "'";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;

                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public void Disconnect(string sessionId)
        {
            using (SqlConnection connection = new SqlConnection(sqlConn()))
            {
                string queryString = "update userAccounts set sessionId = '', loginStatus = 0 where loginStatus = 1 and sessionId = '" + sessionId + "'";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void Login(string email, string pass, string sessionId)
        {
            var hash = new Hash(SHA512.Create());
            string encryptedPass = hash.PassEncrypt(pass);

            using (SqlConnection connection = new SqlConnection(sqlConn()))
            {
                string queryString = "select * from userAccounts where email = '" + email + "' and pass = '" + encryptedPass+ "'";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;

                DataTable table = new DataTable();
                adapter.Fill(table);

                if (table == null)
                {
                }
                else
                {
                    string queryStringDois = "update userAccounts set loginStatus = 1, sessionId = '" + sessionId+ "' where email = '" + email + "' and pass = '" + encryptedPass + "'";
                    SqlCommand commandDois = new SqlCommand(queryStringDois, connection);
                    commandDois.ExecuteNonQuery();
                }
            }
        }

        public DataTable SearchUserAccountByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(sqlConn()))
            {
                string queryString = "select * from userAccounts where email = '" + email + "'";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;

                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public DataTable SearchUserAccountByPass(string email, string pass)
        {
            var hash = new Hash(SHA512.Create());
            string encryptedPass = hash.PassEncrypt(pass);

            using (SqlConnection connection = new SqlConnection(sqlConn()))
            {
                string queryString = "select * from userAccounts where pass = '" + encryptedPass + "' and email = '" + email + "'";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = command;

                DataTable table = new DataTable();
                adapter.Fill(table);
                return table;
            }
        }

        public void UserLogin(string sessionId, string userNickname)
        {
            using (SqlConnection connection = new SqlConnection(sqlConn()))
            {
                string queryString = "update userAccounts set userLoggedNickname = '" + userNickname + "' where sessionId = '" + sessionId + "'";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
