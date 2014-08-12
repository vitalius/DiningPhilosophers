using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiningPhilosophers
{
    /// <summary>
    /// Philosopher states
    /// </summary>
    public enum PhilosopherState
    {
        Eating,
        Thinking,
        Done
    }


    /// <summary>
    /// Philosopher model in the Dining Philosophers problem
    /// </summary>
    public class Philosopher
    {
        private int ThinkMs;
        private int EatMs;
        private int Id;
        private int FoodCount;
        private Mutex LeftFork;
        private Mutex RightFork;

        // wait time for mutex
        public int MutexWaitMs = 1000;


        public PhilosopherState State;


        /// <summary>
        /// Construct a philosopher
        /// </summary>
        /// <param name="id">indentifier</param>
        /// <param name="left">left fork</param>
        /// <param name="right">right fork</param>
        /// <param name="thinkMs">time it takes to think</param>
        /// <param name="eatMs">time it takes to eat</param>
        /// <param name="food">food units</param>
        public Philosopher(int id, Mutex left, Mutex right, 
                           int thinkMs = 2000, int eatMs = 1000, int food = 3)
        {
            LeftFork = left;
            RightFork = right;
            ThinkMs = thinkMs;
            EatMs = eatMs;
            FoodCount = food;
            Id = id;
            State = PhilosopherState.Thinking;
        }


        /// <summary>
        /// Attempt to secure left and right fork and eat food
        /// </summary>
        public void Eat()
        {
            while (State != PhilosopherState.Done)
            {
                //Console.WriteLine("{0} is requesting left", Id);
                if (LeftFork.WaitOne(MutexWaitMs))
                {
                    //Console.WriteLine("{0} is requesting right", Id);
                    if (RightFork.WaitOne(MutexWaitMs))
                    {
                        FoodCount--;
                        State = PhilosopherState.Eating;
                        Console.WriteLine("{0} eating; {1} food", Id, FoodCount);
                        Thread.Sleep(EatMs);

                        // release forks after eating
                        RightFork.ReleaseMutex();
                        LeftFork.ReleaseMutex();

                        if (FoodCount < 1)
                        {
                            State = PhilosopherState.Done;
                            Console.WriteLine("{0} done.", Id);
                        }

                        // commence thinking!
                        Think();
                    }
                    else
                    {
                        // release left fork if unable aquire right fork
                        LeftFork.ReleaseMutex();
                    }
                }
            }
        }

        
        /// <summary>
        /// If still have food, think for awhile
        /// </summary>
        public void Think()
        {
            if (State == PhilosopherState.Done)
                return;

            State = PhilosopherState.Thinking;
            Console.WriteLine("{0} thinking; {1} food", Id, FoodCount);
            Thread.Sleep(ThinkMs);
        }
    }
}
