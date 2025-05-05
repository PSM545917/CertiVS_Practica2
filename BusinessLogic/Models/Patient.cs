using System;

namespace BusinessLogic.Models
{
    public class Patient
    {
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string CI { get; set; } = string.Empty;
        public string BloodGroup { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Name},{LastName},{CI},{BloodGroup}";
        }

        public static Patient FromString(string patientString)
        {
            var parts = patientString.Split(',');
            if (parts.Length != 4)
            {
                throw new ArgumentException("Invalid patient string format");
            }

            return new Patient
            {
                Name = parts[0].Trim(),
                LastName = parts[1].Trim(),
                CI = parts[2].Trim(),
                BloodGroup = parts[3].Trim()
            };
        }
    }
}