using System;

namespace TollFeeCalculator
{
    public class Program
    {
        
        static void Main()
        {
            Program program = new Program();
            var inputFile = Environment.CurrentDirectory + "../../../../testData.txt";
            program.Run(inputFile);
        }

        public void Run(String inputFile) {
            Calculator calculator = new Calculator();
            FileHandler fileHandler = new FileHandler();
            string dataInString = fileHandler.ReadDataFromFile(inputFile);
            DateTime[] passingTime = fileHandler.GetPassingTimeFromString(dataInString);
            Print(calculator.TotalFeeCost(passingTime));
        }

        public void Print(int totalFee)
        {
            Console.Write("The total fee for the inputfile is " + totalFee);
        }
    }
}
