using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;

using ICSharpCode.SharpZipLib.Zip;

namespace T2VSoft.DWQ
{
    public class DwqZip
    {
        public static bool ZipFile(string FileToZip, string ZipedFile, String Password)
        {
            if (!File.Exists(FileToZip))
            {
                throw new System.IO.FileNotFoundException("NoFile: " + FileToZip + " Not Exist!");
            }
            FileStream readFile = null;
            FileStream zipFile = null;

            ZipOutputStream zipStream = null;
            ZipEntry zipEntry = null;

            bool res = true;
            try
            {
                if (File.Exists(ZipedFile))
                {
                    File.Delete(ZipedFile);
                }
                zipFile = File.Create(ZipedFile);
                readFile = File.OpenRead(FileToZip);

                zipEntry = new ZipEntry(Path.GetFileName(FileToZip));
                zipStream = new ZipOutputStream(zipFile);
                zipEntry.DateTime = DateTime.Now;
                zipEntry.Size = readFile.Length;
                zipStream.PutNextEntry(zipEntry);
                zipStream.SetLevel(6);

                int stepLength = Convert.ToInt32(ConfigurationManager.AppSettings["ZipBufferLength"]);
                int startPoint = 0;
                for (int i = 1; true; i++)
                {
                    try
                    {
                        if (startPoint >= readFile.Length)
                            break;
                        if (readFile.Length - startPoint <= stepLength)
                        {
                            stepLength = Convert.ToInt32(readFile.Length) - startPoint;
                        }
                        byte[] buffer = new byte[stepLength];
                        readFile.Read(buffer, 0, stepLength);

                        //ZipStream.Password = Password;
                        zipStream.Write(buffer, 0, stepLength);
                        startPoint += stepLength;

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            finally
            {
                if (zipEntry != null)
                {
                    zipEntry = null;
                }
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
                if (zipFile != null)
                {
                    zipFile.Close();
                    zipFile = null;
                }
                if (readFile != null)
                {
                    readFile.Close();
                }

                GC.Collect();
                GC.Collect(1);
            }

            return res;
        }

        public static bool ZipByteFile(string FilePath,string ZipedFile, string strValue)
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            using (StreamWriter sw = File.CreateText(FilePath))
            {
                sw.Write(strValue);
            }

            FileStream readFile = null;
            FileStream zipFile = null;

            ZipOutputStream zipStream = null;
            ZipEntry zipEntry = null;

            bool res = true;
            try
            {
                if (File.Exists(ZipedFile))
                {
                    File.Delete(ZipedFile);
                }
                zipFile = File.Create(ZipedFile);
                readFile = File.OpenRead(FilePath);

                zipEntry = new ZipEntry(Path.GetFileName(FilePath));
                zipStream = new ZipOutputStream(zipFile);
                zipEntry.DateTime = DateTime.Now;
                zipEntry.Size = readFile.Length;
                zipStream.PutNextEntry(zipEntry);
                zipStream.SetLevel(6);

                int stepLength = Convert.ToInt32(ConfigurationManager.AppSettings["ZipBufferLength"]);
                int startPoint = 0;
                for (int i = 1; true; i++)
                {
                    try
                    {
                        if (startPoint >= readFile.Length)
                            break;
                        if (readFile.Length - startPoint <= stepLength)
                        {
                            stepLength = Convert.ToInt32(readFile.Length) - startPoint;
                        }
                        byte[] buffer = new byte[stepLength];
                        readFile.Read(buffer, 0, stepLength);

                        //ZipStream.Password = Password;
                        zipStream.Write(buffer, 0, stepLength);
                        startPoint += stepLength;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                res = false;
            }
            finally
            {
                if (zipEntry != null)
                {
                    zipEntry = null;
                }
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
                if (zipFile != null)
                {
                    zipFile.Close();
                    zipFile = null;
                }
                if (readFile != null)
                {
                    readFile.Close();
                }
                GC.Collect();
                GC.Collect(1);
            }

            return res;
        }

        public static bool ZipFile(List<String> FilesToZip, string ZipedFile, String Password)
        {
            for (int i = 0; i < FilesToZip.Count; i++)
            {
                if (!File.Exists(FilesToZip[i]))
                {
                    throw new System.IO.FileNotFoundException("NoFile: " + FilesToZip[i] + " Not Exist!");
                }
            }

            FileStream readFile = null;
            FileStream zipFile = null;
            ZipOutputStream zipStream = null;
            ZipEntry zipEntry = null;

            bool res = true;
            try
            {
                zipFile = File.Create(ZipedFile);
                zipStream = new ZipOutputStream(zipFile);
                for (int j = 0; j < FilesToZip.Count; j++)
                {
                    readFile = File.OpenRead(FilesToZip[j]);
                    zipEntry = new ZipEntry(Path.GetFileName(FilesToZip[j]));
                    zipEntry.DateTime = DateTime.Now;
                    zipEntry.Size = readFile.Length;
                    zipStream.PutNextEntry(zipEntry);
                    zipStream.SetLevel(6);
                    int stepLength = Convert.ToInt32(ConfigurationManager.AppSettings["ZipBufferLength"]);
                    int startPoint = 0;
                    for (int i = 1; true; i++)
                    {
                        try
                        {
                            if (startPoint >= readFile.Length)
                                break;
                            if (readFile.Length - startPoint <= stepLength)
                            {
                                stepLength = Convert.ToInt32(readFile.Length) - startPoint;
                            }
                            byte[] buffer = new byte[stepLength];
                            readFile.Read(buffer, 0, stepLength);

                            //ZipStream.Password = Password;
                            zipStream.Write(buffer, 0, stepLength);
                            startPoint += stepLength;

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    zipEntry = null;
                    readFile.Close();
                }

            }
            catch (Exception ex)
            {
                res = false;
                throw ex;
            }
            finally
            {
                if (zipEntry != null)
                {
                    zipEntry = null;
                }
                if (zipStream != null)
                {
                    zipStream.Finish();
                    zipStream.Close();
                }
                if (zipFile != null)
                {
                    zipFile.Close();
                    zipFile = null;
                }
                if (readFile != null)
                {
                    readFile.Close();
                }

                GC.Collect();
                GC.Collect(1);
            }

            return res;
        }
    }
}
