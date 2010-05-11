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
        private ArrayList pickups;

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


        #endregion

        #region Main Methods Constructor, updateStatistics
        public Statistics(bool started)
        {
            this.started = started;
            this.spreeLength = 0;
            this.lifeTimes = new ArrayList();
            this.pickups = new ArrayList();
            this.startTime = new TimeSpan(0);
            this.currentTime = new TimeSpan(0);
        }

        public void enemyKilled()
        {
            spreeLength++;
            if(spreeLength>maxSpreeLength){
                maxSpreeLength = spreeLength;
            }
        }

        public void incrementWeaponPickup(WeaponPickup p)
        {
            Console.WriteLine("Incrementing weapon:" + p.weaponIndex);
            pickups.Add(p);
        }

        public void playerDied()
        {
            spreeLength = 0;
            lifeTimes.Add(currentTime - startTime);
            //Console.WriteLine("Player Died: " + (currentTime - startTime));
            startTime = currentTime;
        }

        public void updateStatisticsTime(GameTime time)
        {
           // set the current time to the current game time;
            if(started)
                currentTime = time.TotalGameTime;
        }
        #endregion

        #region Generated Statistics
        
        // Returns the average lifetime in seconds
        public float averageLifeTime()
        {
            int count = 1;
            TimeSpan totalLifeTime = new TimeSpan(0);
            // If the player died we want to sum their lifetimes
            // and divide by number of deaths
            if (lifeTimes.Count > 0)
            {
                foreach (TimeSpan i in lifeTimes)
                {
                    totalLifeTime += i;
                    count++;
                }
            }
            // otherwise their totalLifeTime is the level time and we divide by 1
            else
            {
                totalLifeTime = currentTime - startTime;
            }
            //Console.WriteLine("Average life of player:" + (totalLifeTime.TotalSeconds / count));
            double avgMillis = totalLifeTime.TotalMilliseconds / count;
            float avgSec = (float)avgMillis / 1000;
            return avgSec;
        }

        public String favoritePickup()
        {
            int[] weaponCounts = new int[4];

            foreach (WeaponPickup p in pickups)
            {
                switch (p.weaponIndex)
                {
                    case 0:
                        weaponCounts[0]++;
                        break;
                    case 1:
                        weaponCounts[1]++;
                        break;
                    case 2:
                        weaponCounts[2]++;
                        break;
                    case 3:
                        weaponCounts[3]++;
                        break;
                }
            }

            // now we have an array of the number of times a player used each weapon
            int mostUsedWeaponIndex=0;
            int highestNumber=0;
            for (int i = 0; i < 4; i++)
            {
                if(highestNumber<weaponCounts[i]){
                    highestNumber = weaponCounts[i];
                    mostUsedWeaponIndex = i;
                }
            }

            return (new Weapons(null,null,null).names[mostUsedWeaponIndex]);
        }

        #endregion
    }
}