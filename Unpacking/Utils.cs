using System;
using SevenZip;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Unpacking
{
    class Utils
    {
        public const String OUT_PATH = @"V:\TEMP\OUT";
        public static List<Task> Tasks;

        public async static void DeleteOutPath()
        {
            DirectoryInfo di = new DirectoryInfo(OUT_PATH);
            FileInfo[] fi = di.GetFiles("*.*", SearchOption.AllDirectories);

            foreach (var item in fi)
            {
                File.Delete(item.FullName);
            }
        }

        public async static void ExtractFile(String pth,int index) {
            SevenZipCompressor.SetLibraryPath(@".\x86\7z.dll");
           
            FileInfo fi = new FileInfo(pth);
            DirectoryInfo di = new DirectoryInfo(OUT_PATH);
            using (var tmp = new SevenZipExtractor(fi.FullName))
            {
                for (int i = 0; i<tmp.ArchiveFileData.Count; i++)
                {
                    tmp.ExtractFiles(di.FullName, tmp.ArchiveFileData[i].Index);
                   
                    FileInfo fni = new FileInfo(di.FullName + @"\" + tmp.ArchiveFileData[i].FileName);
                    FileInfo fno = new FileInfo(fni.DirectoryName + @"\" + 
                        fni.Name.Replace(fni.Extension, "") + "_" + (index++).ToString() + fni.Extension);
                    File.Move(fni.FullName,fno.FullName);
                    Console.WriteLine(fno.FullName + " thread id = " + Task.CurrentId);
                    
                    if (fno.Name.Contains(".zip") || fno.Name.Contains(".7z"))
                    {

                        var child = Task.Factory.StartNew(() => 
                        {
                            ExtractFile(fno.FullName, index++);
                            File.Delete(fno.FullName);
                           
                        });

                        child.Wait();
                    }
                }
            }

            
        }
    }
}
