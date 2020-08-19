using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace VitorRubio.DynamicHelpersTest
{
    /// <summary>
    /// Testa as classes Cobaia que são classes comuns fortemente tipadas
    /// 
    /// Tests the Cobaia (lab-rat / subject *) class, which common ordinary poco strongly typed anemic class
    /// 
    /// *I shouldn't find a correct expression to translate "Cobaia" to english, but in my researches I found "Guinea Pig" ?!?! WTF?
    /// </summary>
    [TestClass]
    public class StaticTest
    {
        [TestMethod]
        public void ShouldNotIgnoreNullPropertiesNewtonsoftJson() //amen
        {
            Cobaia obj = new Cobaia();
            string serializedObject = JsonHelpers.ToJsonStringUsingNewtonsoftJson(obj);
            Assert.IsFalse(string.IsNullOrWhiteSpace(serializedObject));
            Assert.IsTrue(serializedObject.Contains("UmIntNulavelQualquer"));
        }

        [TestMethod]
        public void ShouldNotIgnoreNullPropertiesDataContractJsonSerialyzer() //amen
        {
            Cobaia obj = new Cobaia();
            string serializedObject = JsonHelpers.ToJsonStringUsingDataContractJsonSerialyzer(obj);
            Assert.IsFalse(string.IsNullOrWhiteSpace(serializedObject));
            Assert.IsTrue(serializedObject.Contains("UmIntNulavelQualquer"));
        }

        [TestMethod]
        public void ShouldNotIgnoreNullPropertiesJavaScriptJsonSerializer() //amen
        {
            Cobaia obj = new Cobaia();
            string serializedObject = JsonHelpers.ToJsonStringUsingJavaScriptJsonSerializer(obj);
            Assert.IsFalse(string.IsNullOrWhiteSpace(serializedObject));
            Assert.IsTrue(serializedObject.Contains("UmIntNulavelQualquer"));
        }

    }
}
