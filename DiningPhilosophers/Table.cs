using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DiningPhilosophers
{
    /// <summary>
    /// Using Mutexes as forks and Philosophers as threads, 
    /// model the Dining philosophers problem
    /// </summary>
    public class PhilosophersTable
    {
        public List<Mutex> forks;
        public List<Thread> threads;

        /// <summary>
        /// Build philosopher threads
        /// </summary>
        /// <param name="party_size">number of philosophers at the table</param>
        public PhilosophersTable(int party_size)
        {
            forks = new List<Mutex>();
            threads = new List<Thread>();

            // build first fork
            var left = new Mutex();
            forks.Add(left);

            // build philosophers and more forks
            for (int i = 1; i < party_size; i++)
            {                
                var right = new Mutex();
                forks.Add(right);
                threads.Add(new Thread(new Philosopher(i, left, right).Eat));
                left = right;
            }

            // build last philosopher with fork shared by first philosopher
            var last = new Philosopher(party_size, left, forks.First());
            threads.Add(new Thread(last.Eat));
        }


        /// <summary>
        /// Start threads
        /// </summary>
        public void Run()
        {
            Console.WriteLine("Philosophers {0}  Forks {0}", 
                threads.Count(), forks.Count());

            foreach (var t in threads)
            {
                t.Start();
            }
        }
    }
}

