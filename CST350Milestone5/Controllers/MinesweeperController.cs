using CST350Milestone5.Models;
using CST350Milestone5.Services;
using CST350Milestone5.Utility;
using Microsoft.AspNetCore.Mvc;

namespace CST350Milestone5.Controllers
{
	public class MinesweeperController : Controller
	{
		const int BOARD_WIDTH = 20, BOARD_HEIGHT = 12;

		const double DIFFICULTY = 0.05;

        private readonly SecurityService _securityService = new SecurityService();
		private readonly IMyLogger _logger;

        static GameBoardModel? gameBoard { get; set; } = null;

		// Dependency injection in constructor
		public MinesweeperController(IMyLogger logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// Return Minesweeper page
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[CustomAuthorization]
		public IActionResult Index()
        {
			if (gameBoard == null) gameBoard = GameService.InitializeBoard(BOARD_WIDTH, BOARD_HEIGHT, DIFFICULTY);

            return View("Index", gameBoard);
		}

		/// <summary>
		/// Forcibly reinitializes the board and returns the Minesweeper page
		/// </summary>
		/// <returns></returns>
		[Route("/Minesweeper/Newgame")]
		public IActionResult NewGame()
		{
            gameBoard = GameService.InitializeBoard(BOARD_WIDTH, BOARD_HEIGHT, DIFFICULTY);

            return View("Index", gameBoard);
        }

		/// <summary>
		/// Tries to load a saved game
		/// </summary>
		/// <param name="boardid">The ID of the saved game to load</param>
		/// <returns>A View containing either the loaded game or a new game</returns>
		[Route("/Minesweeper/Load/{boardid}")]
		public IActionResult LoadGame(int boardid)
		{
            gameBoard = _securityService.LoadGame(boardid);

            if (gameBoard == null) // Save not found
			{
                gameBoard = GameService.InitializeBoard(BOARD_WIDTH, BOARD_HEIGHT, DIFFICULTY);
            }

            return View("Index", gameBoard);
        }

		/// <summary>
		/// Saves the game in progress
		/// </summary>
		/// <returns>True or false if it saved</returns>
		[Route("/Minesweeper/Save")]
		[Produces(typeof(bool))]
		public ActionResult<bool> SaveGame()
		{
			_logger.Info($"Saving game, {(gameBoard == null ? "Null board" : "Good board")}, {(HttpContext.Session.GetInt32("id") == null ? "Null ID" : "Good ID")}");
			if (gameBoard != null)
			{
				int? id = HttpContext.Session.GetInt32("id");
				if (id != null)
					return _securityService.SaveGame(gameBoard, id.Value);
			}

			return false;
        }

		/// <summary>
		/// Handles flagging cells and returns the updated partial view
		/// </summary>
		/// <param name="column"></param>
		/// <param name="row"></param>
		/// <returns></returns>
		[Route("/Minesweeper/Flag")]
		public IActionResult HandleFlag(int column, int row)
		{
			if (gameBoard != null)
            {
                GameService.FlagCell(gameBoard, column, row);
			}

			return PartialView("_GameBoard", gameBoard);
		}

		/// <summary>
		/// Handles visiting cells and returns the updated partial view
		/// </summary>
		/// <param name="column"></param>
		/// <param name="row"></param>
		/// <returns></returns>
		[Route("/Minesweeper/Fill")]
		public IActionResult HandleButtonClick(int column, int row)
		{
			if (gameBoard != null)
			{
                GameService.FloodFill(gameBoard, column, row);

				if (gameBoard.CellGrid[column][row].CellLive)
				{
					// Lost the game because bomb is clicked
					_securityService.DeleteGame(gameBoard.Id); // Remove any saved version of the game, no save-scumming
                    gameBoard = GameService.InitializeBoard(BOARD_WIDTH, BOARD_HEIGHT, DIFFICULTY);
                    // Needs a scoreboard
                    //return RedirectToAction("Index", "Scoreboard");
                    return RedirectToAction("Lose");
                }
				else if (GameService.IsGameComplete(gameBoard))
				{
                    // Won the game no bombs clicked
                    // Handle high score here
                    _securityService.DeleteGame(gameBoard.Id); // Remove any saved version of the game, no duplicate high-scores
                    gameBoard = GameService.InitializeBoard(BOARD_WIDTH, BOARD_HEIGHT, DIFFICULTY);
                    // Needs a scoreboard
                    //return RedirectToAction("Index", "Scoreboard");
                    return RedirectToAction("Win");
				}
			}

            return PartialView("_GameBoard", gameBoard);
        }

		/// <summary>
		/// Returns the victory screen
		/// </summary>
		/// <returns></returns>
        public IActionResult Win()
        {
            return View("Win");
        }

		/// <summary>
		/// Returns the loss screen
		/// </summary>
		/// <returns></returns>
        public IActionResult Lose()
        {
            return View("Lose");
        }

		/// <summary>
		/// Returns the departure screen
		/// </summary>
		/// <returns></returns>
		public IActionResult Exit()
		{
			return View("Exit");
		}

		/// <summary>
		/// Closes the web application
		/// </summary>
		/// <returns></returns>
        public IActionResult Close()
        {
            // Logic to close the application
            System.Environment.Exit(0);
            return new EmptyResult();
        }
    }
}
