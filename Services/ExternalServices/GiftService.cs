using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Services.Models;

namespace Services.ExternalServices
{
    public class GiftService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GiftService> _logger;
        private const string GiftApiUrl = "https://api.restful-api.dev/objects";

        public GiftService(HttpClient httpClient, ILogger<GiftService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Gift>> GetGiftsAsync()
        {
            _logger.LogInformation("Requesting gifts from external API: {Url}", GiftApiUrl);

            try
            {
                var response = await _httpClient.GetAsync(GiftApiUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var gifts = JsonSerializer.Deserialize<List<Gift>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Gift>();

                _logger.LogInformation("Successfully retrieved {Count} gifts", gifts.Count);
                return gifts;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error while fetching gifts: {Message}", ex.Message);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error while processing gifts: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while fetching gifts: {Message}", ex.Message);
                throw;
            }
        }
    }
}