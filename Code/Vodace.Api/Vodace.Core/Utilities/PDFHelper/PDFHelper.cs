using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Utilities.PDFHelper
{
    public class PDFHelper
    {
        /// <summary>
        /// 使用LibreOffice将Office文档转换为PDF
        /// </summary>
        /// <param name="inputFile">输入文件路径</param>
        /// <param name="outputFile">输出PDF文件路径</param>
        /// <returns>转换是否成功</returns>
        public static bool ConvertToPdfUsingLibreOffice(string inputFile, string outputFile)
        {
            string libreOfficePath = GetLibreOfficePath();
            if (string.IsNullOrEmpty(libreOfficePath))
            {
                throw new FileNotFoundException("not found LibreOffice");
            }

            string outputDir = Path.GetDirectoryName(outputFile);
            string convertArgs = $"--headless --convert-to pdf:writer_pdf_Export --outdir \"{outputDir}\" \"{inputFile}\"";

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = libreOfficePath,
                Arguments = convertArgs,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (Process process = new Process { StartInfo = psi })
            {
                process.Start();

                // 超时机制 - 5分钟
                bool exited = process.WaitForExit(5 * 60 * 1000);

                if (!exited)
                {
                    try
                    {
                        process.Kill();
                    }
                    catch { }
                    return false;
                }

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                // 检查是否转换成功（LibreOffice命令行在成功时会输出类似"convert document.xxx -> document.pdf using filter"的信息）
                bool success = process.ExitCode == 0 && output.Contains("->") && output.Contains(".pdf");

                if (!success)
                {
                    string errorMessage = "error";
                    if (!string.IsNullOrEmpty(error))
                    {
                        errorMessage += $": {error}";
                    }
                    throw new Exception(errorMessage);
                }

                return success;
            }


            //// 查找LibreOffice的可执行文件路径
            //string libreOfficePath = GetLibreOfficePath();
            //if (string.IsNullOrEmpty(libreOfficePath))
            //{
            //    throw new FileNotFoundException("未找到相关转换工具");
            //}

            //// 设置转换参数
            //ProcessStartInfo psi = new ProcessStartInfo
            //{
            //    FileName = libreOfficePath,
            //    Arguments = $"--headless --convert-to pdf --outdir \"{Path.GetDirectoryName(outputFile)}\" \"{inputFile}\"",
            //    UseShellExecute = false,
            //    CreateNoWindow = true,
            //    RedirectStandardOutput = true,
            //    RedirectStandardError = true
            //};

            //using (Process process = new Process { StartInfo = psi })
            //{
            //    process.Start();
            //    string output = process.StandardOutput.ReadToEnd();
            //    string error = process.StandardError.ReadToEnd();
            //    process.WaitForExit();

            //    // 检查是否转换成功（LibreOffice命令行在成功时会输出类似"convert document.xxx -> document.pdf using filter"的信息）
            //    bool success = process.ExitCode == 0 && output.Contains("->") && output.Contains(".pdf");

            //    if (!success)
            //    {
            //        string errorMessage = "转换失败";
            //        if (!string.IsNullOrEmpty(error))
            //        {
            //            errorMessage += $": {error}";
            //        }
            //        throw new Exception(errorMessage);
            //    }

            //    return success;
            //}
        }

        /// <summary>
        /// 获取默认的LibreOffice路径
        /// </summary>
        private static string GetLibreOfficePath()
        {
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                // Windows系统可能的安装路径
                var programFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                var programFilesX86 = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);

                var possiblePaths = new[]
                {
                Path.Combine(programFiles, "LibreOffice", "program", "soffice.exe"),
                Path.Combine(programFilesX86, "LibreOffice", "program", "soffice.exe"),
                Path.Combine(programFiles, "LibreOffice 7", "program", "soffice.exe"),
                Path.Combine(programFilesX86, "LibreOffice 7", "program", "soffice.exe")
            };

                foreach (var path in possiblePaths)
                {
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }
            }
            else if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                // Linux系统可能的安装路径
                var possiblePaths = new[] { "/usr/bin/libreoffice", "/usr/bin/soffice" };

                foreach (var path in possiblePaths)
                {
                    if (File.Exists(path))
                    {
                        return path;
                    }
                }
            }

            return null;
        }
    }
}
