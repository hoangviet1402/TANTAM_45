using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BussinessObject.Models.Employee;
using DataAccess;
using DataAccess.Dao.TanTamDao;
using Logger;

namespace BussinessObject.Helper
{
    public static class EmployeeBoHelper
    {
        public class EmployeeCodeValidationResult
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }
            public bool ShouldAutoGenerate { get; set; }
        }

        public static EmployeeCodeValidationResult ValidateEmployeeCode(int companyId, string employeeCode, int excludeEmployeeId = 0)
        {
            var result = new EmployeeCodeValidationResult
            {
                IsValid = true,
                ErrorMessage = string.Empty,
                ShouldAutoGenerate = false
            };

            try
            {
                if (string.IsNullOrWhiteSpace(employeeCode))
                {
                    result.IsValid = false;
                    result.ShouldAutoGenerate = true;
                    result.ErrorMessage = "Mã nhân viên không được để trống.";
                    return result;
                }

                if (employeeCode.Length > 50)
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Mã nhân viên không được vượt quá 50 ký tự.";
                    return result;
                }

                // Sử dụng stored procedure để kiểm tra employee code có tồn tại hay không
                var exists = DaoFactory.Employee.CheckEmployeeCodeExists(companyId, employeeCode.Trim(), excludeEmployeeId);
                if (exists)
                {
                    result.IsValid = false;
                    if (excludeEmployeeId == 0)
                    {
                        result.ShouldAutoGenerate = true;
                        result.ErrorMessage = $"Mã nhân viên '{employeeCode}' đã tồn tại. Hệ thống sẽ tự động tạo mã mới.";
                    }
                    else
                    {
                        result.ErrorMessage = $"Mã nhân viên '{employeeCode}' đã được sử dụng bởi nhân viên khác.";
                    }
                    return result;
                }

                return result;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBoHelper.ValidateEmployeeCode - Error", ex);
                result.IsValid = false;
                result.ErrorMessage = "Lỗi khi kiểm tra mã nhân viên.";
                return result;
            }
        }



        public static string GenerateNextEmployeeCodeForCompany(int companyId)
        {
            try
            {
                var allEmployeeCodes = DaoFactory.Employee.GetAllEmployeeCodes(companyId);
                var lastCode = FindHighestEmployeeCode(allEmployeeCodes);
                if (!string.IsNullOrEmpty(lastCode))
                {
                    return GenerateNextEmployeeCode(lastCode);
                }
                else
                {
                    return "0001";
                }
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBoHelper.GenerateNextEmployeeCodeForCompany - Error", ex);
                return "0001";
            }
        }

        public static string FindHighestEmployeeCode(List<string> employeeCodes)
        {
            if (employeeCodes == null || !employeeCodes.Any())
            {
                return null;
            }

            var validCodes = employeeCodes.Where(code => !string.IsNullOrWhiteSpace(code)).ToList();
            if (!validCodes.Any())
            {
                return null;
            }

            var sortedCodes = validCodes
                .Select(code => new
                {
                    Code = code,
                    NumericPart = ExtractNumericPart(code),
                    CodeLength = code.Length
                })
                .OrderByDescending(item => item.NumericPart)
                .ThenByDescending(item => item.CodeLength)
                .ThenByDescending(item => item.Code)
                .Select(item => item.Code)
                .ToList();

            return sortedCodes.FirstOrDefault();
        }

        public static long ExtractNumericPart(string employeeCode)
        {
            try
            {
                if (string.IsNullOrEmpty(employeeCode))
                {
                    return 0;
                }

                if (!Regex.IsMatch(employeeCode, @"\d"))
                {
                    return 0;
                }

                if (long.TryParse(employeeCode, out long fullNumeric))
                {
                    return fullNumeric;
                }

                var matches = Regex.Matches(employeeCode, @"\d+");
                if (matches.Count > 0)
                {
                    var lastMatch = matches[matches.Count - 1];
                    if (long.TryParse(lastMatch.Value, out long numericPart))
                    {
                        return numericPart;
                    }
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public static string GenerateNextEmployeeCode(string lastCode)
        {
            try
            {
                if (string.IsNullOrEmpty(lastCode))
                {
                    return "0001";
                }

                var matches = Regex.Matches(lastCode, @"\d+");
                if (matches.Count > 0)
                {
                    var lastMatch = matches[matches.Count - 1];
                    var numberPart = lastMatch.Value;
                    var startIndex = lastMatch.Index;

                    if (int.TryParse(numberPart, out int number))
                    {
                        number++;
                        var newNumberPart = number.ToString().PadLeft(numberPart.Length, '0');
                        var prefix = lastCode.Substring(0, startIndex);
                        var suffix = lastCode.Substring(startIndex + numberPart.Length);
                        return $"{prefix}{newNumberPart}{suffix}";
                    }
                }

                return $"{lastCode}0001";
            }
            catch
            {
                return "0001";
            }
        }
    }
}