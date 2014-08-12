using System;

namespace DiningPhilosophers
{
    /// <summary>
    /// Program for Dining Philosophers problem
    /// Rules:
    /// 
    /// * think until the left fork is available; when it is, pick it up;
    /// * think until the right fork is available; when it is, pick it up;
    /// * when both forks are held, eat for a fixed amount of time;
    /// * then, put the right fork down;
    /// * then, put the left fork down;
    /// * repeat from the beginning.
    /// 
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            var dinnerTable = new PhilosophersTable(5);
            dinnerTable.Run();
            Console.ReadLine();
        }
    }
}
