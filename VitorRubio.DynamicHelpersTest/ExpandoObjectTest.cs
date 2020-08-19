using System;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace VitorRubio.DynamicHelpersTest
{
    [TestClass]
    public class ExpandoObjectTest
    {
        [TestMethod]
        public void AddPropertyTest()
        {
            dynamic obj = new ExpandoObject();
            obj.Nome = "vitor";
            obj.Oid = 1;

            Assert.AreEqual("vitor", obj.Nome);
            Assert.AreEqual(1, obj.Oid);

            Assert.IsTrue((obj as IDictionary<string, object>).ContainsKey("Nome"));
            Assert.IsTrue((obj as IDictionary<string, object>).ContainsKey("Oid"));

            var tipo = obj.GetType();
            Assert.AreEqual(typeof(ExpandoObject), tipo);


        }

        [TestMethod]
        public void AddIndexTest()
        {
            dynamic obj = new ExpandoObject();
            (obj as IDictionary<string, object>)["Nome"] = "vitor";
            obj.Oid = 1;


            Assert.AreEqual("vitor", (obj as IDictionary<string, object>)["Nome"]);
            Assert.AreEqual(1, (obj as IDictionary<string, object>)["Oid"]);

            Assert.IsTrue((obj as IDictionary<string, object>).ContainsKey("Nome"));
            Assert.IsTrue((obj as IDictionary<string, object>).ContainsKey("Oid"));

            var tipo = obj.GetType();
            Assert.AreEqual(typeof(ExpandoObject), tipo);

        }

        [TestMethod]
        public void AddTest()
        {
            dynamic obj = new ExpandoObject();
            (obj as IDictionary<string, object>).Add("Nome", "vitor");
            obj.Oid = 1;

            Assert.AreEqual("vitor", obj.Nome);
            Assert.AreEqual(1, obj.Oid);

            Assert.IsTrue((obj as IDictionary<string, object>).ContainsKey("Nome"));
            Assert.IsTrue((obj as IDictionary<string, object>).ContainsKey("Oid"));

            var tipo = obj.GetType();
            Assert.AreEqual(typeof(ExpandoObject), tipo);

        }

        [TestMethod]
        public void ContainsKeyTest()
        {
            dynamic obj = new ExpandoObject();
            obj.Oid = 2;
            obj.Nome = "vitor";
            (obj as IDictionary<string, object>).Add("Data", DateTime.Today);
            (obj as IDictionary<string, object>)["Valor"] = 1.23;

            Assert.AreEqual("vitor", obj.Nome);
            Assert.AreEqual(DateTime.Today, obj.Data);
            Assert.AreEqual(1.23, obj.Valor);
            Assert.AreEqual(2, obj.Oid);


            Assert.IsTrue((obj as IDictionary<string, object>).ContainsKey("Nome"));
            Assert.IsTrue((obj as IDictionary<string, object>).ContainsKey("Data"));
            Assert.IsTrue((obj as IDictionary<string, object>).ContainsKey("Valor"));
            Assert.IsTrue((obj as IDictionary<string, object>).ContainsKey("Oid"));
        }

        [TestMethod]
        public void TryGetValueTest()
        {
            dynamic obj = new ExpandoObject();
            obj.Oid = 2;
            obj.Nome = "vitor";
            (obj as IDictionary<string, object>).Add("Data", DateTime.Today);
            (obj as IDictionary<string, object>)["Valor"] = 1.23;


            Assert.AreEqual("vitor", obj.Nome);
            Assert.AreEqual("vitor", (obj as IDictionary<string, object>)["Nome"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Nome", out object Nome));
            Assert.IsNotNull(Nome);
            Assert.AreEqual("vitor", Nome);

            Assert.AreEqual(DateTime.Today, obj.Data);
            Assert.AreEqual(DateTime.Today, (obj as IDictionary<string, object>)["Data"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Data", out object Data));
            Assert.IsNotNull(Data);
            Assert.AreEqual(DateTime.Today, Data);

            Assert.AreEqual(1.23, obj.Valor);
            Assert.AreEqual(1.23, (obj as IDictionary<string, object>)["Valor"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Valor", out object Valor));
            Assert.IsNotNull(Valor);
            Assert.AreEqual(1.23, Valor);

            Assert.AreEqual(2, obj.Oid);
            Assert.AreEqual(1.23, (obj as IDictionary<string, object>)["Valor"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Oid", out object Oid));
            Assert.IsNotNull(Oid);
            Assert.AreEqual(2, Oid);
        }

        [TestMethod]
        public void SetValueByPropertyTest()
        {
            dynamic obj = new ExpandoObject();

            obj.Oid = 2;
            obj.Nome = "vitor";
            obj.Data = DateTime.Today;
            obj.Valor = 1.23;


            Assert.AreEqual("vitor", obj.Nome);
            Assert.AreEqual("vitor", (obj as IDictionary<string, object>)["Nome"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Nome", out object Nome));
            Assert.IsNotNull(Nome);
            Assert.AreEqual("vitor", Nome);

            Assert.AreEqual(DateTime.Today, obj.Data);
            Assert.AreEqual(DateTime.Today, (obj as IDictionary<string, object>)["Data"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Data", out object Data));
            Assert.IsNotNull(Data);
            Assert.AreEqual(DateTime.Today, Data);

            Assert.AreEqual(1.23, obj.Valor);
            Assert.AreEqual(1.23, (obj as IDictionary<string, object>)["Valor"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Valor", out object Valor));
            Assert.IsNotNull(Valor);
            Assert.AreEqual(1.23, Valor);

            Assert.AreEqual(2, obj.Oid);
            Assert.AreEqual(1.23, (obj as IDictionary<string, object>)["Valor"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Oid", out object Oid));
            Assert.IsNotNull(Oid);
            Assert.AreEqual(2, Oid);
        }



        [TestMethod]
        public void SetValueByAddTest()
        {
            dynamic obj = new ExpandoObject();

            (obj as IDictionary<string, object>).Add("Oid", 2);
            (obj as IDictionary<string, object>).Add("Nome", "vitor");
            (obj as IDictionary<string, object>).Add("Data", DateTime.Today);
            (obj as IDictionary<string, object>).Add("Valor", 1.23);


            Assert.AreEqual("vitor", obj.Nome);
            Assert.AreEqual("vitor", (obj as IDictionary<string, object>)["Nome"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Nome", out object Nome));
            Assert.IsNotNull(Nome);
            Assert.AreEqual("vitor", Nome);

            Assert.AreEqual(DateTime.Today, obj.Data);
            Assert.AreEqual(DateTime.Today, (obj as IDictionary<string, object>)["Data"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Data", out object Data));
            Assert.IsNotNull(Data);
            Assert.AreEqual(DateTime.Today, Data);

            Assert.AreEqual(1.23, obj.Valor);
            Assert.AreEqual(1.23, (obj as IDictionary<string, object>)["Valor"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Valor", out object Valor));
            Assert.IsNotNull(Valor);
            Assert.AreEqual(1.23, Valor);

            Assert.AreEqual(2, obj.Oid);
            Assert.AreEqual(1.23, (obj as IDictionary<string, object>)["Valor"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Oid", out object Oid));
            Assert.IsNotNull(Oid);
            Assert.AreEqual(2, Oid);
        }




        [TestMethod]
        public void SetValueByIndexTest()
        {
            dynamic obj = new ExpandoObject();

            (obj as IDictionary<string, object>)["Oid"] = 2;
            (obj as IDictionary<string, object>)["Nome"] = "vitor";
            (obj as IDictionary<string, object>)["Data"] = DateTime.Today;
            (obj as IDictionary<string, object>)["Valor"] = 1.23;


            Assert.AreEqual("vitor", obj.Nome);
            Assert.AreEqual("vitor", (obj as IDictionary<string, object>)["Nome"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Nome", out object Nome));
            Assert.IsNotNull(Nome);
            Assert.AreEqual("vitor", Nome);

            Assert.AreEqual(DateTime.Today, obj.Data);
            Assert.AreEqual(DateTime.Today, (obj as IDictionary<string, object>)["Data"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Data", out object Data));
            Assert.IsNotNull(Data);
            Assert.AreEqual(DateTime.Today, Data);

            Assert.AreEqual(1.23, obj.Valor);
            Assert.AreEqual(1.23, (obj as IDictionary<string, object>)["Valor"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Valor", out object Valor));
            Assert.IsNotNull(Valor);
            Assert.AreEqual(1.23, Valor);

            Assert.AreEqual(2, obj.Oid);
            Assert.AreEqual(1.23, (obj as IDictionary<string, object>)["Valor"]);
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Oid", out object Oid));
            Assert.IsNotNull(Oid);
            Assert.AreEqual(2, Oid);
        }



        [TestMethod]
        public void ForeachTest()
        {
            dynamic obj = new ExpandoObject();

            obj.Oid = 2;
            obj.Nome = "vitor";
            obj.Data = DateTime.Today;
            obj.Valor = 1.23;
            (obj as IDictionary<string, object>).Add("Foo", "teste foo");
            (obj as IDictionary<string, object>)["Bar"] = "teste bar";

            foreach (var x in obj)
            {
                Console.WriteLine($"{x.Key}:{x.Value}");
            }
        }

        [TestMethod]
        public void ReflectionTest()
        {
            dynamic obj = new ExpandoObject();

            obj.Oid = 2;
            obj.Nome = "vitor";
            obj.Data = DateTime.Today;
            obj.Valor = 1.23;
            (obj as IDictionary<string, object>).Add("Foo", "teste foo");
            (obj as IDictionary<string, object>)["Bar"] = "teste bar";

            foreach (var x in obj.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
            {
                if (x.Name == "Item")
                    continue;
                Console.WriteLine($"{x.Name}:{x.GetValue(obj)}");
            }
        }


        [TestMethod]
        public void SerializeTest()
        {
            dynamic obj = new ExpandoObject();

            obj.Oid = 2;
            obj.Nome = "vitor";
            obj.Data = DateTime.Today;
            obj.Valor = 1.23;
            (obj as IDictionary<string, object>).Add("Foo", "teste foo");
            (obj as IDictionary<string, object>)["Bar"] = "teste bar";


            Assert.AreEqual("vitor", obj.Nome);
            Assert.AreEqual(DateTime.Today, obj.Data);
            Assert.AreEqual(1.23, obj.Valor);
            Assert.AreEqual(2, obj.Oid);
            Assert.AreEqual("teste foo", obj.Foo);
            Assert.AreEqual("teste bar", obj.Bar);

            string serializedObject = JsonConvert.SerializeObject(obj,  new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = 2,

            });

            Assert.IsTrue(!string.IsNullOrWhiteSpace(serializedObject));
        }



        [TestMethod]
        public void GetPropertyTest()
        {
            dynamic obj = new ExpandoObject();
            obj.Nome = "vitor";

            Assert.AreEqual("vitor", obj.Nome);
            Assert.IsTrue((obj as IDictionary<string, object>).ContainsKey("Nome"));
            Assert.IsTrue((obj as IDictionary<string, object>).TryGetValue("Nome", out object Nome));
            Assert.AreEqual("vitor", Nome);

            AssertHelper.AssertThrows(() => {
                Assert.AreEqual(null, obj.Foo);
            });

            Assert.IsFalse((obj as IDictionary<string, object>).ContainsKey("Foo"));
            Assert.IsFalse((obj as IDictionary<string, object>).TryGetValue("Foo", out object Foo));
            Assert.AreEqual(null, Foo);

        }

        [TestMethod]
        public void DictionaryTest()
        {
            dynamic dic = new ExpandoObject();

            object nome = null;
            AssertHelper.AssertThrows(() =>
            {
                nome = (dic as IDictionary<string, object>)["Nome"];
            });
            Assert.IsNull(nome);

            var teste = (dic as IDictionary<string, object>).TryGetValue("Nome", out nome);
            Assert.IsFalse(teste);
            Assert.IsNull(nome);

            //(dic as IDictionary<string, object>).Add("Nome", "vitor");
            (dic as IDictionary<string, object>)["Nome"] = "vitor";
            (dic as IDictionary<string, object>)["Nome"] = "vitor";
            AssertHelper.AssertThrows(() =>
            {
                (dic as IDictionary<string, object>).Add("Nome", "vitor");
            });
        }


    }
}
