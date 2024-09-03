using CST350Milestone5.Models;
using CST350Milestone5.Service;
using NLog;

namespace CST350Milestone5.Services
{
    public class SecurityService
    {
        private readonly SecurityDAO _securityDAO = new SecurityDAO();
        private static readonly Logger logger = LogManager.GetLogger("LoginAppLoggerrule");

        /// <summary>
        /// Calls the data in SecurityDAO class to see if the user logged in properly
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool IsValid(PlayerLoginModel player)
        {
            logger.Info("IsValid method called.");
            logger.Info("Player Username: " + player.Username);
            logger.Info("Player Password: " + player.Password);

            if (String.Compare(player.Username, "anonymous", true) == 0)
            {
                logger.Info("Anonymous login detected.");
                return true;
            }

            bool result = _securityDAO.FindByUserData(player);
            logger.Info("FindByUserData result: " + result);
            return result;
        }

        /// <summary>
        /// Checks with the Security Data Access Object to try to register a player
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool RegisterPlayer(PlayerRegisterModel player)
        {
            logger.Info("User added");
            logger.Info("Player Username: " + player.Username);
            logger.Info("Player Password: " + player.Password);
            logger.Info("Player Email: " + player.Email);
            logger.Info("Player First Name: " + player.FirstName);
            logger.Info("Player Last Name: " + player.LastName);
            logger.Info("Player Sex: " + player.Sex);
            logger.Info("Player Age: " + player.Age);

            if (String.Compare(player.Username, "anonymous", true) == 0)
            {
                return false;
            }

            bool result = _securityDAO.AddByUserData(player);
            return result;
        }

        /// <summary>
        /// Checks with the Security Data Access Object to try to find a list of saved games in the form of DTOs
        /// </summary>
        /// <param name="playerId">The Player ID to match</param>
        /// <returns>A list of all saved games with matching Player ID</returns>
        public List<GameBoardDTO> GetSavedGames(int playerId)
        {
            return _securityDAO.GetSavedGames(playerId);
        }

        /// <summary>
        /// Checks with the Security Data Access Object to try to find a saved GameBoardModel
        /// </summary>
        /// <param name="id">The ID to match</param>
        /// <returns>The matching GameBoardModel, or null if not found</returns>
        public GameBoardModel? LoadGame(int id)
        {
            return _securityDAO.LoadGame(id);
        }

        /// <summary>
        /// Checks with the Security Data Access Object to try to save a GameBoardModel
        /// </summary>
        /// <param name="gameBoardModel">The GameBoardModel to save</param>
        /// <param name="playerId">The Player ID that owns the game</param>
        /// <returns>True if saved, false if no changes</returns>
        public bool SaveGame(GameBoardModel gameBoardModel, int playerId)
        {
            return _securityDAO.SaveGame(gameBoardModel, playerId);
        }

        /// <summary>
        /// Checks with the Security Data Access Object to try to remove a saved GameBoardModel
        /// </summary>
        /// <param name="id">The ID to match</param>
        /// <returns>True if removed, false if no changes</returns>
        public bool DeleteGame(int id)
        {
            return _securityDAO.DeleteGame(id);
        }
    }
}
