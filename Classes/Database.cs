using donely_Inspilab.Enum;
using donely_Inspilab.Exceptions;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

        public void UpdateGroup(Group group)
        {
            string qry = "UPDATE groups_ SET name = @name, image = @image WHERE groupid = @id";
            Dictionary<string, object> parameters = new()
            {
                { "@id", group.Id },
                { "@name", group.Name },
                { "@image", group.ImageLink }
            };
            int rowsAffected = ExecuteNonQuery(qry, parameters, out _);

            if (rowsAffected != 1)
                throw new ArgumentException("Something went wrong, group wasn't updated.");
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

        public void DeleteGroupMember(int memberId)
        {
            string qry = "DELETE FROM group_users WHERE group_userID = @memberID;";
            Dictionary<string, object> parameters = new() { ["@memberID"] = memberId };
            int rowsAffected = ExecuteNonQuery(qry, parameters, out _);
            if (rowsAffected != 1)
                throw new Exception("Failed to delete member.");
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
            string qry = @"
                        SELECT 
                            gu.group_userID,
                            gu.currency,
                            gu.joined,
                            gu.role,
                            u.*,
                            COALESCE(t.ActiveTasks, 0) AS ActiveTasks,
                            COALESCE(t.PendingTasks, 0) AS PendingTasks,
                            COALESCE(t.CompletedTasks, 0) AS CompletedTasks
                        FROM group_users gu
                        JOIN users u ON gu.userID = u.userID
                        LEFT JOIN (
                            SELECT 
                                groupUserID,
                                SUM(CASE WHEN status = 0 THEN 1 ELSE 0 END) AS ActiveTasks,
                                SUM(CASE WHEN status = 1 THEN 1 ELSE 0 END) AS PendingTasks,
                                SUM(CASE WHEN status = 2 OR status = 3  THEN 1 ELSE 0 END) AS CompletedTasks
                                FROM task_instances
                                GROUP BY groupUserID
                             ) t ON gu.group_userID = t.groupUserID
                             WHERE gu.groupID = 17;";

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

                List<ShopItem> boughtItems = new(); // TODO

                GroupMember groupMember = new(
                    _id: Convert.ToInt32(row["group_userID"]),
                    _user: user,
                    _groupID: groupID,
                    _currency: Convert.ToInt32(row["currency"]),
                    _boughtItems: boughtItems,
                    _joined: Convert.ToDateTime(row["joined"]),
                    _activeTaskCount: Convert.ToInt32(row["ActiveTasks"]),
                    _pendingTaskCount:  Convert.ToInt32(row["PendingTasks"]),
                    _completedTaskCount: Convert.ToInt32(row["CompletedTasks"])
                );

                groupMembers.Add(groupMember);
            }

            return groupMembers;
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

        #region TASKS
        public int InsertTaskDefinition(Task task)
        {
            string qry = @"INSERT INTO tasks_definition(groupID, name, details, reward_currency, frequency, is_active, validation_required) 
                        VALUES(@groupID, @name, @details, @reward_currency, @frequency, @is_active, @validation_required)";
                Dictionary<string, object> parameters = new() {
                    ["@groupID"] = task.GroupId,
                    ["@name"] = task.Name,
                    ["@details"] = task.Description,
                    ["@reward_currency"] = task.Reward,
                    ["@frequency"] = task.Frequency,
                    ["@is_active"] = task.IsActive,
                    ["@validation_required"] = task.RequiresValidation,
                };
            int rowsAffected = ExecuteNonQuery(qry, parameters, out int taskId);
            if (rowsAffected != 1) //checken of werkelijk toegevoegd
                throw new ArgumentException("Something went wrong with the database");
            return taskId;
        }
        public Task GetTaskById(int taskId)
        {
            string qry = "SELECT * FROM tasks_definition WHERE taskID = @taskId;";
            Dictionary<string, object> parameters = new() { ["@taskId"] = taskId };
            var result = ExecuteReader(qry, parameters)[0];
            Task task = new Task(
                   _id: Convert.ToInt32(result["taskID"]),
                   _name: result["name"].ToString(),
                   _description: result["details"].ToString(),
                   _reward: Convert.ToInt32(result["reward_currency"]),
                   _frequency: (TaskFrequency)Convert.ToInt32(result["frequency"]),
                   _requiresValidation: Convert.ToBoolean(result["validation_required"]),
                   _IsActive: Convert.ToBoolean(result["is_active"]),
                   _groupId: Convert.ToInt32(result["groupID"])
               );
            return task;
        }
        public int UpdateTaskDefinition(Task task)
        {
            string qry = @"UPDATE tasks_definition
                   SET name = @name,
                       details = @details,
                       reward_currency = @reward_currency,
                       frequency = @frequency,
                       validation_required = @validation_required,
                       is_active = @is_active
                   WHERE taskID = @taskID";

            var parameters = new Dictionary<string, object>
            {
                ["@name"] = task.Name,
                ["@details"] = task.Description,
                ["@reward_currency"] = task.Reward,
                ["@frequency"] = task.Frequency,
                ["@validation_required"] = task.RequiresValidation,
                ["@is_active"] = task.IsActive,
                ["@taskID"] = task.Id
            };

            int rowsAffected = ExecuteNonQuery(qry, parameters, out _);
            return rowsAffected;
        }
        public void UpdateTaskIsActive(int taskId, bool isActive)
        {
            string qry = "UPDATE tasks_definition SET is_active = @is_active WHERE taskID = @taskID";
            Dictionary<string, object> parameters = new()
            {
                ["@is_active"] = isActive,
                ["@taskID"] = taskId
            };
            int rowsAffected = ExecuteNonQuery(qry, parameters, out _);
            if (rowsAffected != 1)
                throw new Exception("Failed to update task status.");
        }
        //Gets all group task definitions, loaded and user for the Task Library for the Group Owner (TaskLibraryPage
        public List<Task> GetGroupTaskDefinitions(int groupId)
        {
            string qry = "SELECT * FROM tasks_definition WHERE groupID = @groupID AND is_deleted = 0;";
            Dictionary<string, object> parameters = new() { ["groupID"] = groupId };
            var results = ExecuteReader(qry, parameters);

            List<Task> taskDefinitions = new();

            foreach (var row in results)
            {
                Task task = new Task(
                    _id: Convert.ToInt32(row["taskID"]),
                    _name: row["name"].ToString(),
                    _description: row["details"].ToString(),
                    _reward: Convert.ToInt32(row["reward_currency"]),
                    _frequency: (TaskFrequency)Convert.ToInt32(row["frequency"]),
                    _requiresValidation: Convert.ToBoolean(row["validation_required"]),
                    _IsActive: Convert.ToBoolean(row["is_active"]),
                    _groupId: Convert.ToInt32(row["groupID"])
                );
                taskDefinitions.Add(task);
            }
            return taskDefinitions;
        }

        //Sets is_deleted variable to 1 so it's "deleted" (won't show in library). No full delete since task_instances keep history too, and that needs the Task Definition to still exist.
        //This way the task can't be given again by the owner, but can still be fetched for still running and completed tasks.
        //Also: This means an admin can go into the database and restore the task... so that's nice
        public void SoftDeleteTask(int taskId)
        {
            string qry = "UPDATE tasks_definition SET is_deleted = 1 WHERE taskID = @taskID";
            Dictionary<string, object> parameters = new() { ["@taskID"] = taskId };
            int rowsAffected = ExecuteNonQuery(qry, parameters, out _);
            if (rowsAffected != 1)
                throw new Exception("Failed to delete task.");
        }

        public int InsertTaskInstance(TaskInstance task)
        {
            string qry = @"INSERT INTO task_instances(taskID, groupUserID, status, deadline) 
                        VALUES(@taskID, @groupUserID, @status, @deadline) ";
            Dictionary<string, object> parameters = new()
            {
                ["@taskID"] = task.TaskId,
                ["@groupUserID"] = task.MemberId, 
                ["@status"] = task.Status,            
                ["@deadline"] = task.Deadline         
            };
            int rowsAffected = ExecuteNonQuery(qry, parameters, out int taskId);
            if (rowsAffected != 1) //checken of werkelijk toegevoegd
                throw new ArgumentException("Something went wrong with the database");
            return taskId;
        }

        //public TaskInstance GetTaskInstance(int taskInstanceId)
        //{
        //    string qry = "SELECT * FROM task_instances WHERE TaskInstanceID = @TaskInstanceId;";
        //    Dictionary<string, object> parameters = new() { ["@TaskInstanceId"] = taskInstanceId };
        //    var result = ExecuteReader(qry, parameters)[0];
        //    TaskInstance instance = new TaskInstance(
        //            _id: Convert.ToInt32(result["taskID"]),
        //            _name: result["name"].ToString(),
        //            _description: result["details"].ToString(),
        //            _reward: Convert.ToInt32(result["reward_currency"]),
        //            _frequency: (TaskFrequency)Convert.ToInt32(result["frequency"]),
        //            _requiresValidation: Convert.ToBoolean(result["validation_required"]),
        //            _IsActive: Convert.ToBoolean(result["is_active"]),
        //            _groupId: Convert.ToInt32(result["groupID"])
        //        );
        //}

       
        #endregion
    }
}
