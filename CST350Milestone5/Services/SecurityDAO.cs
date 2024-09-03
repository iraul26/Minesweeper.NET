using CST350Milestone5.Models;
using System.Data;
using System.Data.SqlClient;

namespace CST350Milestone5.Service
{
    public class SecurityDAO
    {
        //conection string to database
        string connectionString = @"Data Source = (localdb)\MSSQLLocalDB;Initial Catalog = MinesweeperPlayers;";


        /// <summary>
        /// Attempts to find a user with a matching username and password for logging in
        /// </summary>
        /// <param name="player">The PlayerLoginModel submitted from the Login index page</param>
        /// <returns>True if the player was found and their password is correct</returns>
        public bool FindByUserData(PlayerLoginModel player)
        {
            bool success = false;

            string sqlStatement = "SELECT * FROM dbo.MinesweeperPlayers WHERE username = @username AND password = @password";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(sqlStatement, connection);

                command.Parameters.Add("@username", System.Data.SqlDbType.VarChar, 40).Value = player.Username;
                command.Parameters.Add("@password", System.Data.SqlDbType.VarChar, 40).Value = player.Password;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        success = true;
                        reader.Read();
                        player.Id = reader.GetInt32(0);
                    }
                    reader.Close();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return success;
            }
        }

        /// <summary>
        /// Attempts to register the player using the given PlayerRegisterModel
        /// Checks the SQL Database for existing users with the same username or email
        /// Inserts unique players into the database
        /// </summary>
        /// <param name="player">The PlayerRegisterModel submitted from the Registration index page</param>
        /// <returns>True if the player was successfully registered</returns>
        public bool AddByUserData(PlayerRegisterModel player)
        {
            bool success = false;

            string sqlStatementCheck = "SELECT * FROM dbo.MinesweeperPlayers WHERE username = @username OR email = @email",
                   sqlStatementInsert = $"INSERT INTO dbo.MinesweeperPlayers (FirstName, LastName, Sex, Age, State, Email, Username, Password) VALUES (@first, @last, @sex, @age, @state, @email, @username, @password)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand(sqlStatementCheck, connection);

                    command.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = player.Username;
                    command.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = player.Email;

                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        reader.Close();
                        command = new SqlCommand(sqlStatementInsert, connection);
                        command.Parameters.Add("@first", SqlDbType.VarChar, 50).Value = player.FirstName;
                        command.Parameters.Add("@last", SqlDbType.VarChar, 50).Value = player.LastName;
                        command.Parameters.Add("@sex", SqlDbType.VarChar, 50).Value = player.Sex;
                        command.Parameters.Add("@age", SqlDbType.VarChar, 50).Value = player.Age;
                        command.Parameters.Add("@state", SqlDbType.VarChar, 50).Value = player.State;
                        command.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = player.Email;
                        command.Parameters.Add("@username", SqlDbType.VarChar, 50).Value = player.Username;
                        command.Parameters.Add("@password", SqlDbType.VarChar, 50).Value = player.Password;
                        success = command.ExecuteNonQuery() > 0;
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return success;
        }

        /// <summary>
        /// Finds all saved games with the matching Player ID
        /// </summary>
        /// <param name="playerId">The Player ID to match, or 0 for all games</param>
        /// <returns>A list of all matching saved games</returns>
        public List<GameBoardDTO> GetSavedGames(int playerId)
        {
            List<GameBoardDTO> gameBoardModelList = new List<GameBoardDTO>();

            string sqlStatement = "SELECT * FROM dbo.MinesweeperSavedGames WHERE playerid = @playerid";

            if (playerId <= 0)
                sqlStatement = "SELECT * FROM dbo.MinesweeperSavedGames";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                command.Parameters.Add("@playerid", System.Data.SqlDbType.Int).Value = playerId;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        gameBoardModelList.Add(new GameBoardDTO(reader.GetInt32(0), reader.GetDateTime(2)));
                    }
                    reader.Close();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return gameBoardModelList;
        }

        /// <summary>
        /// Tries to find a saved game with the matching ID
        /// </summary>
        /// <param name="id">The ID to match</param>
        /// <returns>The GameBoardDTO if found, null if not found</returns>
        public GameBoardModel? LoadGame(int id)
        {
            GameBoardModel? gameBoardModel = null;

            string sqlStatement = "SELECT * FROM dbo.MinesweeperSavedGames WHERE id = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);

                command.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        gameBoardModel = GameBoardModel.Deserialize(reader.GetString(3));
                        if (gameBoardModel != null)
                            gameBoardModel.Id = reader.GetInt32(0);
                    }
                    reader.Close();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return gameBoardModel;
        }

        /// <summary>
        /// Adds or updates a saved game
        /// </summary>
        /// <param name="gameBoardModel">The Game Board to save</param>
        /// <param name="playerId">The Player ID that owns this game</param>
        /// <returns>True if saved, false if no changes</returns>
        public bool SaveGame(GameBoardModel gameBoardModel, int playerId)
        {
            bool success = false, // Returned, was any data changed
                 insertEntry = true; // Does the entry need to be inserted as a new row

            string sqlStatement;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    SqlCommand command;
                    SqlDataReader reader;

                    if (gameBoardModel.Id > 0)
                    {
                        sqlStatement = "SELECT * FROM dbo.MinesweeperSavedGames WHERE id = @id AND playerid = @playerid";
                        command = new SqlCommand(sqlStatement, connection);
                        command.Parameters.Add("@id", SqlDbType.Int).Value = gameBoardModel.Id;
                        command.Parameters.Add("@playerid", SqlDbType.Int).Value = playerId;

                        reader = command.ExecuteReader();
                        if (reader.HasRows)
                            insertEntry = false;

                        reader.Close();
                    }

                    if (insertEntry) // Insert new save
                    {
                        DateTime saveTime = DateTime.Now;

                        sqlStatement = "INSERT INTO dbo.MinesweeperSavedGames (PlayerId, Date, SaveData) VALUES (@playerid, @date, @savedata)";
                        command = new SqlCommand(sqlStatement, connection);
                        command.Parameters.Add("@playerid", SqlDbType.Int).Value = playerId;
                        command.Parameters.Add("@date", SqlDbType.DateTime).Value = saveTime;
                        command.Parameters.Add("@savedata", SqlDbType.VarChar, int.MaxValue).Value = gameBoardModel.Serialize();

                        success = command.ExecuteNonQuery() > 0;

                        if (success) // Update existing board with new ID
                        {
                            sqlStatement = "SELECT * FROM dbo.MinesweeperSavedGames WHERE playerid = @playerid AND date=@date";
                            command = new SqlCommand(sqlStatement, connection);
                            command.Parameters.Add("@playerid", SqlDbType.Int).Value = playerId;
                            command.Parameters.Add("@date", SqlDbType.DateTime).Value = saveTime;

                            reader = command.ExecuteReader();
                            if (reader.Read())
                                gameBoardModel.Id = reader.GetInt32(0);

                            reader.Close();
                        }
                    }
                    else // Update existing save
                    {
                        sqlStatement = "UPDATE dbo.MinesweeperSavedGames SET date = @date, savedata = @savedata WHERE id = @id";
                        command = new SqlCommand(sqlStatement, connection);
                        command.Parameters.Add("@date", SqlDbType.DateTime).Value = DateTime.Now;
                        command.Parameters.Add("@savedata", SqlDbType.VarChar, int.MaxValue).Value = gameBoardModel.Serialize();
                        command.Parameters.Add("@id", SqlDbType.Int).Value = gameBoardModel.Id;

                        success = command.ExecuteNonQuery() > 0;
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return success;
        }

        /// <summary>
        /// Removes a saved game by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteGame(int id)
        {
            if (id <= 0) return false;

            bool success = false;

            string sqlStatement = "DELETE FROM dbo.MinesweeperSavedGames WHERE id = @id";
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(sqlStatement, connection);
                command.Parameters.Add("@id", SqlDbType.Int).Value = id;

                try
                {
                    connection.Open();
                    success = command.ExecuteNonQuery() > 0;
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return success;
        }
    }
}
