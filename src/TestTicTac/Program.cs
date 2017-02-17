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
            Console.WriteLine("Training 2 AIs against each other...");
            t.Train(25000);
            string response = "";
            while (!response.Contains("q"))
            {
                t.Play(SpotState.O);
                t.Play(SpotState.X);
                Console.WriteLine("Hit 'q' <enter> to quit, <enter> to play again.");
                response = Console.ReadLine();
            }
        }


    }
}
