using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Task2.Tests
{
    [TestClass]
    public class MyMathUnitTest
    {
        [TestMethod]
        public void Factorial_PositiveLittleNumber_Calculated()
        {
            int testNumber = 4;
            Assert.AreEqual(24, MyMath.Factorial(testNumber));
        }

        [TestMethod]
        public void Factorial_PositiveHugeNumber_Calculated()
        {
            int testNumber = 10;
            Assert.AreEqual(3628800, MyMath.Factorial(testNumber));
        }

        [TestMethod]
        public void Factorial_NegativeNumber_ThrowException()
        {
            int testNumber = -4;
            try
            {
                MyMath.Factorial(testNumber);
                Assert.Fail("ArgumentOutOfRange exception expected");
            }
            catch (Exception e)
            {
            }
        }

        [TestMethod]
        public void Factorial_VaryBigNumber_Calculated()
        {
            MyMath.Factorial(10000);
        }


        [TestMethod]
        public void Fibonachi_ForTwo_EqualThree()
        {
            Assert.AreEqual(3, MyMath.Fibonachi(4));
        }
        [TestMethod]
        public void Fibonachi_ForLargeNumber_Calculated()
        {
            Assert.AreEqual(3, MyMath.Fibonachi(400));
        }
    }
}
