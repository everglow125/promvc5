using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsolDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Test();
            } while (Console.ReadKey().ToString() == "Q");
        }
        static void Test()
        {
            try
            {
                bool result = false;
                Thread unZipAndMatchThread = new Thread(() => ConvertInt("aaa"));
                unZipAndMatchThread.IsBackground = true;
                unZipAndMatchThread.Start();
                result = true;
                Console.WriteLine(result);
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static int ConvertInt(string aaa)
        {
            return Convert.ToInt32(aaa);
        }
    }
}
