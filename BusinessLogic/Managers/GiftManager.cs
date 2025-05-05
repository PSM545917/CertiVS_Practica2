using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Services.ExternalServices;
using Services.Models;

namespace BusinessLogic.Managers
{
    public class GiftManager
    {
        private readonly GiftService _giftService;
        private readonly ILogger<GiftManager> _logger;

        public GiftManager(GiftService giftService, ILogger<GiftManager> logger)
        {
            _giftService = giftService;
            _logger = logger;
        }

        public async Task<List<Gift>> GetGiftsForPatientsAsync()
        {
            _logger.LogInformation("Getting gifts for patients");

            try
            {
                var gifts = await _giftService.GetGiftsAsync();
                _logger.LogInformation("Retrieved {Count} gifts for patients", gifts.Count);
                return gifts;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error retrieving gifts for patients");
                throw;
            }
        }
    }
}
