using System;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DiningPhilosophers
{
    /// <summary>
    /// Different philosophers' states
    /// </summary>
    public enum PhilosopherState
    {
        Hungry,
        Eating,
        Thinking,
        Done
    }


    /// <summary>
    /// Philosopher model in the Dining Philosophers problem
    /// </summary>
    internal class Philosopher
    {
        private int ThinkMs;
        private int EatMs;
        private int FoodCount;

        private Mutex LeftFork;
        private Mutex RightFork;

        private Ellipse pSprite, lSprite, rSprite;
        private Label statusLabel;
        private TextBox foodText;

        private PhilosopherState State;


        /// <summary>
        /// Build philosopher thread
        /// </summary>
        /// <param name="id">identifier, helps with random number generator</param>
        /// <param name="left">left Fork mutex</param>
        /// <param name="right">right Fork mutex</param>
        /// <param name="philSprite">UI ellipse for marking philosopher</param>
        /// <param name="lforkSprite">UI ellipse for left fork</param>
        /// <param name="rforkSprite">UI ellipse for right fork</param>
        /// <param name="pLabel">UI label for showing philosopher status</param>
        /// <param name="fText">UI TextBox for food counter</param>
        public Philosopher(int id, Mutex left, Mutex right,
                           Ellipse philSprite,
                           Ellipse lforkSprite, Ellipse rforkSprite,
                           Label pLabel, TextBox fText)
        {
            LeftFork = left;
            RightFork = right;

            // activity timers
            Random rnd = new Random((int)DateTime.Now.Ticks + id);
            ThinkMs = rnd.Next(100, 500);
            EatMs = rnd.Next(100, 500);
            FoodCount = rnd.Next(10, 30);

            // ui
            pSprite = philSprite;
            lSprite = lforkSprite;
            rSprite = rforkSprite;
            foodText = fText;
            statusLabel = pLabel;            
        }


        /// <summary>
        /// Secure left and right fork and commence eating.
        /// </summary>
        public void Participate()
        {
            SetState(PhilosopherState.Thinking);

            while (State != PhilosopherState.Done)
            {
                // attempt to aquire left fork
                if (LeftFork.WaitOne(10))
                {
                    // now right fork
                    if (RightFork.WaitOne(10))
                    {
                        // clean the forks ;)
                        Thread.Sleep(100);

                        SetState(PhilosopherState.Eating);

                        RightFork.ReleaseMutex();
                        LeftFork.ReleaseMutex();

                        SetState(PhilosopherState.Thinking);

                        if (FoodCount > 0)
                            SetState(PhilosopherState.Hungry);
                        else
                            SetState(PhilosopherState.Done);
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
        /// Update philosoher state and corresponding UI elements
        /// </summary>
        public void SetState(PhilosopherState state)
        {
            State = state;

            switch (State)
            {
                case PhilosopherState.Hungry:
                    statusLabel.Dispatcher.Invoke(new Action(() => statusLabel.Content = "Hungry"));
                    foodText.Dispatcher.Invoke(new Action(() => foodText.Text = FoodCount.ToString()));
                    break;
                case PhilosopherState.Eating:
                    rSprite.Dispatcher.Invoke(new Action(() => rSprite.Visibility = Visibility.Visible));
                    lSprite.Dispatcher.Invoke(new Action(() => lSprite.Visibility = Visibility.Visible));
                    pSprite.Dispatcher.Invoke(new Action(() => pSprite.Visibility = Visibility.Visible));
                    statusLabel.Dispatcher.Invoke(new Action(() => statusLabel.Content = "Eating"));
                    foodText.Dispatcher.Invoke(new Action(() => foodText.Text = FoodCount.ToString()));
                    FoodCount--;
                    Thread.Sleep(EatMs);
                    break;
                case PhilosopherState.Thinking:
                    rSprite.Dispatcher.Invoke(new Action(() => rSprite.Visibility = Visibility.Hidden));
                    lSprite.Dispatcher.Invoke(new Action(() => lSprite.Visibility = Visibility.Hidden));
                    pSprite.Dispatcher.Invoke(new Action(() => pSprite.Visibility = Visibility.Hidden));
                    statusLabel.Dispatcher.Invoke(new Action(() => statusLabel.Content = "Thinking..."));
                    foodText.Dispatcher.Invoke(new Action(() => foodText.Text = FoodCount.ToString()));
                    Thread.Sleep(ThinkMs);
                    break;
                case PhilosopherState.Done:
                    statusLabel.Dispatcher.Invoke(new Action(() => statusLabel.Content = "Done."));
                    foodText.Dispatcher.Invoke(new Action(() => foodText.Text = FoodCount.ToString()));
                    break;
                default:
                    break;
            }
        }
    }
}
