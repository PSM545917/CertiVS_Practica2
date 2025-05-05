using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Models;

namespace CertiVS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GiftController : ControllerBase
    {
        private readonly GiftManager _giftManager;
        private readonly ILogger<GiftController> _logger;

        public GiftController(GiftManager giftManager, ILogger<GiftController> logger)
        {
            _giftManager = giftManager;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Gift>>> GetGiftsForPatients()
        {
            _logger.LogInformation("Received request to get gifts for patients");

            try
            {
                var gifts = await _giftManager.GetGiftsForPatientsAsync();
                return Ok(gifts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting gifts for patients");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving gifts");
            }
        }
    }
}