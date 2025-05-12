using System;

namespace BusinessLogic.Models
{
    public class Patient
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string CI { get; set; } = string.Empty;
        public string BloodGroup { get; set; } = string.Empty;
        public string PatientCode { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Name},{LastName},{CI},{BloodGroup},{PatientCode}";
        }

        public static Patient FromString(string patientString)
        {
            var parts = patientString.Split(',');
            
            // Handle both old format (4 parts) and new format (5 parts with PatientCode)
            if (parts.Length < 4)
            {
                throw new ArgumentException("Invalid patient string format");
            }

            return new Patient
            {
                Name = parts[0].Trim(),
                LastName = parts[1].Trim(),
                CI = parts[2].Trim(),
                BloodGroup = parts[3].Trim(),
                PatientCode = parts.Length > 4 ? parts[4].Trim() : string.Empty
            };
        }
    }
}