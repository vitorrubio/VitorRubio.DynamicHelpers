using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace VitorRubio.DynamicHelpersTest
{
    [TestClass]
    public class TestesGeraisCansadoTest
    {
        [TestMethod]
        [ExpectedException(typeof(Exception), "Deveria disparar uma exception aqui mas não disparou", AllowDerivedTypes =true )]
        public void ThrowsExceptionTest()
        {
            throw new Exception("Eita!");
        }

    }
}
