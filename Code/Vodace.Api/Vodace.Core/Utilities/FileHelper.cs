using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Vodace.Core.Extensions;
using Vodace.Core.Utilities.Log4Net;

namespace Vodace.Core.Utilities
{
    public class FileHelper
    {
        private static object _filePathObj = new object();

        #region  -- vol自带 --

        /// <summary>
        /// 通过迭代器读取平面文件行内容(必须是带有\r\n换行的文件,百万行以上的内容读取效率存在问题,适用于100M左右文件，行100W内，超出的会有卡顿)
        /// </summary>
        /// <param name="fullPath">文件全路径</param>
        /// <param name="page">分页页数</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="seekEnd"> 是否最后一行向前读取,默认从前向后读取</param>
        /// <returns></returns>
        public static IEnumerable<string> ReadPageLine(string fullPath, int page, int pageSize, bool seekEnd = false)
        {
            if (page <= 0)
            {
                page = 1;
            }
            fullPath = fullPath.ReplacePath();
            var lines = File.ReadLines(fullPath, Encoding.UTF8);
            if (seekEnd)
            {
                int lineCount = lines.Count();
                int linPageCount = (int)Math.Ceiling(lineCount / (pageSize * 1.00));
                //超过总页数，不处理
                if (page > linPageCount)
                {
                    page = 0;
                    pageSize = 0;
                }
                else if (page == linPageCount)//最后一页，取最后一页剩下所有的行
                {
                    pageSize = lineCount - (page - 1) * pageSize;
                    if (page == 1)
                    {
                        page = 0;
                    }
                    else
                    {
                        page = lines.Count() - page * pageSize;
                    }
                }
                else
                {
                    page = lines.Count() - page * pageSize;
                }
            }
            else
            {
                page = (page - 1) * pageSize;
            }
            lines = lines.Skip(page).Take(pageSize);

            var enumerator = lines.GetEnumerator();
            int count = 1;
            while (enumerator.MoveNext() || count <= pageSize)
            {
                yield return enumerator.Current;
                count++;
            }
            enumerator.Dispose();
        }
        public static bool FileExists(string path)
        {
            return File.Exists(path.ReplacePath());
        }

        public static string GetCurrentDownLoadPath()
        {
            return ("Download\\").MapPath();
        }

        public static bool DirectoryExists(string path)
        {
            return Directory.Exists(path.ReplacePath());
        }

        public static string Read_File(string fullpath, string filename, string suffix)
        {
            return ReadFile((fullpath + "\\" + filename + suffix).MapPath());
        }
        public static string ReadFile(string fullName)
        {
            //  Encoding code = Encoding.GetEncoding(); //Encoding.GetEncoding("gb2312");
            string temp = fullName.MapPath().ReplacePath();
            string str = "";
            if (!File.Exists(temp))
            {
                return str;
            }
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(temp);
                str = sr.ReadToEnd(); // 读取文件
            }
            catch { }
            sr?.Close();
            sr?.Dispose();
            return str;
        }

        /// <summary>
        /// 取后缀名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>.gif|.html格式</returns>
        public static string GetPostfixStr(string filename)
        {
            int start = filename.LastIndexOf(".");
            int length = filename.Length;
            string postfix = filename.Substring(start, length - start);
            return postfix;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">路径 </param>
        /// <param name="fileName">文件名</param>
        /// <param name="content">写入的内容</param>
        /// <param name="appendToLast">是否将内容添加到未尾,默认不添加</param>
        public static void WriteFile(string path, string fileName, string content, bool appendToLast = false)
        {
            path = path.ReplacePath();
            fileName = fileName.ReplacePath();
            if (!Directory.Exists(path))//如果不存在就创建file文件夹
                Directory.CreateDirectory(path);

            using (FileStream stream = File.Open(path + fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                byte[] by = Encoding.Default.GetBytes(content);
                if (appendToLast)
                {
                    stream.Position = stream.Length;
                }
                else
                {
                    stream.SetLength(0);
                }
                stream.Write(by, 0, by.Length);
            }
        }

        /// <summary>
        /// 追加文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="strings">内容</param>
        public static void FileAdd(string Path, string strings)
        {
            StreamWriter sw = File.AppendText(Path.ReplacePath());
            sw.Write(strings);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }

        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="OrignFile">原始文件</param>
        /// <param name="NewFile">新文件路径</param>
        public static void FileCoppy(string OrignFile, string NewFile)
        {
            File.Copy(OrignFile.ReplacePath(), NewFile.ReplacePath(), true);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="Path">路径</param>
        public static void FileDel(string Path)
        {
            File.Delete(Path.ReplacePath());
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="OrignFile">原始路径</param>
        /// <param name="NewFile">新路径</param>
        public static void FileMove(string OrignFile, string NewFile)
        {
            File.Move(OrignFile.ReplacePath(), NewFile.ReplacePath());
        }

        /// <summary>
        /// 在当前目录下创建目录
        /// </summary>
        /// <param name="OrignFolder">当前目录</param>
        /// <param name="NewFloder">新目录</param>
        public static void FolderCreate(string OrignFolder, string NewFloder)
        {
            Directory.SetCurrentDirectory(OrignFolder.ReplacePath());
            Directory.CreateDirectory(NewFloder.ReplacePath());
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="Path"></param>
        public static void FolderCreate(string Path)
        {
            // 判断目标目录是否存在如果不存在则新建之
            if (!Directory.Exists(Path.ReplacePath()))
                Directory.CreateDirectory(Path.ReplacePath());
        }

        public static void FileCreate(string Path)
        {
            FileInfo CreateFile = new FileInfo(Path.ReplacePath()); //创建文件 
            if (!CreateFile.Exists)
            {
                FileStream FS = CreateFile.Create();
                FS.Close();
            }
        }
        /// <summary>
        /// 递归删除文件夹目录及文件
        /// </summary>
        /// <param name="dir"></param>  
        /// <returns></returns>
        public static void DeleteFolder(string dir)
        {
            dir = dir.ReplacePath();
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //直接删除其中的文件                        
                    else
                        DeleteFolder(d); //递归删除子文件夹 
                }
                Directory.Delete(dir, true); //删除已空文件夹                 
            }
        }

        /// <summary>
        /// 指定文件夹下面的所有内容copy到目标文件夹下面
        /// </summary>
        /// <param name="srcPath">原始路径</param>
        /// <param name="aimPath">目标文件夹</param>
        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                aimPath = aimPath.ReplacePath();
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                //如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                //string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath.ReplacePath());
                //遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    //先当作目录处理如果存在这个目录就递归Copy该目录下面的文件

                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    //否则直接Copy文件
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.ToString());
            }
        }

        public static async Task<bool> CopyFile(string sourceFile, string destinationFile,int bufferSize = 81920) 
        {
            try
            {
                if (!File.Exists(sourceFile))
                    throw new FileNotFoundException($"源文件不存在: {sourceFile}");

                string destinationDirectory = Path.GetDirectoryName(destinationFile);
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
                using (FileStream destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write))
                {
                    await sourceStream.CopyToAsync(destinationStream, bufferSize);
                }
                Log4NetHelper.Info($"文件复制成功: {sourceFile} -> {destinationFile}");
                return true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error($"文件复制失败: {ex.Message}");
                return false;
            }
        }

        public static bool MoveFile(string sourceFile, string destinationFile, bool overwrite = true)
        {
            try
            {
                // 参数验证
                if (string.IsNullOrWhiteSpace(sourceFile))
                    throw new ArgumentException("源文件路径不能为空");

                if (string.IsNullOrWhiteSpace(destinationFile))
                    throw new ArgumentException("目标文件路径不能为空");

                // 检查源文件是否存在
                if (!File.Exists(sourceFile))
                    throw new FileNotFoundException($"源文件不存在: {sourceFile}");

                // 确保目标目录存在
                string destinationDirectory = Path.GetDirectoryName(destinationFile);
                if (!Directory.Exists(destinationDirectory))
                {
                    Directory.CreateDirectory(destinationDirectory);
                }

                // 如果目标文件已存在且不允许覆盖，则抛出异常
                if (File.Exists(destinationFile) && !overwrite)
                {
                    throw new IOException($"目标文件已存在: {destinationFile}");
                }

                // 如果目标文件已存在且允许覆盖，先删除目标文件
                if (File.Exists(destinationFile) && overwrite)
                {
                    File.Delete(destinationFile);
                }

                // 执行文件移动
                File.Move(sourceFile, destinationFile);

                Console.WriteLine($"文件移动成功: {sourceFile} -> {destinationFile}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"文件移动失败: {ex.Message}");
                return false;
            }
        }
        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <returns></returns>
        public static long GetDirectoryLength(string dirPath)
        {
            dirPath = dirPath.ReplacePath();
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0;
            DirectoryInfo di = new DirectoryInfo(dirPath);
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }

        /// <summary>
        /// 获取指定文件详细属性
        /// </summary>
        /// <param name="filePath">文件详细路径</param>
        /// <returns></returns>
        public static string GetFileAttibe(string filePath)
        {
            string str = "";
            filePath = filePath.ReplacePath();
            System.IO.FileInfo objFI = new System.IO.FileInfo(filePath);
            str += "详细路径:" + objFI.FullName + "<br>文件名称:" + objFI.Name + "<br>文件长度:" + objFI.Length.ToString() + "字节<br>创建时间" + objFI.CreationTime.ToString() + "<br>最后访问时间:" + objFI.LastAccessTime.ToString() + "<br>修改时间:" + objFI.LastWriteTime.ToString() + "<br>所在目录:" + objFI.DirectoryName + "<br>扩展名:" + objFI.Extension;
            return str;
        }


        #endregion

        #region  -- Island写 --


        /// <summary>
        /// 读取指定文件夹当前目录下的所有文件和文件夹，进行排序，并获取文件类型及大小
        /// </summary>
        /// <param name="directoryPath">要扫描的文件夹路径</param>
        /// <returns>包含文件和文件夹信息的列表</returns>
        public List<FileSystemItem> GetDirectoryContents(string directoryPath)
        {
            List<FileSystemItem> items = new List<FileSystemItem>();

            try
            {
                // 验证目录是否存在
                if (!Directory.Exists(directoryPath))
                {
                    throw new DirectoryNotFoundException($"The specified directory does not exist: {directoryPath}");
                }

                // 获取当前目录下的所有文件夹并按名称排序
                var directories = Directory.GetDirectories(directoryPath)
                                          .OrderBy(d => Path.GetFileName(d))
                                          .ToList();

                // 获取当前目录下的所有文件并按名称排序
                var files = Directory.GetFiles(directoryPath)
                                    .OrderBy(f => Path.GetFileName(f))
                                    .ToList();

                // 先添加文件夹（类型为"文件夹"，大小为0）
                foreach (var dir in directories)
                {
                    items.Add(new FileSystemItem
                    {
                        Name = Path.GetFileName(dir),
                        FullPath = dir,
                        IsDirectory = true,
                        FileSize = 0,
                        FormattedSize = "-",
                        FileType = "Folder"
                    });
                }

                // 再添加文件（包含文件大小和类型）
                foreach (var file in files)
                {
                    try
                    {
                        FileInfo fileInfo = new FileInfo(file);
                        string fileExtension = Path.GetExtension(file).ToUpper();
                        string fileType = GetFileTypeDescription(fileExtension);

                        items.Add(new FileSystemItem
                        {
                            Name = Path.GetFileName(file),
                            FullPath = file,
                            IsDirectory = false,
                            FileSize = fileInfo.Length,
                            FormattedSize = FormatFileSize(fileInfo.Length),
                            FileType = fileType
                        });
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // 处理无访问权限的文件
                        items.Add(new FileSystemItem
                        {
                            Name = Path.GetFileName(file),
                            FullPath = file,
                            IsDirectory = false,
                            FileSize = 0,
                            FormattedSize = "No access permission",
                            FileType = "Unknown"
                        });
                    }
                    catch (Exception ex)
                    {
                        // 处理其他错误
                        items.Add(new FileSystemItem
                        {
                            Name = Path.GetFileName(file),
                            FullPath = file,
                            IsDirectory = false,
                            FileSize = 0,
                            FormattedSize = "Error",
                            FileType = ex.Message
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常信息
                Console.WriteLine($"扫描目录时发生错误: {ex.Message}");
            }

            return items;
        }


        /// <summary>
        /// 获取文件类型的描述信息
        /// </summary>
        public string GetFileTypeDescription(string extension)
        {
            // 简单的文件类型映射，可以根据需要扩展
            //var fileTypes = new Dictionary<string, string>
            //{
            //    { ".DOC", "Word文档" },
            //    { ".DOCX", "Word文档" },
            //    { ".XLS", "Excel表格" },
            //    { ".XLSX", "Excel表格" },
            //    { ".PPT", "PowerPoint演示文稿" },
            //    { ".PPTX", "PowerPoint演示文稿" },
            //    { ".PDF", "PDF文档" },
            //    { ".TXT", "文本文件" },
            //    { ".JPG", "JPEG图片" },
            //    { ".JPEG", "JPEG图片" },
            //    { ".PNG", "PNG图片" },
            //    { ".GIF", "GIF图片" },
            //    { ".BMP", "位图文件" },
            //    { ".ZIP", "ZIP压缩文件" },
            //    { ".RAR", "RAR压缩文件" },
            //    { ".7Z", "7Z压缩文件" },
            //    { ".EXE", "可执行文件" },
            //    { ".DLL", "动态链接库" },
            //    { ".CS", "C#源代码文件" },
            //    { ".JS", "JavaScript文件" },
            //    { ".HTML", "HTML文件" },
            //    { ".CSS", "CSS样式文件" },
            //    { ".JSON", "JSON数据文件" },
            //    { ".XML", "XML文件" }
            //};

            var fileTypes = new Dictionary<string, string>
            {
                { ".DOC", "doc" },
                { ".DOCX", "docx" },
                { ".XLS", "xls" },
                { ".XLSX", "xlsx" },
                { ".PPT", "PowerPoint" },
                { ".PPTX", "PowerPoint" },
                { ".PDF", "pdf" },
                { ".TXT", "txt" },
                { ".JPG", "jpg" },
                { ".JPEG", "jpeg" },
                { ".PNG", "png" },
                { ".GIF", "gif" },
                { ".BMP", "bmp" },
                { ".ZIP", "zip" },
                { ".RAR", "rar" },
                { ".7Z", "7z" },
                { ".EXE", "exe" },
                { ".DLL", "dll" },
                { ".CS", "c#" },
                { ".JS", "javascript" },
                { ".HTML", "html" },
                { ".CSS", "css" },
                { ".JSON", "json" },
                { ".XML", "xml" }
            };


            if (fileTypes.TryGetValue(extension, out string description))
            {
                return description;
            }

            return "Unknown file type";
        }

        /// <summary>
        /// 将文件大小格式化为KB/MB/GB的辅助方法
        /// </summary>
        public string FormatFileSize(long bytes)
        {
            string[] sizes = { "byte", "kb", "mb", "gb", "tb" };
            double size = bytes;
            int unitIndex = 0;

            while (size >= 1024 && unitIndex < sizes.Length - 1)
            {
                size /= 1024;
                unitIndex++;
            }

            return string.Format("{0:F2} {1}", size, sizes[unitIndex]);
        }

        #endregion


        /// <summary>
        /// 在同一目录下生成不重复的文件名：若存在同名则在文件名后追加流水号（例如：name (1).ext）。
        /// </summary>
        /// <param name="directoryPath">目录路径</param>
        /// <param name="fileName">目标文件名（含扩展名）</param>
        /// <param name="useFullWidthParentheses">是否使用全角括号（中文括号），默认否</param>
        /// <returns>不重复的文件名</returns>
        public static string EnsureUniqueFileName(string directoryPath, string fileName, bool useFullWidthParentheses = false)
        {
            if (string.IsNullOrWhiteSpace(fileName)) return fileName;

            var dir = (directoryPath ?? string.Empty).ReplacePath();
            var ext = Path.GetExtension(fileName);
            var name = Path.GetFileNameWithoutExtension(fileName);

            // 若原始文件名已经包含 (n) 或 （n），去除末尾编号并从 n+1 开始
            var asciiMatch = Regex.Match(name, @"\s*\((\d+)\)$");
            var fullWidthMatch = Regex.Match(name, @"\s*（(\d+)）$");

            int startNo = 1;
            if (asciiMatch.Success)
            {
                if (int.TryParse(asciiMatch.Groups[1].Value, out var n))
                {
                    startNo = Math.Max(1, n + 1);
                    name = name.Substring(0, name.Length - asciiMatch.Value.Length).TrimEnd();
                }
            }
            else if (fullWidthMatch.Success)
            {
                if (int.TryParse(fullWidthMatch.Groups[1].Value, out var n))
                {
                    startNo = Math.Max(1, n + 1);
                    name = name.Substring(0, name.Length - fullWidthMatch.Value.Length).TrimEnd();
                }
            }

            string candidate = name + ext;
            string full = Path.Combine(dir, candidate).ReplacePath();
            if (!File.Exists(full)) return candidate;

            // 存在同名文件，追加流水号
            var parenFmt = useFullWidthParentheses ? "（{0}）" : " ({0})";
            int i = startNo;
            while (true)
            {
                candidate = string.Format("{0}{1}{2}", name, string.Format(parenFmt, i), ext);
                full = Path.Combine(dir, candidate).ReplacePath();
                if (!File.Exists(full))
                {
                    return candidate;
                }
                i++;
                if (i > int.MaxValue - 1) throw new IOException("Failed to generate unique file name: overflow.");
            }
        }

        /// <summary>
        /// 在同一目录下生成不重复的完整文件路径（含目录），若存在同名则在文件名后追加流水号。
        /// </summary>
        public static string EnsureUniqueFilePath(string directoryPath, string fileName, bool useFullWidthParentheses = false)
        {
            var uniqueName = EnsureUniqueFileName(directoryPath, fileName, useFullWidthParentheses);
            return Path.Combine((directoryPath ?? string.Empty).ReplacePath(), uniqueName).ReplacePath();
        }
        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="oldPath"></param>
        /// <param name="newPath"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        public static bool RenameFile(string oldPath, string newPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(oldPath)) throw new ArgumentNullException(nameof(oldPath));
                if (string.IsNullOrWhiteSpace(newPath)) throw new ArgumentNullException(nameof(newPath));

                var oldFullPath = oldPath.ReplacePath();
                var newFullPath = newPath.ReplacePath();

                if (!File.Exists(oldFullPath)) throw new FileNotFoundException("File not found: " + oldFullPath);
                if (File.Exists(newFullPath)) throw new IOException("File already exists: " + newFullPath);

                File.Move(oldFullPath, newFullPath);
                return true;
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(ex.Message);
                return false;
            }
            
        }
    }

    /// <summary>
    /// 表示文件系统中的一个项目（文件或文件夹）
    /// </summary>
    public class FileSystemItem
    {
        public string Name { get; set; }           // 名称（不含路径）
        public string FullPath { get; set; }       // 完整路径
        public bool IsDirectory { get; set; }      // 是否为文件夹
        public long FileSize { get; set; }         // 文件大小（字节）
        public string FormattedSize { get; set; }  // 格式化的文件大小
        public string FileType { get; set; }       // 文件类型描述
    }
}
