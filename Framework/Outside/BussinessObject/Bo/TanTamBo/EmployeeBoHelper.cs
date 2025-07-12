using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BussinessObject.Models.Employee;
using DataAccess;
using DataAccess.Dao.TanTamDao;
using Logger;

namespace BussinessObject.Bo.TanTamBo
{
    public static class EmployeeBoHelper
    {
        public class EmployeeCodeValidationResult
        {
            public bool IsValid { get; set; }
            public string ErrorMessage { get; set; }
            public bool ShouldAutoGenerate { get; set; }
        }

        public static EmployeeCodeValidationResult ValidateEmployeeCodeForCreate(int companyId, string employeeCode, IEmployeeDao employeeDao = null)
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

                var dao = employeeDao ?? DaoFactory.Employee;
                var existingCodes = dao.GetAllEmployeeCodes(companyId);
                if (existingCodes != null && existingCodes.Any())
                {
                    var normalizedEmployeeCode = employeeCode.Trim();
                    var duplicateCode = existingCodes.FirstOrDefault(code =>
                        !string.IsNullOrEmpty(code) &&
                        code.Trim().Equals(normalizedEmployeeCode, StringComparison.OrdinalIgnoreCase));

                    if (duplicateCode != null)
                    {
                        result.IsValid = false;
                        result.ShouldAutoGenerate = true;
                        result.ErrorMessage = $"Mã nhân viên '{employeeCode}' đã tồn tại. Hệ thống sẽ tự động tạo mã mới.";
                        return result;
                    }
                }

                if (employeeCode.Length > 50)
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Mã nhân viên không được vượt quá 50 ký tự.";
                    return result;
                }

                return result;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBoHelper.ValidateEmployeeCodeForCreate - Error", ex);
                result.IsValid = false;
                result.ErrorMessage = "Lỗi khi kiểm tra mã nhân viên.";
                return result;
            }
        }

        public static EmployeeCodeValidationResult ValidateEmployeeCodeForUpdate(int companyId, string employeeCode, int excludeEmployeeId)
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
                    result.ErrorMessage = "Mã nhân viên không được để trống.";
                    return result;
                }

                var existingCodes = DaoFactory.Employee.GetAllEmployeeCodes(companyId);
                if (existingCodes != null && existingCodes.Any())
                {
                    var normalizedEmployeeCode = employeeCode.Trim();
                    var duplicateCode = existingCodes.FirstOrDefault(code =>
                        !string.IsNullOrEmpty(code) &&
                        code.Trim().Equals(normalizedEmployeeCode, StringComparison.OrdinalIgnoreCase));

                    //if (duplicateCode != null)
                    //{
                    //    var employeeAccountMaps = DaoFactory.Employee.GetEmployeeAccountMapByCompanyId(companyId);
                    //    var duplicateEmployee = employeeAccountMaps?.FirstOrDefault(emp =>
                    //        !string.IsNullOrEmpty(emp.EmployeeCode) &&
                    //        emp.EmployeeCode.Trim().Equals(normalizedEmployeeCode, StringComparison.OrdinalIgnoreCase) &&
                    //        emp.Id != excludeEmployeeId);
                    //    if (duplicateEmployee != null)
                    //    {
                    //        result.IsValid = false;
                    //        result.ErrorMessage = $"Mã nhân viên '{employeeCode}' đã được sử dụng bởi nhân viên khác.";
                    //        return result;
                    //    }
                    //}
                }

                if (employeeCode.Length > 50)
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Mã nhân viên không được vượt quá 50 ký tự.";
                    return result;
                }

                return result;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBoHelper.ValidateEmployeeCodeForUpdate - Error", ex);
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

        public static List<EmployeeAccountMapValidationDto> GetEmployeeAccountMapForValidation(int companyId, IEmployeeDao employeeDao = null)
        {
            try
            {
                var dao = employeeDao ?? DaoFactory.Employee;
                var entityResults = dao.GetEmployeeAccountMapByCompanyId(companyId);
                
                if (entityResults == null || !entityResults.Any())
                    return new List<EmployeeAccountMapValidationDto>();

                var validationList = new List<EmployeeAccountMapValidationDto>();
                
                foreach (var entity in entityResults)
                {
                    var validationDto = new EmployeeAccountMapValidationDto();
                    validationDto.EmployeeAccountMapId = entity.Id;
                    validationDto.Email = entity.Email;
                    validationDto.Phone = entity.Phone;
                    validationDto.EmployeeCode = entity.AccountId.ToString();
                    
                    validationList.Add(validationDto);
                }
                
                return validationList;
            }
            catch (Exception ex)
            {
                CommonLogger.DefaultLogger.Error("EmployeeBoHelper.GetEmployeeAccountMapForValidation - Error", ex);
                return new List<EmployeeAccountMapValidationDto>();
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

        public static bool CheckDuplicateContactInfo(
            List<EmployeeAccountMapValidationDto> allEmployees,
            string email,
            string phone,
            int excludeEmployeeId,
            out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!string.IsNullOrEmpty(email))
            {
                var duplicateEmail = allEmployees
                    .FirstOrDefault(map => map.Email != null
                        && map.Email.ToLower().Trim() == email.ToLower().Trim()
                        && map.EmployeeAccountMapId != excludeEmployeeId);
                if (duplicateEmail != null)
                {
                    errorMessage = $"Email '{email}' đã được sử dụng bởi nhân viên khác.";
                    return true;
                }
            }
            if (!string.IsNullOrEmpty(phone))
            {
                var duplicatePhone = allEmployees
                    .FirstOrDefault(map => map.Phone != null
                        && map.Phone.Trim() == phone.Trim()
                        && map.EmployeeAccountMapId != excludeEmployeeId);
                if (duplicatePhone != null)
                {
                    errorMessage = $"Số điện thoại '{phone}' đã được sử dụng bởi nhân viên khác.";
                    return true;
                }
            }
            return false;
        }

        public static bool CheckDuplicateEmployeeCode(
            List<EmployeeAccountMapValidationDto> allEmployees,
            string employeeCode,
            int excludeEmployeeId,
            out string errorMessage)
        {
            errorMessage = string.Empty;
            if (!string.IsNullOrEmpty(employeeCode))
            {
                var duplicateCode = allEmployees
                    .FirstOrDefault(map => map.EmployeeCode != null
                        && map.EmployeeCode.Trim() == employeeCode.Trim()
                        && map.EmployeeAccountMapId != excludeEmployeeId);
                if (duplicateCode != null)
                {
                    errorMessage = $"Mã nhân viên '{employeeCode}' đã được sử dụng bởi nhân viên khác.";
                    return true;
                }
            }
            return false;
        }
    }
} 