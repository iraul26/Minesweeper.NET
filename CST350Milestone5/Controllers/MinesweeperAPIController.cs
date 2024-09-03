using CST350Milestone5.Models;
using CST350Milestone5.Services;
using Microsoft.AspNetCore.Mvc;

namespace CST350Milestone5.Controllers
{
    [ApiController]
    public class MinesweeperAPIController : ControllerBase
    {
        private readonly SecurityService _securityService = new SecurityService();

        [HttpGet("/api/showSavedGames")]
        [Produces(typeof(List<GameBoardDTO>))]
        public ActionResult<IEnumerable<GameBoardDTO>> FindSavedGameList()
        {
            List<GameBoardDTO> gameBoardDTOs = _securityService.GetSavedGames(0);

            return gameBoardDTOs;
        }

        [HttpGet("/api/showSavedGames/{id}")]
        [Produces(typeof(GameBoardModel))]
        public ActionResult<GameBoardModel> FindSavedGame(int id)
        {
            GameBoardModel? gameBoardModel = _securityService.LoadGame(id);

            if (gameBoardModel != null)
                return gameBoardModel;

            return NoContent();
        }

        [HttpGet("/api/deleteOneGame/{id}")]
        [Produces(typeof(List<GameBoardDTO>))]
        public ActionResult<IEnumerable<GameBoardDTO>> DeleteOneGame(int id)
        {
            _securityService.DeleteGame(id);

            List<GameBoardDTO> gameBoardDTOs = _securityService.GetSavedGames(0);

            return gameBoardDTOs;
        }
    }
}
