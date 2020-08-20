using System;
using System.Collections.Generic;
using System.Linq;
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

        [TestMethod]
        public void DictionaryShouldBeConcatenated()
        {
            Dictionary<string, object> dic1 = new Dictionary<string, object>();
            dic1.Add("a", new Cobaia());
            dic1.Add("b", new Cobaia());

            Dictionary<string, object> dic2 = new Dictionary<string, object>();
            dic2.Add("a", new Cobaia());
            dic2.Add("c", new Cobaia());

            Dictionary<string, object> dic3 = dic1.Concat(dic2)
                //.ToDictionary(d => d.Key, d => d.Value);
                .GroupBy(d => d.Key)
                .ToDictionary(d => d.Key, d => d.First().Value);
        }

        [TestMethod]
        public void DictionaryShouldBeUnified()
        {
            Dictionary<string, object> dic1 = new Dictionary<string, object>();
            dic1.Add("a", new Cobaia());
            dic1.Add("b", new Cobaia());

            Dictionary<string, object> dic2 = new Dictionary<string, object>();
            dic2.Add("a", new Cobaia());
            dic2.Add("c", new Cobaia());

            Dictionary<string, object> dic3 = dic1.Union(dic2)
                //.ToDictionary(d => d.Key, d => d.Value);
                .GroupBy(d => d.Key)
                .ToDictionary(d => d.Key, d => d.First().Value);
        }

        [TestMethod]
        public void TwoKeyValuePairsWithSameKeysAndValuesShouldBeEqual()
        {
            var k1 = new KeyValuePair<string, object>("A", "test1");
            var k2 = new KeyValuePair<string, object>("A", "test1");

            Assert.IsTrue(k1.Equals(k2));
        }

    }
}
