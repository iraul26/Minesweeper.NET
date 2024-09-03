using CST350Milestone5.Models;
using CST350Milestone5.Services;
using CST350Milestone5.Utility;
using Microsoft.AspNetCore.Mvc;

namespace CST350Milestone5.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IMyLogger _logger;
        private readonly SecurityService _securityService = new SecurityService();

        // Dependency injection in constructor
        public RegistrationController(IMyLogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Default index page for the registration controller
        /// </summary>
        /// <returns>The registration index page</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Attempts to register a player using a PlayerRegisterModel
        /// Players using an existing username or email will not be registered
        /// </summary>
        /// <param name="player">The PlayerRegisterModel submitted from the Registration Index</param>
        /// <returns>Registration Success/Failure pages depending on the result</returns>
        public IActionResult ProcessRegistration(PlayerRegisterModel player)
        {
            if (_securityService.RegisterPlayer(player))
            {
                _logger.Info($"Successful user registration @ {player.Username}");

                return View("RegistrationSuccess", player);
            }
            else
            {
                _logger.Warning($"Failed user registration @ {player.Username}");

                return View("RegistrationFailure", player);
            }
        }
    }
}
