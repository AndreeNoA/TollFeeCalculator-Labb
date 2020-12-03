using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Calculator
    {
        public int TotalFeeCost(DateTime[] dateArray)
        {
            int totalFee = 0;
            DateTime startingInterval = dateArray[0];
            foreach (var passing in dateArray)
            {
                var interval = CalculateTimeBetweenPassings(passing, startingInterval);
                if (interval.TotalMinutes > 60 || passing == dateArray[0])
                {
                    totalFee += TollFeePass(passing);
                    startingInterval = passing;
                }
                else
                {
                    int costDifference = CalculateCostDifference(TollFeePass(passing), TollFeePass(startingInterval));
                    if (costDifference > 0)
                    {
                        totalFee += costDifference;
                    }
                }
            }
            return Math.Min(totalFee, 60);
        }

        public TimeSpan CalculateTimeBetweenPassings(DateTime passing, DateTime startingInterval)
        {
            TimeSpan timeInterval = passing - startingInterval;
            return timeInterval;
        }

        public int CalculateCostDifference(int passingCost, int startingIntervalCost)
        {
            int differenceInCost = passingCost - startingIntervalCost;
            return differenceInCost;
        }

        public int TollFeePass(DateTime currentPassing)
        {
            if (costFreeDay(currentPassing)) return 0;
            int hour = currentPassing.Hour;
            int minute = currentPassing.Minute;
            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 8 && minute >= 30 && minute <= 59) return 8;
            else if (hour >= 9 && hour <= 14) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }
        public bool costFreeDay(DateTime day)
        {
            return (int)day.DayOfWeek == 0 || (int)day.DayOfWeek == 6 || day.Month == 7;
        }
    }
}
