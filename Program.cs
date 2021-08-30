using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripting
{
    class Program
    {
        static void Main(string[] args)
        {
            SearchPathASG SP = new SearchPathASG();
            SP.GeneradorDeMaze();
            Console.ReadKey();
        }
    }
}
