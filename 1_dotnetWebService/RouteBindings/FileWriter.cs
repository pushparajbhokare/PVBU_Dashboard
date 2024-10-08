using App.Configurations;
using System;
using System.IO;

namespace dotnetWebService.RouteBindings
{
    public static class FileWriter
    {
        //Object for thread safe logging 
        private static readonly object _syncObject = new object();

        public static void WriteToFile(string Message)
        {
            string logMessage;

            try
            {
                lock (_syncObject)
                {
                    string filePrefix = "HEXAGON_LOG";
                    runTimeConfiguration config = new runTimeConfiguration();

                    var Txtpath = config.getParticularConfig("LogTxt_filepath", "path");
                    string path = Txtpath + "\\Logs";

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string filePath = String.Format("{0}\\Logs\\{1}{2}.log", Txtpath, filePrefix, String.Format("{0:yyyyMMdd}", DateTime.Now));

                    if (!File.Exists(filePath))
                    {
                        using (StreamWriter sw = new StreamWriter(filePath, true))
                        {
                            logMessage = String.Format("{0:MM/dd/yyyy hh:mm:ss.fff tt} : {1}", DateTime.Now, Message);
                            sw.WriteLine(logMessage);
                        }
                    }
                    else
                    {
                        if ((File.ReadAllBytes(filePath).Length >= 100 * 1024 * 1024)) // (100mB) File to big? Create new
                        {
                            string newfilepath = String.Format("{0}\\Logs\\{1}{2}.log", Txtpath, filePrefix, String.Format("{0:yyyyMMdd_hhmmsstt}", DateTime.Now));
                            File.Move(filePath, newfilepath);                // Rename existing log file
                        }

                        using (StreamWriter sw = new StreamWriter(filePath, true))
                        {
                            logMessage = String.Format("{0:MM/dd/yyyy hh:mm:ss.fff tt} : {1}", DateTime.Now, Message);
                            sw.WriteLine(logMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logMessage = String.Format("WriteToFile Error : {0}.", ex.Message + "\n" + ex.StackTrace + "\n" + ex.InnerException);
                WriteToFile(logMessage);
            }
        }
        private static void WriteToConsoleAndFile(string message)
        {
            Console.WriteLine(message);
        }
    }
}
