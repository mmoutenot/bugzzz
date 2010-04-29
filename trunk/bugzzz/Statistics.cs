using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Bugzzz
{
    class Statistics
    {
        #region Fields startTime, currentTime, started, spreeLength, lifeTimes
        private int startTime;
        private int currentTime;
        private bool started;
        private int spreeLength;            //Number of enemies killed in one life?
        private ArrayList lifeTimes;        //Length of each life
        private int wepSwitch;              //Number of times player switched their weapon
        #endregion

        #region Accessors StartTime, Started, SpreeLength, WepSwitch
        public int StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public bool Started
        {
            get { return started; }
            set { started = value; }
        }

        public int SpreeLength
        {
            get { return spreeLength; }
            set { spreeLength = value; }
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
        public Statistics(int startTime,bool started)
        {
            this.startTime = startTime;
            this.started = started;
            this.spreeLength = 0;
            this.lifeTimes = new ArrayList();
            this.currentTime = startTime;
        }

        public void updateStatistics()
        {
           // should be set to the current game time 
           // currentTime = 0;
        }
        #endregion
    }
}