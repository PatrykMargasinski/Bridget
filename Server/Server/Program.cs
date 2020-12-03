using System;
using System.Collections.Generic;
namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0;i<7; i++)
            {
                Score score = new Score((1).ToString()+":D", 7+i, true, true);
                Console.WriteLine(score.GetScore());
            }
            //Controller controller = new Controller();
            //controller.Start();
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Console.WriteLine("exit");
            Environment.Exit(Environment.ExitCode);
        }
    }
}