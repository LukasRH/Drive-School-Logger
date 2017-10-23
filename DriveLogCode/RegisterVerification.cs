﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriveLogCode
{
    public static class RegisterVerification
    {
        public static bool UsernameVerifacation(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            char[] specialChars = { '_', '-', '.' };

            if (specialChars.Contains(input[0]) || specialChars.Contains(input[input.Length - 1]))
                return false;

            for(int i = 0; i < input.Length; i++)
            {
                if (!char.IsLetterOrDigit(input[i]) && !specialChars.Contains(input[i]))
                    return false;
                if (i != 0 && specialChars.Contains(input[i]) && specialChars.Contains(input[i-1]))
                    return false;
            }

            return true;
        }

        public static bool InputOnlyLettersVerification(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                    return false;
            }

            return true;
        }

        public static bool AdressVerification(string adress)
        {
            if (string.IsNullOrEmpty(adress) || !adress.Contains(' '))
                return false;

            string[] adressStrings = adress.Split(' ');
            string street = adressStrings[0];
            string streetNo = adressStrings[1];

            if (adressStrings.Length == 1)
                return false;

            if (!InputOnlyLettersVerification(street))
                return false;

            foreach (char number in streetNo)
            {
                if (!char.IsDigit(number))
                    return false;
            }

            if (adressStrings.Length == 3)
            {
                string floor = adressStrings[2];
                foreach (char c in floor)
                {
                    if (!char.IsLetterOrDigit(c))
                        return false;
                }
            }
            return true;
        }

        public static bool EmailVerification(string input)
        {
            string[] splittedEmail = input.Split('@');
            string localPart;
            string domainPart;

            if (splittedEmail.Length != 2)
                return false;

            else
            {
                localPart = splittedEmail[0];
                domainPart = splittedEmail[1];
            }
            if (!localPart.All(c => char.IsLetterOrDigit(c) || c == '_' || c == '.' || c == '-'))
            {
                return false;
            }
            else if (!domainPart.All(c => char.IsLetterOrDigit(c) || c == '.' || c == '-')
                || domainPart.StartsWith(".")
                || domainPart.StartsWith("-")
                || domainPart.EndsWith("-")
                || domainPart.EndsWith(".")
                || !domainPart.Any(c => c == '.'))
            {
                return false;
            }
            else
                return true;
        }

        public static bool CPRVerification(string input)
        {
            input = input.Replace("-", "");

            if (input.Length != 10)
                return false;

            int[] intArray = input.Select(c => (c - '0')).ToArray();

            int sum = (intArray[0] * 4) + (intArray[1] * 3) + (intArray[2] * 2) + (intArray[3] * 7) + (intArray[4] * 6) + (intArray[5] * 5) +
                (intArray[6] * 4) + (intArray[7] * 3) + (intArray[8] * 2) + intArray[9];

            if (sum % 11 == 0)
                return true;
            else
                return false;
        }

        public static bool PasswordVertification(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;

            if (password.Length < 3) return false;

            foreach (char c in password)
            {
                if (!char.IsLetterOrDigit(c) && !c.Equals('_') && !c.Equals('-'))
                    return false;
            }

            return true;
        }

        public static int PasswordStrength(string password)
        {
            int hasUppercase = 0;
            int hasLowercase = 0;
            int hasDigit = 0;
            int hasSpecialChar = 0;

            foreach(char c in password)
            {
                if (char.IsDigit(c))
                    hasDigit = 2;
                else if (char.IsLower(c))
                    hasLowercase = 2;
                else if (char.IsUpper(c))
                    hasUppercase = 2;
                else if (c.Equals('_') || c.Equals('-'))
                    hasSpecialChar = 2;
            }

            return hasUppercase + hasLowercase + hasDigit + hasSpecialChar + password.Length;
        }

        public static bool PhoneVerifacation(string input)
        {

            if (string.IsNullOrEmpty(input) || input.Length != 8)
                return false;

            foreach (char c in input)
            {
                if (!input.All(char.IsDigit))
                    return false;
            }
            return true;
        }

        public static bool ZipVerifacation(string input)
        {

            if (string.IsNullOrEmpty(input) || input.Length != 4)
                return false;

            foreach (char c in input)
            {
                if (!input.All(char.IsDigit))
                    return false;
            }
            return true;
        }
    }
}

