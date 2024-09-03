using CST350Milestone5.Controllers;
using CST350Milestone5.Models;
using CST350Milestone5.Services;
using CST350Milestone5.Utility;
using Microsoft.AspNetCore.Mvc;

namespace CST350Milestone1.Controllers
{
    public class LoginController : Controller
    {
        private readonly IMyLogger _logger;

        // Dependency injection in constructor
        public LoginController(IMyLogger logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Allows logging in with AJAX to continue using the login page for loading a game
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns>A Partial View of either the game load screen or the login failure screen</returns>
        [Route("/LoginAJAX")]
        public IActionResult ProcessPartialLogin(string username, string password)
        {
            SecurityService securityService = new SecurityService();

            PlayerLoginModel player = new PlayerLoginModel(0, username, password);

            if (securityService.IsValid(player))
            {
                HttpContext.Session.SetString("username", player.Username);
                HttpContext.Session.SetInt32("id", player.Id);

                _logger.Info($"Successful user sign @ {player.Username}:{"".PadRight(player.Password.Length, '*')}");

                return PartialView("_GameList", player);
            }
            else
            {
                HttpContext.Session.Remove("username");
                HttpContext.Session.Remove("id");

                _logger.Warning($"Failed user sign @ {player.Username}");

                return PartialView("_LoginFailure", player);
            }
        }
    }
}