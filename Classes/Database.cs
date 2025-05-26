using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using System.ComponentModel.Design;
using donely_Inspilab.Exceptions;

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

        #region EXECUTORS
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
            catch (MySqlException ex) when (ex.Number == 1062)
            {
                throw new DuplicateException("Duplicate value", ex); //nog aanpassen voor andere unieke velden, zal nu altijd email exception geven
            }
            catch (MySqlException ex)
            {
                throw new DataAccessException("Database error occurred.", ex);
            }
        }

        private List<Dictionary<string, object>> ExecuteReader(string qry, Dictionary<string, object> parameters = null) // SELECT (multiple)
        {
            List<Dictionary<string, object>> results = new();
            using MySqlConnection connection = new MySqlConnection(connectionString);

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

        #endregion

        #region USERS
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
            var res = ExecuteReader(qry, parameters);
            if (res.Count ==0) throw new ArgumentException("Wrong password or email"); //Eerst in var (of list maar wil dat niet uittypen) om dan in dict te steken. Anders error als onbekend emailadres bij login


            Dictionary<string, object> result = res[0];
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
            string qry = "DELETE  FROM users WHERE userID = @userID";
            Dictionary<string, object> parameters = [];
            parameters.Add("@userID", id);
            return ExecuteNonQuery(qry, parameters, out _);
        }
        public List<User> GetAllUsers()
        {
            string qry = "SELECT * FROM users";
            var results = ExecuteReader(qry);
            List<User> users = new();

            foreach (var result in results)
            {
                users.Add(new User(
                    _name: result["name"].ToString(),
                    _email: result["email"].ToString(),
                    _telephoneNumber: result["telephone_nr"].ToString(),
                    _profilePicture: result["profile_picture"].ToString(),
                    _id: Convert.ToInt32(result["userID"]),
                    _accountCreated: Convert.ToDateTime(result["created"]),
                    _lastLogin: Convert.ToDateTime(result["last_login"]),
                    _isAdmin: Convert.ToBoolean(result["is_admin"])
                ));
            }

            return users;
        }
        public void UpdateUser(int userId, string name, string email, string phone)
        {
            string qry = "UPDATE users SET name = @name, email = @mail, telephone_nr = @phone WHERE userID = @id";
            var parameters = new Dictionary<string, object>
            {
                ["@id"] = userId,
                ["@name"] = name,
                ["@mail"] = email,
                ["@phone"] = phone
            };

            ExecuteNonQuery(qry, parameters, out _);
        }


        #endregion

        #region GROUPS
        public int InsertGroup(Group newGroup)
        {
            Dictionary<string, object> parameters = [];
            string qry = "INSERT INTO groups_ (name, owner, image, invite_code) VALUES (@name, @owner, @image, @invite_code)";
            parameters.Add("@name", newGroup.Name);
            parameters.Add("@owner", newGroup.Owner.Id);
            parameters.Add("@image", newGroup.ImageLink);
            parameters.Add("@invite_code", newGroup.InviteCode);
            int rowsAffected = ExecuteNonQuery(qry, parameters, out int newGroupID);
            if (rowsAffected == -1)
                throw new ArgumentException("Something went wrong, new group wasn't added");
            return newGroupID;
        }

        public bool CheckInviteCode(string code)
        {
            string qry = "SELECT invite_code FROM Groups_ WHERE invite_code = @code";
            Dictionary<string, object> parameters = new Dictionary<string, object>{ ["@code"] = code };
            return (ExecuteReader(qry, parameters).Count!=1);
        }

        public (int groupID, string name, int ownerID) GetGroupIdByInviteCode(string code)
        {
            string qry = "SELECT groupID, name, owner FROM Groups_ WHERE invite_code = @code";
            Dictionary<string, object> parameters = new Dictionary<string, object> { ["@code"] = code };
            var res = ExecuteReader(qry, parameters);
            if (res.Count == 0) throw new ArgumentException("Code not found");
            int groupID = Convert.ToInt32(res[0]["groupID"]);
            string groupName = res[0]["name"].ToString();
            int ownerID = Convert.ToInt32(res[0]["owner"]);
            return (groupID, groupName, ownerID);
        }

        public bool MemberPresentInGroup(int groupID, int userID)
        {
            string qry = "SELECT groupID, userID FROM group_users WHERE groupID = @groupID AND userID = @userID";
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                ["@groupID"] = groupID,
                ["@userID"] = userID
            };
            return (ExecuteReader(qry, parameters).Count != 0);
        }

        public int InsertNewGroupMember(GroupMember member)
        {
            if (MemberPresentInGroup(member.GroupId, member.UserId))
            {
                throw new DuplicateException("This user is already in this group");
            }
            string qry = "INSERT INTO group_users (userID, groupID, currency, role) VALUES (@userID, @groupID, @currency, @role)";
            Dictionary<string, object> parameters = new Dictionary<string, object> {
                ["@userID"] = member.UserId,
                ["@groupID"] = member.GroupId,
                ["@currency"] = member.Currency,
                ["@role"] = member.Role,
            };
            ExecuteNonQuery(qry, parameters, out int groupUserId);
            return groupUserId;
        }

        public List<Group> GetGroupOverview(int userId)
        {
            string qry = @"
                        SELECT     
                            g.groupID, g.name AS group_name, g.owner AS group_owner,
                            g.creation_date, g.image AS group_image, g.invite_code,
                            u.userID AS user_id, u.name AS user_name, u.email, 
                            u.telephone_nr, u.is_admin, u.profile_picture, 
                            u.created, u.last_login
                        FROM groups_ g
                        JOIN group_users gu ON g.groupID = gu.groupID
                        JOIN users u ON g.owner = u.userId
                        WHERE gu.userId = @userID
            ";
            Dictionary<string, object> parameters = new() { ["@userID"] = userId };
            var results = ExecuteReader(qry, parameters);
            List<Group> groups = new List<Group>();
            foreach (var record in results)
            {
                User owner = new User(
                    _name: record["user_name"].ToString(),
                    _email: record["email"].ToString(),
                    _telephoneNumber: record["telephone_nr"].ToString(),
                    _profilePicture: record["profile_picture"].ToString(),
                    _id: Convert.ToInt32(record["user_id"]),
                    _accountCreated: Convert.ToDateTime(record["created"]),
                    _lastLogin: Convert.ToDateTime(record["last_login"]),
                    _isAdmin: Convert.ToBoolean(record["is_admin"])
                );

                Group group = new Group(
                    _id: Convert.ToInt32(record["groupID"]),
                    _name: record["group_name"].ToString(),
                    _owner: owner,
                    _creationDate: Convert.ToDateTime(record["creation_date"]),
                    _imageLink: record["group_image"].ToString(),
                    _inviteCode: record["invite_code"].ToString()
                );

                groups.Add(group);
            }
            return groups;
        }

        public List<Group> GetOwnGroups(User currentUser)
        {
            string qry = @"
                        SELECT *
                        FROM groups_ 
                        WHERE owner = @userID
            ";
            Dictionary<string, object> parameters = new() { ["@userID"] = currentUser.Id };
            var results = ExecuteReader(qry, parameters);
            List<Group> ownedGroups = new List<Group>();
            foreach (var record in results)
            {
                Group group = new Group(
                    _id: Convert.ToInt32(record["groupID"]),
                    _name: record["name"].ToString(),
                    _owner: currentUser, // already known
                    _creationDate: Convert.ToDateTime(record["creation_date"]),
                    _imageLink: record["image"].ToString(),
                    _inviteCode: record["invite_code"].ToString()
                );
                ownedGroups.Add(group);
            }
            return ownedGroups;
        }

        public List<GroupMember> GetGroupMembers(int groupID)
        {
            string qry = @"SELECT GU.group_userID, GU.currency, GU.joined, GU.role, u.* 
                    FROM group_users GU 
                    JOIN users u ON GU.userID = u.userID
                    WHERE groupID = @groupID;";
            Dictionary<string, object> parameters = new() { ["@groupID"] = groupID };

            var results = ExecuteReader(qry, parameters);

            List<GroupMember> groupMembers = new();

            foreach (var row in results)
            {
                  User user = new User(
                 _name: row["name"].ToString(),
                 _email: row["email"].ToString(),
                 _telephoneNumber: row["telephone_nr"] as string ?? string.Empty,
                 _profilePicture: row["profile_picture"] as string ?? "profilePicture.jpg",
                 _id: Convert.ToInt32(row["userID"]),
                 _accountCreated: Convert.ToDateTime(row["created"]),
                 _lastLogin: Convert.ToDateTime(row["last_login"]),
                 _isAdmin: Convert.ToBoolean(row["is_admin"])
             );

                List<ShopItem> boughtItems = new(); // TODO: Lijst populeren met gekochte items

                GroupMember groupMember = new (
                    _id: Convert.ToInt32(row["group_userID"]),
                    _user: user,
                    _groupID: groupID,
                    _currency: Convert.ToInt32(row["currency"]),
                    _boughtItems: boughtItems,
                    _joined: Convert.ToDateTime(row["joined"])
                );

                groupMembers.Add(groupMember);
            }
            return groupMembers;

        }
        #endregion

        #region ADMIN
        public int GetTotalUserCount()
        {
            string qry = "SELECT COUNT(*) FROM users";
            var res = ExecuteReader(qry);
            return Convert.ToInt32(res[0].Values.First());
        }

        public int GetTotalGroupCount()
        {
            string qry = "SELECT COUNT(*) FROM groups_";
            var res = ExecuteReader(qry);
            return Convert.ToInt32(res[0].Values.First());
        }
        #endregion

        #region SHOP
        public int InsertShopItems(List<ShopItem> shopItems, int groupID)
        {
            Dictionary<string, object> parameters = [];

            List<string> sets = [];
            for (int i = 0; i < shopItems.Count; i++){
                sets.Add($"(@groupID{i}, @name{i}, @description{i}, @price{i}, @limit{i})");
                parameters.Add($"@groupID{i}", groupID);
                parameters.Add($"@name{i}", shopItems[i].Name);
                parameters.Add($"@description{i}", shopItems[i].Description);
                parameters.Add($"@price{i}", shopItems[i].Price);
                parameters.Add($"@limit{i}", shopItems[i].Limit);
            }
            string qry = $"INSERT INTO shop_items (groupID, name, description, cost, `limit`) VALUES {string.Join(", ",sets)}";

            int rowsAffected = ExecuteNonQuery(qry, parameters, out int _);
            if (rowsAffected == -1)
                throw new ArgumentException("Something went wrong, new item wasn't added");
            return rowsAffected;
        }
        #endregion
    }
}
