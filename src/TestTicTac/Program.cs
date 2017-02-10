using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTicTac
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var t = new Trainer();
            t.Train(25000);
            string response = "";
            while (!response.Contains("q"))
            {
                t.Play(SpotState.O);
                t.Play(SpotState.X);
                response = Console.ReadLine();
            }
        }


    }
}
