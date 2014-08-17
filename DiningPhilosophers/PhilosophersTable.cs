using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace DiningPhilosophers
{
    /// <summary>
    /// Using Mutexes as forks and Philosophers as threads, 
    /// model the Dining philosophers problem
    /// </summary>
    public class PhilosophersTable
    {
        public List<Mutex> lstForkMutexs;
        public List<Thread> lstPhilosopherThreads;

        /// <summary>
        /// Build philosopher threads
        /// </summary>
        public PhilosophersTable(List<Ellipse> pSprites, 
                                 List<Ellipse> fSprites, 
                                 List<Label> pLabels,
                                 List<TextBox> foodTexts)
        {
            lstForkMutexs = new List<Mutex>();
            lstPhilosopherThreads = new List<Thread>();

            // build first fork
            var leftForkMutex = new Mutex();
            lstForkMutexs.Add(leftForkMutex);

            // build philosophers and more forks
            for (int id = 0; id < pSprites.Count() - 1; id++)
            {
                var rightForkMutex = new Mutex();
                lstForkMutexs.Add(rightForkMutex);
                lstPhilosopherThreads.Add(
                    new Thread(
                        new Philosopher(
                            id, leftForkMutex, rightForkMutex,
                            pSprites.ElementAt(id),
                            fSprites.ElementAt(id), fSprites.ElementAt(id + 1),
                            pLabels.ElementAt(id), foodTexts.ElementAt(id)).Participate));
                leftForkMutex = rightForkMutex;
            }

            // build last philosopher, fork shared with first philosopher
            lstPhilosopherThreads.Add(
                new Thread(
                    new Philosopher(
                        pSprites.Count(), lstForkMutexs.Last(), lstForkMutexs.First(),
                        pSprites.Last(),
                        fSprites.Last(), fSprites.First(),
                        pLabels.Last(), foodTexts.Last()).Participate));
        }


        /// <summary>
        /// Start philosopher threads
        /// </summary>
        public void Run()
        {
            foreach (var t in lstPhilosopherThreads)
            {
                t.Start();
            }
        }
    }
}

