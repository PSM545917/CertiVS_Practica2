using Microsoft.Extensions.Logging;

namespace PatientCodeAPI.Services
{
    public class PatientCodeService : IPatientCodeService
    {
        private readonly ILogger<PatientCodeService> _logger;

        public PatientCodeService(ILogger<PatientCodeService> logger)
        {
            _logger = logger;
        }

        public string GeneratePatientCode(string name, string lastName, string ci)
        {
            _logger.LogInformation("Generating patient code for {Name} {LastName} with CI {CI}", name, lastName, ci);

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastName) || string.IsNullOrWhiteSpace(ci))
            {
                _logger.LogWarning("Invalid input parameters for patient code generation");
                throw new ArgumentException("Name, LastName and CI are required to generate a patient code");
            }

            try
            {
                // Get first letter of name and last name
                char firstLetterName = char.ToUpper(name.Trim()[0]);
                char firstLetterLastName = char.ToUpper(lastName.Trim()[0]);

                // Combine with CI
                string patientCode = $"{firstLetterName}{firstLetterLastName}-{ci}";

                _logger.LogInformation("Generated patient code: {PatientCode}", patientCode);
                return patientCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating patient code");
                throw;
            }
        }
    }
} 