using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Core
{
    public class Day4
    {
        public class Part1
        {
            public int Solve(IEnumerable<string> rawData)
            {
                // 0 = byr
                // 1 = iyr
                // 2 = eyr
                // 3 = hgt
                // 4 = hcl
                // 5 = ecl
                // 6 = pid
                // 7 = cid => in fact we don't need to track this one.
                var passport = new bool[8];
                var validPassports = 0;

                foreach (var line in rawData)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        // Validate previous passport.
                        if (IsValid(passport))
                            validPassports++;

                        // Reset state.
                        for (var i = 0; i < passport.Length; i++)
                            passport[i] = false;
                    }
                    else
                    {
                        var rawFields = line.Split(' ');
                        var fields = rawFields
                            .Select(x => x.Split(':'))
                            .Select(x => (fieldName: x[0], fieldValue: x[1]));

                        foreach ((var fieldName, var fieldValue) in fields)
                        {
                            var fieldId = GetFieldId(fieldName);
                            passport[fieldId] = IsValid(fieldId, fieldValue);
                        }
                    }
                }

                // Check last password.
                if (IsValid(passport))
                    validPassports++;

                return validPassports;
            }

            private static int GetFieldId(string name)
            {
                return name switch
                {
                    "byr" => 0,
                    "iyr" => 1,
                    "eyr" => 2,
                    "hgt" => 3,
                    "hcl" => 4,
                    "ecl" => 5,
                    "pid" => 6,
                    "cid" => 7,
                    _ => -1
                };
            }

            private static bool IsValid(bool[] passport)
            {
                // Valid of cid is not relevant.
                var passportSummary = passport.Take(passport.Length - 1);

                return !passportSummary.Contains(false);
            }

            private static bool IsValid(int fieldId, string fieldValue)
            {
                return fieldId switch
                {
                    0 => int.TryParse(fieldValue, out var bday) && bday >= 1920 && bday <= 2002,
                    1 => int.TryParse(fieldValue, out var issue) && issue >= 2010 && issue <= 2020,
                    2 => int.TryParse(fieldValue, out var exp) && exp >= 2020 && exp <= 2030,
                    3 => IsValidHeight(fieldValue),
                    4 => IsValidHairColor(fieldValue),
                    5 => IsValidEyeColor(fieldValue),
                    6 => IsValidPassportId(fieldValue),
                    7 => true,
                    _ => false
                };
            }
            private static bool IsValidHeight(string heightString)
            {
                if (!int.TryParse(heightString.Substring(0, heightString.Length - 2), out var height))
                    return false;

                return heightString.EndsWith("cm")
                    ? height >= 150 && height <= 193
                    : height >= 59 && height <= 76;
            }

            private static bool IsValidHairColor(string input)
            {
                var r = new Regex(@"^#[a-z0-9]{6}$");
                var isvalid = r.Match(input).Success;
                return isvalid;
            }

            private static bool IsValidEyeColor(string input)
            {
                return string.Equals("amb", input) ||
                       string.Equals("blu", input) ||
                       string.Equals("brn", input) ||
                       string.Equals("gry", input) ||
                       string.Equals("grn", input) ||
                       string.Equals("hzl", input) ||
                       string.Equals("oth", input);
            }

            private static bool IsValidPassportId(string input)
            {
                var r = new Regex(@"^[0-9]{9}$");
                var isvalid = r.Match(input).Success;
                return isvalid;
            }
        }

        public class Part2
        {
            public void Solve(object input)
            {

            }
        }
    }
    // byr, iyr, eyr, hgt, hcl, ecl, pid, cid
    // field separator: nl or space
    // field in random order
    // only cid missing => valid
}
