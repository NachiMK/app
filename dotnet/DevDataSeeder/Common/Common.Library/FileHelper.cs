using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Export.Common.Library
{
    public class FileHelper
    {
        public static bool IsValidDirectoryPath(string dirPath)
        {
            DirectoryInfo di = new DirectoryInfo(dirPath);
            bool blnRetval = false;

            if ((di != null) && (di.Exists))
                blnRetval = true;

            return blnRetval;
        }
        public static string RemoveInvalidChars(string stringToClean)
        {
            string strCleaned = stringToClean;
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                strCleaned = strCleaned.Replace(c, '_');
            }
            return strCleaned;
        }

        public static string GetFormattedDate(string dateFormatString, short millisecondsToWait = 1)
        {
            System.Threading.Thread.Sleep(millisecondsToWait);
            return DateTime.Now.ToString(dateFormatString);
        }

        public static void CreateFile(string FileName, MemoryStream DataMemoryStream, string compressedFileExt)
        {
            if (DataMemoryStream == null)
                DataMemoryStream = new MemoryStream();
            try
            {
                // open a File Stream
                if (!compressedFileExt.Contains("gzip"))
                {
                    using (FileStream fs = new FileStream(FileName, FileMode.Append, FileAccess.Write))
                    {
                        DataMemoryStream.WriteTo(fs);
                    }
                }
                else
                {
                    // open compressed file
                    string strFileName = FileName + compressedFileExt;
                    using (FileStream compressedFStream = new FileStream(strFileName, FileMode.Append, FileAccess.Write))
                    {
                        // compressions stream
                        using (GZipStream compressionStream = new GZipStream(compressedFStream, CompressionMode.Compress))
                        {
                            DataMemoryStream.WriteTo(compressionStream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DataMemoryStream.Dispose();
                throw ex;
            }
        }

        public static void CompressFile(string FileName, string compressedFileExt)
        {
            // get file details for file to be compressed
            FileInfo fileToCompress = new FileInfo(FileName);

            // open the given file as a stream
            using (FileStream originalFileStream = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            {
                // open a compression file
                using (FileStream compressedFileStream = File.Create(fileToCompress.FullName + compressedFileExt))
                {
                    // open a gzip stream
                    using (GZipStream compressionStream = new GZipStream(compressedFileStream,
                        CompressionMode.Compress))
                    {
                        // write to compressed stream
                        originalFileStream.CopyTo(compressionStream);
                    } // auto close
                } // auto close
            }
        }

        public static bool CopyFileTo(string sourceFileName, string destinationPath
                                     ,bool deleteSourceAfterCopy = true
                                     ,bool Overwrite = true)
        {
            bool retVal = false;

            // Check if source file exists
            if (!CheckFileExists(sourceFileName))
                throw new FileNotFoundException("File to copy is missing.", sourceFileName);

            if (!CheckDirectoryExists(destinationPath))
                throw new DirectoryNotFoundException($"Desitnation Directory {destinationPath} is not found.");

            string strSlash = (destinationPath.Substring(destinationPath.Length - 1, 1) == "\\") ? "" : "\\";
            // get just file name from given full path name
            string strFileName = Path.GetFileName(sourceFileName);
            // create destination file path
            string strDestinationFile = Path.Combine(destinationPath, strFileName);
            bool DestFileExists = CheckFileExists(strDestinationFile);
            try
            {
                // check if file already exists in desitnation
                if (((DestFileExists) && (Overwrite)) || (!DestFileExists))
                {
                    // copy file
                    File.Copy(sourceFileName, strDestinationFile, Overwrite);

                    // check if file exists
                    if (CheckFileExists(strDestinationFile))
                    {
                        retVal = true;
                        // delete source file
                        if (deleteSourceAfterCopy)
                            File.Delete(sourceFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                // catch and return exception
                throw ex;
            }


            // return
            return retVal;
        }

        /// <summary>
        /// returns true if file exists.
        /// If any errors occur then false is returend
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static bool CheckFileExists(string FileName)
        {
            bool retVal = false;
            try
            {
                retVal = (!string.IsNullOrEmpty(FileName)) && (FileName.Length > 0) && (File.Exists(FileName));
            }
            catch
            {
                retVal = false;
            }
            return retVal;
        }

        /// <summary>
        /// returns true if directory exists.
        /// If any errors occur then false is returend
        /// </summary>
        /// <param name="DirectoryPath"></param>
        /// <returns></returns>
        public static bool CheckDirectoryExists(string DirectoryPath)
        {
            bool retVal = false;
            try
            {
                retVal = ((!string.IsNullOrEmpty(DirectoryPath)) && (DirectoryPath.Length > 0) && (Directory.Exists(DirectoryPath)));
            }
            catch
            {
                retVal = false;
            }
            return retVal;
        }

    }
}
