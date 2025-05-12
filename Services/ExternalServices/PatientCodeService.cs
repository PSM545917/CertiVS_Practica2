using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Services.ExternalServices
{
    public class PatientCodeService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PatientCodeService> _logger;
        private readonly string _patientCodeApiUrl;

        public PatientCodeService(
            HttpClient httpClient, 
            IConfiguration configuration, 
            ILogger<PatientCodeService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _patientCodeApiUrl = configuration["ExternalServices:PatientCodeApiUrl"] ?? "https://localhost:7200";
        }

        public async Task<string> GeneratePatientCodeAsync(string name, string lastName, string ci)
        {
            _logger.LogInformation("Requesting patient code generation for {Name} {LastName} with CI {CI}", name, lastName, ci);

            try
            {
                var request = new
                {
                    Name = name,
                    LastName = lastName,
                    CI = ci
                };

                var response = await _httpClient.PostAsJsonAsync($"{_patientCodeApiUrl}/api/PatientCode/generate", request);
                
                response.EnsureSuccessStatusCode();
                
                var result = await response.Content.ReadFromJsonAsync<PatientCodeResponse>();
                
                if (result == null || string.IsNullOrEmpty(result.PatientCode))
                {
                    throw new InvalidOperationException("Received null or empty patient code from API");
                }
                
                _logger.LogInformation("Successfully received patient code: {PatientCode}", result.PatientCode);
                return result.PatientCode;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while getting patient code");
                throw new Exception("Failed to communicate with the Patient Code service. Please try again later.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating patient code");
                throw;
            }
        }

        // DTO class for deserialization
        private class PatientCodeResponse
        {
            public string PatientCode { get; set; } = string.Empty;
        }
    }
} 