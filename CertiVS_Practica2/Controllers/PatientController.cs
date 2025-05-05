using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLogic.Managers;
using BusinessLogic.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CertiVS_Practica2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientController : ControllerBase
    {
        private readonly PatientManager _patientManager;
        private readonly ILogger<PatientController> _logger;

        public PatientController(PatientManager patientManager, ILogger<PatientController> logger)
        {
            _patientManager = patientManager;
            _logger = logger;
        }

        // DTO classes for API interaction
        public class CreatePatientRequest
        {
            public string Name { get; set; }
            public string LastName { get; set; }
            public string CI { get; set; }
        }

        public class UpdatePatientRequest
        {
            public string Name { get; set; }
            public string LastName { get; set; }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Patient>>> GetAllPatients()
        {
            _logger.LogInformation("Received request to get all patients");
            try
            {
                var patients = await _patientManager.GetAllPatientsAsync();
                return Ok(patients);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all patients");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving patients");
            }
        }

        [HttpGet("{ci}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Patient>> GetPatientByCI(string ci)
        {
            _logger.LogInformation("Received request to get patient with CI: {CI}", ci);
            try
            {
                var patient = await _patientManager.GetPatientByCIAsync(ci);
                if (patient == null)
                {
                    return NotFound("Patient not found");
                }
                return Ok(patient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting patient with CI {CI}", ci);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the patient");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Patient>> CreatePatient([FromBody] CreatePatientRequest request)
        {
            _logger.LogInformation("Received request to create new patient with CI: {CI}", request.CI);

            if (string.IsNullOrWhiteSpace(request.Name) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.CI))
            {
                return BadRequest("Name, LastName and CI are required");
            }

            try
            {
                var newPatient = new Patient
                {
                    Name = request.Name,
                    LastName = request.LastName,
                    CI = request.CI
                };

                var createdPatient = await _patientManager.CreatePatientAsync(newPatient);
                return CreatedAtAction(nameof(GetPatientByCI), new { ci = createdPatient.CI }, createdPatient);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating patient");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the patient");
            }
        }

        [HttpPut("{ci}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Patient>> UpdatePatient(string ci, [FromBody] UpdatePatientRequest request)
        {
            _logger.LogInformation("Received request to update patient with CI: {CI}", ci);

            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.LastName))
            {
                return BadRequest("Name and LastName are required");
            }

            try
            {
                var updatedPatient = await _patientManager.UpdatePatientAsync(ci, request.Name, request.LastName);
                if (updatedPatient == null)
                {
                    return NotFound("Patient not found");
                }
                return Ok(updatedPatient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating patient with CI {CI}", ci);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the patient");
            }
        }

        [HttpDelete("{ci}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeletePatient(string ci)
        {
            _logger.LogInformation("Received request to delete patient with CI: {CI}", ci);

            try
            {
                var result = await _patientManager.DeletePatientAsync(ci);
                if (!result)
                {
                    return NotFound("Patient not found");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting patient with CI {CI}", ci);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the patient");
            }
        }
    }
}
