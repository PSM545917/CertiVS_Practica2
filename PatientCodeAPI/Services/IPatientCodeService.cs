namespace PatientCodeAPI.Services
{
    public interface IPatientCodeService
    {
        string GeneratePatientCode(string name, string lastName, string ci);
    }
} 