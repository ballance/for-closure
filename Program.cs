using System;
using System.Threading.Tasks;

namespace ballance.it.for_closure
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Retrieving the list");
            Runner runner = new Runner();

            try
            {
                runner.Run();
            }
            catch(Exception ex)
            {
                System.Console.WriteLine("Error!");
                System.Console.WriteLine("------");
                System.Console.WriteLine(ex);
            }
            Console.Read();
        }
    }
}
