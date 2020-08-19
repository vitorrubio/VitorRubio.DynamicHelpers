using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitorRubio.DynamicHelpers
{

    public class DynamicObjectSample : DynamicObject
    {

        #region fields privados

        Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        #endregion



        #region propriedades públicas

        public int Oid { get; set; }

        #endregion



        #region métodos públicos

        #region implementação e sobrecarga de DynamicObject

        /// <summary>
        /// When a new property is set, 
        /// add the property name and value to the dictionary
        /// </summary>     
        public override bool TrySetMember (SetMemberBinder binder, object value)
        {
            if (!_dictionary.ContainsKey(binder.Name))
                _dictionary.Add(binder.Name, value);
            else
                _dictionary[binder.Name] = value;

            return true;
        }

        /// <summary>
        /// When user accesses something, return the value if we have it
        /// </summary>      
        public override bool TryGetMember (GetMemberBinder binder, out object result)  
        {
            if (_dictionary.ContainsKey(binder.Name))
            {
                result = _dictionary[binder.Name];
                return true;
            }
            else
            {
                return base.TryGetMember(binder, out result);
            }
        }

        /// <summary>
        /// If a property value is a delegate, invoke it
        /// </summary>     
        public override bool TryInvokeMember (InvokeMemberBinder binder, object[] args, out object result)
        {
            if (_dictionary.ContainsKey(binder.Name) && _dictionary[binder.Name] is Delegate)   
            {
                result = (_dictionary[binder.Name] as Delegate).DynamicInvoke(args);
                return true;
            }
            else
            {
                return base.TryInvokeMember(binder, args, out result);
            }
        }


        /// <summary>
        /// Return all dynamic member names
        /// </summary>
        /// <returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            var keys = _dictionary.Keys;

            var props = (from p in this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                         where !p.GetIndexParameters().Any()
                         select p.Name);

            return keys.Union(props);
        }

        #endregion


        #region metodos sobrecarregados padrão object

        public override string ToString()
        {
            string serializedObject = JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MaxDepth = 2,

            });

            return serializedObject;
        }

        #endregion

        #endregion





    }
}
