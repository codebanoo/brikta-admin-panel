using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWork
{
    /// <summary>
    /// https://blog.aspose.com/2020/04/22/create-zip-archives-add-files-or-folders-to-zip-in-csharp-asp.net/
    /// </summary>
    public static class ZipsManagement
    {
        public static bool CreateFromDirectory(string sourcePath,
            string destinationPath)
        {
            try
            {
                ZipFile.CreateFromDirectory(sourcePath, destinationPath, CompressionLevel.Optimal, true, Encoding.UTF8);
                return true;
            }
            catch (Exception exc)
            { }

            return false;
        }

        public static bool CreateFromFile(string sourceFilePath,
            string destinationFilePath)
        {
            try
            {
                using (FileStream __fStream = File.Open(destinationFilePath, FileMode.Create))
                {
                    GZipStream obj = new GZipStream(__fStream, CompressionMode.Compress);

                    byte[] bt = File.ReadAllBytes(sourceFilePath);
                    obj.Write(bt, 0, bt.Length);

                    obj.Close();
                    obj.Dispose();
                }
                return true;
            }
            catch (Exception exc)
            { }

            return false;
        }

        public static bool CreateFromFileWithExt(string zipFilePath, string fileName, string filePath)
        {
            try
            {
                using (var fileStream = new FileStream(zipFilePath, FileMode.CreateNew))
                {
                    using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create, true))
                    {
                        byte[] bt = File.ReadAllBytes(filePath);

                        var zipArchiveEntry = archive.CreateEntry(fileName, CompressionLevel.Optimal);

                        using (var zipStream = zipArchiveEntry.Open())
                            zipStream.Write(bt, 0, bt.Length);
                    }
                }
            }
            catch (Exception exc)
            { }

            return false;
        }

        public static bool UnZip(string zipPath, string destinationPath)
        {
            try
            {
                ZipFile.ExtractToDirectory(zipPath, destinationPath, Encoding.UTF8, true);
                return true;
            }
            catch (Exception exc)
            { }

            return false;
        }
    }
}
