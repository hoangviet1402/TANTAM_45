using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BussinessObject.Helper
{
    public static class FileUploadHelper
    {
        // Allowed file extensions
        private static readonly string[] AllowedExtensions = 
        {
            ".doc", ".docx", ".xls", ".xlsx", ".pdf", ".png", ".jpeg", ".jpg"
        };
        
        // Maximum file size in bytes (10MB)
        private const int MaxFileSizeBytes = 10 * 1024 * 1024;
        
        // Maximum number of files per request
        private const int MaxFilesCount = 10;
        
        /// <summary>
        /// Validates uploaded files for comment feature
        /// </summary>
        /// <param name="files">List of uploaded files</param>
        /// <returns>Validation result with error messages if any</returns>
        public static FileValidationResult ValidateCommentFiles(List<HttpPostedFileBase> files)
        {
            var result = new FileValidationResult { IsValid = true, ErrorMessages = new List<string>() };
            
            if (files == null || !files.Any())
            {
                return result; // No files to validate
            }
            
            // Check maximum files count
            if (files.Count > MaxFilesCount)
            {
                result.IsValid = false;
                result.ErrorMessages.Add($"Maximum {MaxFilesCount} files allowed per request. You uploaded {files.Count} files.");
                return result;
            }
            
            foreach (var file in files)
            {
                if (file == null || file.ContentLength == 0)
                {
                    result.IsValid = false;
                    result.ErrorMessages.Add("Empty files are not allowed.");
                    continue;
                }
                
                // Check file size
                if (file.ContentLength > MaxFileSizeBytes)
                {
                    result.IsValid = false;
                    result.ErrorMessages.Add($"File '{file.FileName}' exceeds maximum size of {MaxFileSizeBytes / (1024 * 1024)}MB.");
                }
                
                // Check file extension
                var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
                if (string.IsNullOrEmpty(extension) || !AllowedExtensions.Contains(extension))
                {
                    result.IsValid = false;
                    result.ErrorMessages.Add($"File '{file.FileName}' has unsupported format. Allowed formats: {string.Join(", ", AllowedExtensions)}.");
                }
            }
            
            return result;
        }
        
        /// <summary>
        /// Gets the MIME type based on file extension
        /// </summary>
        /// <param name="fileName">File name with extension</param>
        /// <returns>MIME type string</returns>
        public static string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
            
            switch (extension)
            {
                case ".doc":
                    return "application/msword";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".pdf":
                    return "application/pdf";
                case ".png":
                    return "image/png";
                case ".jpeg":
                case ".jpg":
                    return "image/jpeg";
                default:
                    return "application/octet-stream";
            }
        }
    }
    
    public class FileValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }
}