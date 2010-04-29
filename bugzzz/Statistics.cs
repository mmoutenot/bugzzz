using System;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Bugzzz
{
    class Statistics
    {
        #region Fields startTime, currentTime, started, spreeLength, lifeTimes
        private TimeSpan startTime;
        private TimeSpan currentTime;
        private bool started;
        private int spreeLength;            //Number of enemies killed in one life?
        private int maxSpreeLength;
        private ArrayList lifeTimes;        //Length of each life
        private int wepSwitch;              //Number of times player switched their weapon

        #endregion

        #region Accessors StartTime, Started, SpreeLength, WepSwitch
        public TimeSpan StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public bool Started
        {
            get { return started; }
            set { started = value; }
        }

        public int MaxSpreeLength
        {
            get { return maxSpreeLength; }
        }

        //Setter for wepSwitch doesn't actually set, just increments value
        public int WepSwitch
        {
            get
            {
                return wepSwitch;
            }
            set
            {
                wepSwitch = wepSwitch + 1;
            }
        }
        #endregion

        #region Main Methods Constructor, updateStatistics
        public Statistics(bool started)
        {
            this.started = started;
            this.spreeLength = 0;
            this.lifeTimes = new ArrayList();
            this.startTime = new TimeSpan(0);
            this.currentTime = new TimeSpan(0);
        }

        public void enemeyKilled()
        {
            spreeLength++;
            if(spreeLength>maxSpreeLength){
                maxSpreeLength = spreeLength;
            }
        }

        public void playerDied()
        {
            spreeLength = 0;
            lifeTimes.Add(currentTime - startTime);
            Console.WriteLine("Player Died: " + (currentTime - startTime));
            startTime = currentTime;
            averageLifeTime();
        }

        public void updateStatisticsTime(GameTime time)
        {
           // set the current time to the current game time;
            currentTime = time.TotalGameTime;
        }
        #endregion

        #region Generated Statistics
        
        // Returns the average lifetime in seconds
        public double averageLifeTime()
        {
            int count = 0;
            TimeSpan totalLifeTime = new TimeSpan(0);
            foreach (TimeSpan i in lifeTimes){
                totalLifeTime+=i;
                count++;
            }
            Console.WriteLine("Average life of player:" + (totalLifeTime.TotalSeconds / count));
            return totalLifeTime.TotalSeconds / count;
        }
        #endregion
    }
}