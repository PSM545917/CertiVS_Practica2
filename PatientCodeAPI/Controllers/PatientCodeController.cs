using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PatientCodeAPI.Models;
using PatientCodeAPI.Services;
using System;

namespace PatientCodeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientCodeController : ControllerBase
    {
        private readonly IPatientCodeService _patientCodeService;
        private readonly ILogger<PatientCodeController> _logger;

        public PatientCodeController(IPatientCodeService patientCodeService, ILogger<PatientCodeController> logger)
        {
            _patientCodeService = patientCodeService;
            _logger = logger;
        }

        [HttpPost("generate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PatientCodeResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GeneratePatientCode([FromBody] PatientCodeRequest request)
        {
            _logger.LogInformation("Received request to generate patient code");

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for patient code generation request");
                    return BadRequest(ModelState);
                }

                var patientCode = _patientCodeService.GeneratePatientCode(
                    request.Name, 
                    request.LastName, 
                    request.CI);

                var response = new PatientCodeResponse
                {
                    PatientCode = patientCode
                };

                _logger.LogInformation("Successfully generated patient code: {PatientCode}", patientCode);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Bad request for patient code generation");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating patient code");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "An error occurred while generating the patient code");
            }
        }
    }
} 