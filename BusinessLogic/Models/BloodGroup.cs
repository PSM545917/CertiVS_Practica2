using System;

namespace BusinessLogic.Models
{
    public static class BloodGroup
    {
        private static readonly string[] ValidGroups = { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };

        public static string GetRandomBloodGroup()
        {
            Random random = new Random();
            int index = random.Next(ValidGroups.Length);
            return ValidGroups[index];
        }
    }
}