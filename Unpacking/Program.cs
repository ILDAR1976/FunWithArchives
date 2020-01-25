using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Unpacking
{
    class Program
    {
        static void Main(string[] args)
        {
            Utils.Tasks = new List<Task>();
            Utils.DeleteOutPath();

            var parent = Task.Factory.StartNew(() =>
            {
                Utils.ExtractFile(@"V:\TEMP\arch.zip", 1);
            });

            parent.Wait();
            Console.WriteLine("The quantity runnig threads = {0}", Utils.Tasks.Count);
            
        }
    }
}
