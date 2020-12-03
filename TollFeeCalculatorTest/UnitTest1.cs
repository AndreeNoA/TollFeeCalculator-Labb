using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TollFeeCalculator;

namespace TollFeeCalculatorTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCheckCostFreeDays()
        {
            Calculator calculator = new Calculator();

            var testFreeDay = new DateTime(2020, 12, 05);
            var testNonFreeDay = new DateTime(2020, 12, 02);
            var testFreeMonth = new DateTime(2020, 07, 15);

            Assert.IsTrue(calculator.costFreeDay(testFreeDay));
            Assert.IsFalse(calculator.costFreeDay(testNonFreeDay));
            Assert.IsTrue(calculator.costFreeDay(testFreeMonth));
        }

        [TestMethod]
        public void TestCost()
        {
            Calculator calculator = new Calculator();

            var expected = new DateTime(2020, 12, 02, 05, 00, 00);
            var expectedTwo = new DateTime(2020, 12, 03, 13, 30, 00);
            var expectedThree = new DateTime(2020, 12, 01, 18, 28, 00);

            Assert.AreEqual(0, calculator.TollFeePass(expected));
            Assert.AreEqual(8, calculator.TollFeePass(expectedTwo));
            Assert.AreEqual(8, calculator.TollFeePass(expectedThree));
        }

        [TestMethod]
        public void TestCostBetween8And14()
        {
            Calculator calculator = new Calculator();

            var expected = new DateTime(2020, 12, 02, 09, 10, 00);
            var expectedTwo = new DateTime(2020, 12, 03, 13, 17, 00);
            var expectedThree = new DateTime(2020, 12, 01, 14, 57, 00);
            var expectedFour = new DateTime(2020, 12, 02, 09, 10, 00);

            Assert.AreEqual(8, calculator.TollFeePass(expected));
            Assert.AreEqual(8, calculator.TollFeePass(expectedTwo));
            Assert.AreEqual(8, calculator.TollFeePass(expectedThree));
            Assert.AreNotEqual(0, calculator.TollFeePass(expectedFour));
        }

        [TestMethod]
        public void TestCostBetween15And17()
        {
            Calculator calculator = new Calculator();

            var expected = new DateTime(2020, 12, 02, 15, 10, 00);
            var expectedTwo = new DateTime(2020, 12, 03, 15, 48, 00);
            var expectedThree = new DateTime(2020, 12, 01, 16, 57, 00);

            Assert.AreEqual(13, calculator.TollFeePass(expected));
            Assert.AreEqual(18, calculator.TollFeePass(expectedTwo));
            Assert.AreEqual(18, calculator.TollFeePass(expectedThree));
        }

        [TestMethod]
        public void TestMaxReturnFee()
        {
            Calculator calculator = new Calculator();

            DateTime[] passingFeeOverMax = new DateTime[]
            {
               new DateTime(2020, 12, 02, 06, 31, 00),
               new DateTime(2020, 12, 02, 07, 33, 00),
               new DateTime(2020, 12, 02, 15, 30, 00),
               new DateTime(2020, 12, 02, 16, 35, 00)
            };
            DateTime[] passingFeeUnderMax = new DateTime[]
            {
               new DateTime(2020, 12, 02, 06, 31, 00),
               new DateTime(2020, 12, 02, 07, 33, 00),

            };

            Assert.AreEqual(60, calculator.TotalFeeCost(passingFeeOverMax));
            Assert.AreEqual(31, calculator.TotalFeeCost(passingFeeUnderMax));
        }

        [TestMethod]
        public void TestTimeBetweenPassings()
        {
            Calculator calculator = new Calculator();

            DateTime[] testPassingTimes = new DateTime[]
            {
               new DateTime(2020, 12, 02, 06, 50, 00),
               new DateTime(2020, 12, 02, 07, 20, 00),
            };

            Assert.AreEqual(30, calculator.CalculateTimeBetweenPassings(testPassingTimes[1], testPassingTimes[0]).TotalMinutes);
        }

        [TestMethod]
        public void TestLengthOfDatesArrayAndReturnType()
        {
            FileHandler fileHandler = new FileHandler();

            string dateString = "2020-06-30 08:52, 2020-06-30 10:13, 2020-06-30 10:25, 2020-06-30 11:04, 2020-06-30 16:50, 2020-06-30 18:00, 2020-06-30 21:30, 2020-07-01 00:00";

            Assert.AreEqual(8, fileHandler.GetPassingTimeFromString(dateString).Length);
            Assert.IsInstanceOfType(fileHandler.GetPassingTimeFromString(dateString), typeof(DateTime[]));
        }

        [TestMethod]
        public void TestAddingCostToFee()
        {
            Calculator calculator = new Calculator();

            DateTime[] timesInSameCost = new DateTime[]
            {
               new DateTime(2020, 12, 02, 06, 50, 00),
               new DateTime(2020, 12, 02, 06, 58, 00),
            };
            DateTime[] timesInDifferentCostHigher = new DateTime[]
            {
               new DateTime(2020, 12, 02, 06, 59, 00),
               new DateTime(2020, 12, 02, 07, 58, 00),
            };
            DateTime[] timesInDifferentCostLower = new DateTime[]
            {
               new DateTime(2020, 12, 02, 16, 59, 00),
               new DateTime(2020, 12, 02, 17, 58, 00),
            };

            Assert.AreEqual(13, calculator.TotalFeeCost(timesInSameCost));
            Assert.AreEqual(18, calculator.TotalFeeCost(timesInDifferentCostHigher));
            Assert.AreEqual(18, calculator.TotalFeeCost(timesInDifferentCostLower));
        }

        [TestMethod]
        public void TestPrintedText()
        {
            Program program = new Program();
            string actual;

            using (StringWriter stringWriter = new StringWriter())
            {
                Console.SetOut(stringWriter);
                program.Print(99);
                actual = stringWriter.ToString();
            }
            Assert.AreEqual("The total fee for the inputfile is 99", actual);
        }

        [TestMethod]
        public void TestFormatExceptionWhenEmptyFile()
        {
            FileHandler fileHandler = new FileHandler();

            var emptyFilePath = Environment.CurrentDirectory + "../../../../emptyTestFile.txt";
            var testDataFilePath = System.IO.File.ReadAllText(Environment.CurrentDirectory + "../../../../testData.txt");
            DateTime[] expected = new DateTime[] 
            { 
                new DateTime(2020, 06, 30, 00, 05, 00) 
            };

            Assert.ThrowsException<FormatException>(() => fileHandler.GetPassingTimeFromString(emptyFilePath));
            CollectionAssert.AreEqual(expected, fileHandler.GetPassingTimeFromString(testDataFilePath));
        }

        [TestMethod]
        public void TestFileNotFoundExceptionHandling()
        {
            FileHandler fileHandler = new FileHandler();

            var correctFilePath = Environment.CurrentDirectory + "../../../../emptyTestFile.txt";
            var wrongPath = Environment.CurrentDirectory + "../../../../noFile.txt";

            Assert.AreEqual("No file found", fileHandler.ReadDataFromFile(wrongPath));
            Assert.AreEqual("", fileHandler.ReadDataFromFile(correctFilePath));
        }

    }
}
