using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessor.CP5200;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor.CP5200.Tests
{
    [TestClass()]
    public class Cp5200ExternalTests
    {
        [TestMethod()]
        public void Program_CreateTest()
        {

            Cp5200External cp5200 = new Cp5200External(192,98,6,0x77);
            Assert.IsNull(cp5200.Program_Create());
            cp5200.SetPlayWindowNumber();
            Assert.Fail();
        }
    }
}