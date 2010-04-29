using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Bugzzz
{
    class Statistics
    {
        private int startTime;
        private int currentTime;
        private bool started;
        private int spreeLength;
        private ArrayList lifeTimes;

        public Statistics(int startTime,bool started)
        {
            this.startTime = startTime;
            this.started = started;
            this.spreeLength = 0;
            this.lifeTimes = new ArrayList();
            this.currentTime = startTime;
        }

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

        public void updateStatistics()
        {
           // should be set to the current game time 
           // currentTime = 0;
        }
    }
}