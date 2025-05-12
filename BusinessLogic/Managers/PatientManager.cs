using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Models;
using Microsoft.Extensions.Logging;
using Services.ExternalServices;

namespace BusinessLogic.Managers
{
    public class PatientManager
    {
        private readonly string _patientsFilePath;
        private readonly ILogger<PatientManager> _logger;
        private readonly PatientCodeService _patientCodeService;

        public PatientManager(
            string filePath, 
            ILogger<PatientManager> logger,
            PatientCodeService patientCodeService)
        {
            _patientsFilePath = filePath;
            _logger = logger;
            _patientCodeService = patientCodeService;

            // Ensure the file exists
            if (!File.Exists(_patientsFilePath))
            {
                var directory = Path.GetDirectoryName(_patientsFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.Create(_patientsFilePath).Close();
                _logger.LogInformation("Created new patients file at {FilePath}", _patientsFilePath);
            }
        }

        public async Task<List<Patient>> GetAllPatientsAsync()
        {
            _logger.LogInformation("Getting all patients");
            var patients = new List<Patient>();

            try
            {
                var lines = await File.ReadAllLinesAsync(_patientsFilePath);
                foreach (var line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        patients.Add(Patient.FromString(line));
                    }
                }

                return patients;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading patients from file");
                throw;
            }
        }

        public async Task<Patient?> GetPatientByCIAsync(string ci)
        {
            _logger.LogInformation("Getting patient with CI: {CI}", ci);
            var patients = await GetAllPatientsAsync();
            var patient = patients.FirstOrDefault(p => p.CI == ci);

            if (patient == null)
            {
                _logger.LogWarning("Patient with CI {CI} not found", ci);
            }

            return patient;
        }

        public async Task<Patient> CreatePatientAsync(Patient patient)
        {
            _logger.LogInformation("Creating new patient with CI: {CI}", patient.CI);

            // Check if patient already exists
            var existingPatient = await GetPatientByCIAsync(patient.CI);
            if (existingPatient != null)
            {
                _logger.LogWarning("Patient with CI {CI} already exists", patient.CI);
                throw new InvalidOperationException($"Patient with CI {patient.CI} already exists");
            }

            // Assign random blood group
            patient.BloodGroup = BloodGroup.GetRandomBloodGroup();

            try
            {
                // Generate patient code from the external service
                patient.PatientCode = await _patientCodeService.GeneratePatientCodeAsync(
                    patient.Name, 
                    patient.LastName, 
                    patient.CI);
                
                _logger.LogInformation("Generated patient code: {PatientCode}", patient.PatientCode);

                // Append to file
                await File.AppendAllTextAsync(_patientsFilePath, $"{patient}\n");
                _logger.LogInformation("Patient created successfully: {Patient}", patient);
                return patient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating patient");
                throw;
            }
        }

        public async Task<Patient?> UpdatePatientAsync(string ci, string name, string lastName)
        {
            _logger.LogInformation("Updating patient with CI: {CI}", ci);

            var patients = await GetAllPatientsAsync();
            var patientIndex = patients.FindIndex(p => p.CI == ci);

            if (patientIndex == -1)
            {
                _logger.LogWarning("Patient with CI {CI} not found for update", ci);
                return null;
            }

            // Update only name and lastname, keep other properties
            var oldPatient = patients[patientIndex];
            patients[patientIndex].Name = name;
            patients[patientIndex].LastName = lastName;

            // If PatientCode is empty or null, generate it
            if (string.IsNullOrEmpty(patients[patientIndex].PatientCode))
            {
                try
                {
                    patients[patientIndex].PatientCode = await _patientCodeService.GeneratePatientCodeAsync(
                        name, 
                        lastName, 
                        ci);
                    
                    _logger.LogInformation("Generated patient code during update: {PatientCode}", patients[patientIndex].PatientCode);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error generating patient code during update");
                    // Continue with the update even if patient code generation fails
                }
            }

            try
            {
                await SaveAllPatientsAsync(patients);
                _logger.LogInformation("Patient updated successfully: {Patient}", patients[patientIndex]);
                return patients[patientIndex];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating patient with CI {CI}", ci);
                throw;
            }
        }

        public async Task<bool> DeletePatientAsync(string ci)
        {
            _logger.LogInformation("Deleting patient with CI: {CI}", ci);

            var patients = await GetAllPatientsAsync();
            var patientToRemove = patients.FirstOrDefault(p => p.CI == ci);

            if (patientToRemove == null)
            {
                _logger.LogWarning("Patient with CI {CI} not found for deletion", ci);
                return false;
            }

            patients.Remove(patientToRemove);

            try
            {
                await SaveAllPatientsAsync(patients);
                _logger.LogInformation("Patient with CI {CI} deleted successfully", ci);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting patient with CI {CI}", ci);
                throw;
            }
        }

        private async Task SaveAllPatientsAsync(List<Patient> patients)
        {
            try
            {
                var lines = patients.Select(p => p.ToString());
                await File.WriteAllLinesAsync(_patientsFilePath, lines);
                _logger.LogInformation("Saved {Count} patients to file", patients.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving patients to file");
                throw;
            }
        }
    }
}