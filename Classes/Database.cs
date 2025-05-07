using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.ComponentModel.Design;

namespace donely_Inspilab.Classes
{
    public class Database
    {
        // Voor db te kunnen gebruiken volgens je eigen settings --> appsettings.json file aanmaken
        // dan iets zoals:
        /*
            "ConnectionStrings": {
            "DefaultConnection": "Server=localhost,3306;Database=donely_development;Uid=x;Pwd=x;" --> hier je eigen details invullen waar nodig
            } 
         */
        // Tools -> NuGet packet manager -> deze commando"s uitvoeren
        /*  Install-Package Microsoft.Extensions.Configuration
            Install-Package Microsoft.Extensions.Configuration.Json
            Install-Package Microsoft.Extensions.Configuration.Binder
         */
        private string connectionString = App.Configuration.GetConnectionString("DefaultConnection");


        private int ExecuteNonQuery(string qry, Dictionary<string, object> parameters, out int insertedId) // INSERT, DELETE, UPDATE
        {
            insertedId = -1;
            using MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                using var commandDb = new MySqlCommand(qry, connection);
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        commandDb.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                int affectedRows = commandDb.ExecuteNonQuery();
                insertedId = (int)commandDb.LastInsertedId;
                return affectedRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        private List<Dictionary<string, object>> ExecuteReader(string qry, Dictionary<string, object> parameters = null) // SELECT (multiple)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            using MySqlConnection connection = new MySqlConnection(connectionString);
            try
            {
                connection.Open();
                using var commandDb = new MySqlCommand(qry, connection);
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        commandDb.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                using MySqlDataReader reader = commandDb.ExecuteReader();
                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.GetValue(i);
                    }
                    results.Add(row);
                }
                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return results;
        }

        public int InsertUser(User newUser)
        {
            Dictionary<string, object> parameters = [];
            string qry = "INSERT INTO users (name, email, telephone_nr, is_admin, profile_picture) VALUES (@name, @mail, @phone, @is_admin, @profile_picture)";
            parameters.Add("@name", newUser.Name);
            parameters.Add("@mail", newUser.Email);
            parameters.Add("@phone", newUser.TelephoneNumber);
            parameters.Add("@is_admin", newUser.IsAdmin);
            parameters.Add("@profile_picture", newUser.ProfilePicture);
            int rowsAffected = ExecuteNonQuery(qry, parameters, out int newUserID);
            if (rowsAffected == -1)
                throw new ArgumentException("Something went wrong, new user wasn't added");

            qry = "INSERT INTO user_passwords (userID, password, has_mfa) VALUES (@userID, @password, @mfa)";
            parameters.Clear();
            parameters.Add("@userID", newUserID);
            parameters.Add("@password", newUser.HashedPassword);
            parameters.Add("@mfa", newUser.Is2FA);
            ExecuteNonQuery(qry, parameters, out _);

            return rowsAffected;
        }

        public Credentials GetUserCredentialsByEmail(string email)
        {
            string qry = @"
                SELECT up.* FROM user_passwords up
                JOIN users u ON up.userID = u.userID
                WHERE u.email = @mail;";
            Dictionary<string, object> parameters = [];
            parameters.Add("@mail", email);
            Dictionary<string, object> result = ExecuteReader(qry, parameters)[0];
            string hashedPassword = result["password"].ToString();
            int userID = (int)result["userID"];
            bool is2FA = (bool)result["has_mfa"];
            return new Credentials(userID, hashedPassword, is2FA);
        }
        public User GetUserByID(object userID)
        {
            string qry = @"
                SELECT * FROM users
                WHERE userID = @userID;";
            Dictionary<string, object> parameters = [];
            parameters.Add("@userID", userID);
            Dictionary<string, object> result = ExecuteReader(qry, parameters)[0];
            string _name = result["name"].ToString();
            string _email = result["email"].ToString();
            string _tel = result["telephone_nr"].ToString();
            bool _isAdmin = (bool)result["is_admin"];
            string _profilePicture = result["profile_picture"].ToString();
            DateTime _created = Convert.ToDateTime(result["created"]);
            DateTime _lastLogin = Convert.ToDateTime(result["last_login"]);
            int _id = (int)result["userID"];

            return new User(_name, _email, _tel, _profilePicture, _id, _created, _lastLogin, _isAdmin);
        }

        public void UpdateLogin(int id)
        {
            string sql = "UPDATE users SET last_login = @lastLogin WHERE userID = @userID";
            Dictionary<string, object> parameters = [];
            parameters.Add("@lastLogin", DateTime.Now);
            parameters.Add("@userID", id);
            ExecuteNonQuery(sql, parameters, out _);
        }

        public int DeleteUser(int id)
        {
            string qry = "DELETE FROM users WHERE userID = @userID";
            Dictionary<string, object> parameters = [];
            parameters.Add("@userID", id);
            return ExecuteNonQuery(qry, parameters, out _);
        }

    }
}
