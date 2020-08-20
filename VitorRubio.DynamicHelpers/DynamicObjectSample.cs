using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VitorRubio.DynamicHelpers
{

    public class DynamicObjectSample : DynamicObject
    {

        #region fields privados

        private List<PropertyInfo> _props;
        private Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        #endregion


        #region métodos públicos

        public virtual Dictionary<string, object> ToDictionary() //ou seria melhor AsDictionary?
        {
            var props = this.GetProperties().Select(x => new { Key = x.Name, Value = x.GetValue(this) }).ToDictionary(k => k.Key, v => v.Value);

            var result = props
                .Concat(_dictionary)
                .GroupBy(d => d.Key)
                .ToDictionary(d => d.Key, d => d.First().Value);

            return result;
        }


        #region implementação e sobrecarga de DynamicObject


        /// <summary>
        ///  Resumo:
        ///      Returns the enumeration of all dynamic member names.
        /// 
        ///  Devoluções:
        ///      A sequence that contains dynamic member names.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            var thisEnumerableKeys = this.ToDictionary().Select(x => x.Key);
            return thisEnumerableKeys.Union(base.GetDynamicMemberNames());
        }

        /// <summary>
        /// 
        ///  Resumo:
        ///      Provides the implementation for operations that get member values. Classes derived
        ///      from the System.Dynamic.DynamicObject class can override this method to specify
        ///      dynamic behavior for operations such as getting a value for a property.
        /// 
        ///  Parâmetros:
        ///    binder:
        ///      Provides information about the object that called the dynamic operation. The
        ///      binder.Name property provides the name of the member on which the dynamic operation
        ///      is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty)
        ///      statement, where sampleObject is an instance of the class derived from the System.Dynamic.DynamicObject
        ///      class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies
        ///      whether the member name is case-sensitive.
        /// 
        ///    result:
        ///      The result of the get operation. For example, if the method is called for a property,
        ///      you can assign the property value to result.
        /// 
        ///  Devoluções:
        ///      true if the operation is successful; otherwise, false. If this method returns
        ///      false, the run-time binder of the language determines the behavior. (In most
        ///      cases, a run-time exception is thrown.)
        ///  If you try to get a value of a property
        ///  not defined in the class, this method is called.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_dictionary.ContainsKey(binder.Name))
            {
                return _dictionary.TryGetValue(binder.Name, out result);
            }
            else
            {
                return base.TryGetMember(binder, out result);
            }
        }

        /// <summary>
        /// 
        ///  Resumo:
        ///      Provides the implementation for operations that invoke a member. Classes derived
        ///      from the System.Dynamic.DynamicObject class can override this method to specify
        ///      dynamic behavior for operations such as calling a method.
        /// 
        ///  Parâmetros:
        ///    binder:
        ///      Provides information about the dynamic operation. The binder.Name property provides
        ///      the name of the member on which the dynamic operation is performed. For example,
        ///      for the statement sampleObject.SampleMethod(100), where sampleObject is an instance
        ///      of the class derived from the System.Dynamic.DynamicObject class, binder.Name
        ///      returns "SampleMethod". The binder.IgnoreCase property specifies whether the
        ///      member name is case-sensitive.
        /// 
        ///    args:
        ///      The arguments that are passed to the object member during the invoke operation.
        ///      For example, for the statement sampleObject.SampleMethod(100), where sampleObject
        ///      is derived from the System.Dynamic.DynamicObject class, args[0] is equal to 100.
        /// 
        ///    result:
        ///      The result of the member invocation.
        /// 
        ///  Devoluções:
        ///      true if the operation is successful; otherwise, false. If this method returns
        ///      false, the run-time binder of the language determines the behavior. (In most
        ///      cases, a language-specific run-time exception is thrown.)
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
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
        /// 
        ///  Resumo:
        ///      Provides the implementation for operations that set member values. Classes derived
        ///      from the System.Dynamic.DynamicObject class can override this method to specify
        ///      dynamic behavior for operations such as setting a value for a property.
        /// 
        ///  Parâmetros:
        ///    binder:
        ///      Provides information about the object that called the dynamic operation. The
        ///      binder.Name property provides the name of the member to which the value is being
        ///      assigned. For example, for the statement sampleObject.SampleProperty = "Test",
        ///      where sampleObject is an instance of the class derived from the System.Dynamic.DynamicObject
        ///      class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies
        ///      whether the member name is case-sensitive.
        /// 
        ///    value:
        ///      The value to set to the member. For example, for sampleObject.SampleProperty
        ///      = "Test", where sampleObject is an instance of the class derived from the System.Dynamic.DynamicObject
        ///      class, the value is "Test".
        /// 
        ///  Devoluções:
        ///      true if the operation is successful; otherwise, false. If this method returns
        ///      false, the run-time binder of the language determines the behavior. (In most
        ///      cases, a language-specific run-time exception is thrown.)
        ///  If you try to set a value of a property that is
        ///  not defined in the class, this method is called.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!_dictionary.ContainsKey(binder.Name))
            {
                _dictionary.Add(binder.Name, value);
            }
            else
            {
                _dictionary[binder.Name] = value;
            }

            return true;
        }

        #endregion

        #region metodos sobrecarregados padrão object

        public override string ToString()
        {
            // If you need special handling, you can call another form of SerializeObject below
            string serializedObject = JsonConvert.SerializeObject(this as dynamic, new JsonSerializerSettings
            {
                //não usar o serialize ReferenceLoopHandling.Serialize em conjunto com  PreserveReferencesHandling.None, NUNCA. 
                //ou use  o ReferenceLoopHandling.Ignore ou o PreserveReferencesHandling.Objects

                //ContractResolver = new CamelCasePropertyNamesContractResolver(), //sem alterar fica o DefaultContractResolver
                //Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                //PreserveReferencesHandling = PreserveReferencesHandling.None,
                MaxDepth = 2,

            });

            return serializedObject;
        }

        #endregion

        #endregion


        #region métodos privados

        private IList<PropertyInfo> GetProperties()
        {
            if (_props == null || _props.Count == 0)
            {
                _props = (from p in this.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                          where !p.GetIndexParameters().Any()
                          select p).ToList();
            }

            return _props;
        }

        #endregion

    }
}
