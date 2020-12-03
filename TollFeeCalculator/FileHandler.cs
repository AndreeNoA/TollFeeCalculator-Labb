using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class FileHandler
    {
        public string ReadDataFromFile(String inputFile)
        {
            string indata;
            try
            {
                indata = System.IO.File.ReadAllText(inputFile);
                return indata;
            }
            catch (Exception)
            {
                return "No file found";
            }
        }
        public DateTime[] GetPassingTimeFromString(string inData)
        {
            String[] dateStrings = inData.Split(", ");
            DateTime[] dates = new DateTime[dateStrings.Length];
            for (int i = 0; i < dates.Length; i++)
            {
                try
                {
                    dates[i] = DateTime.Parse(dateStrings[i]);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return dates;
        }
    }
}
