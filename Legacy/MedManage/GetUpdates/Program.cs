using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GetUpdates
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                
                
                string updatePath;
                string[] Files;
                System.Reflection.Assembly uri = System.Reflection.Assembly.GetExecutingAssembly();
                string DestPath = uri.Location.ToString();
                DestPath = DestPath.Substring(0, DestPath.LastIndexOf("\\"));


                Console.WriteLine("Welcome to the Iconic Development CC update utility");
                Console.WriteLine("Please make sure you close the application before you continue with this update");
                Console.WriteLine("<ENTER> to continue ");
                Console.ReadLine();

               
                
                    Console.WriteLine("Checking for updates...");
                    System.Threading.Thread.Sleep(2500);
                    if (args.Length > 0)
                    {
                        updatePath = args[0];
                        Files = Directory.GetFiles(updatePath);//"\\\\msobi02\\igupdates\\");
                       
                        foreach (string FileName in Files)
                        {
                            Console.WriteLine("Copying file " + FileName + "...");
                            //string actualFileName = ;
                            File.Copy(FileName, DestPath + FileName.Substring(FileName.LastIndexOf("\\")), true);
                        }
                        string[] oDir = Directory.GetDirectories(updatePath);
                        foreach (string DirName in oDir)
                        {
                            Files = Directory.GetFiles(DirName);

                            foreach (string FileName in Files)
                            {
                                Console.WriteLine("Copying file " + FileName + "...");
                                //string actualFileName = ;
                                File.Copy(FileName, DestPath + FileName.Substring(FileName.LastIndexOf("\\")), true);
                            }
                        }
                    }
                    Console.WriteLine("Update Sucessfull");
                    Console.ReadLine();
               
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message.ToString());
                Console.ReadLine();
            }

            
        }
    }
}
