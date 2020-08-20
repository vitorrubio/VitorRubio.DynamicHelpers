using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VitorRubio.DynamicHelpers
{
    public class BaseElasticObject :
        DynamicObject,
        IDictionary<string, object>,
        ICollection<KeyValuePair<string, object>>,
        IEnumerable<KeyValuePair<string, object>>,
        IEnumerable,
        INotifyPropertyChanged
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
            var thisKeys = this.ToDictionary().Select(x => x.Key);
            return thisKeys.Union(base.GetDynamicMemberNames());
        }

        //
        // Resumo:
        //     Provides a System.Dynamic.DynamicMetaObject that dispatches to the dynamic virtual
        //     methods. The object can be encapsulated inside another System.Dynamic.DynamicMetaObject
        //     to provide custom behavior for individual actions. This method supports the Dynamic
        //     Language Runtime infrastructure for language implementers and it is not intended
        //     to be used directly from your code.
        //
        // Parâmetros:
        //   parameter:
        //     The expression that represents System.Dynamic.DynamicMetaObject to dispatch to
        //     the dynamic virtual methods.
        //
        // Devoluções:
        //     An object of the System.Dynamic.DynamicMetaObject type.
        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return base.GetMetaObject(parameter);
        }

        //
        // Resumo:
        //     Provides implementation for binary operations. Classes derived from the System.Dynamic.DynamicObject
        //     class can override this method to specify dynamic behavior for operations such
        //     as addition and multiplication.
        //
        // Parâmetros:
        //   binder:
        //     Provides information about the binary operation. The binder.Operation property
        //     returns an System.Linq.Expressions.ExpressionType object. For example, for the
        //     sum = first + second statement, where first and second are derived from the DynamicObject
        //     class, binder.Operation returns ExpressionType.Add.
        //
        //   arg:
        //     The right operand for the binary operation. For example, for the sum = first
        //     + second statement, where first and second are derived from the DynamicObject
        //     class, arg is equal to second.
        //
        //   result:
        //     The result of the binary operation.
        //
        // Devoluções:
        //     true if the operation is successful; otherwise, false. If this method returns
        //     false, the run-time binder of the language determines the behavior. (In most
        //     cases, a language-specific run-time exception is thrown.)
        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object result)
        {
            return base.TryBinaryOperation(binder, arg, out result);
        }

        //
        // Resumo:
        //     Provides implementation for type conversion operations. Classes derived from
        //     the System.Dynamic.DynamicObject class can override this method to specify dynamic
        //     behavior for operations that convert an object from one type to another.
        //
        // Parâmetros:
        //   binder:
        //     Provides information about the conversion operation. The binder.Type property
        //     provides the type to which the object must be converted. For example, for the
        //     statement (String)sampleObject in C# (CType(sampleObject, Type) in Visual Basic),
        //     where sampleObject is an instance of the class derived from the System.Dynamic.DynamicObject
        //     class, binder.Type returns the System.String type. The binder.Explicit property
        //     provides information about the kind of conversion that occurs. It returns true
        //     for explicit conversion and false for implicit conversion.
        //
        //   result:
        //     The result of the type conversion operation.
        //
        // Devoluções:
        //     true if the operation is successful; otherwise, false. If this method returns
        //     false, the run-time binder of the language determines the behavior. (In most
        //     cases, a language-specific run-time exception is thrown.)
        public override bool TryConvert(ConvertBinder binder, out object result)
        {
            return base.TryConvert(binder, out result);
        }

        //
        // Resumo:
        //     Provides the implementation for operations that initialize a new instance of
        //     a dynamic object. This method is not intended for use in C# or Visual Basic.
        //
        // Parâmetros:
        //   binder:
        //     Provides information about the initialization operation.
        //
        //   args:
        //     The arguments that are passed to the object during initialization. For example,
        //     for the new SampleType(100) operation, where SampleType is the type derived from
        //     the System.Dynamic.DynamicObject class, args[0] is equal to 100.
        //
        //   result:
        //     The result of the initialization.
        //
        // Devoluções:
        //     true if the operation is successful; otherwise, false. If this method returns
        //     false, the run-time binder of the language determines the behavior. (In most
        //     cases, a language-specific run-time exception is thrown.)
        public override bool TryCreateInstance(CreateInstanceBinder binder, object[] args, out object result)
        {
            return base.TryCreateInstance(binder, args, out result);
        }

        //
        // Resumo:
        //     Provides the implementation for operations that delete an object by index. This
        //     method is not intended for use in C# or Visual Basic.
        //
        // Parâmetros:
        //   binder:
        //     Provides information about the deletion.
        //
        //   indexes:
        //     The indexes to be deleted.
        //
        // Devoluções:
        //     true if the operation is successful; otherwise, false. If this method returns
        //     false, the run-time binder of the language determines the behavior. (In most
        //     cases, a language-specific run-time exception is thrown.)
        public override bool TryDeleteIndex(DeleteIndexBinder binder, object[] indexes)
        {
            return base.TryDeleteIndex(binder, indexes);
        }

        //
        // Resumo:
        //     Provides the implementation for operations that delete an object member. This
        //     method is not intended for use in C# or Visual Basic.
        //
        // Parâmetros:
        //   binder:
        //     Provides information about the deletion.
        //
        // Devoluções:
        //     true if the operation is successful; otherwise, false. If this method returns
        //     false, the run-time binder of the language determines the behavior. (In most
        //     cases, a language-specific run-time exception is thrown.)
        public override bool TryDeleteMember(DeleteMemberBinder binder)
        {
            return base.TryDeleteMember(binder);
        }

        //
        // Resumo:
        //     Provides the implementation for operations that get a value by index. Classes
        //     derived from the System.Dynamic.DynamicObject class can override this method
        //     to specify dynamic behavior for indexing operations.
        //
        // Parâmetros:
        //   binder:
        //     Provides information about the operation.
        //
        //   indexes:
        //     The indexes that are used in the operation. For example, for the sampleObject[3]
        //     operation in C# (sampleObject(3) in Visual Basic), where sampleObject is derived
        //     from the DynamicObject class, indexes[0] is equal to 3.
        //
        //   result:
        //     The result of the index operation.
        //
        // Devoluções:
        //     true if the operation is successful; otherwise, false. If this method returns
        //     false, the run-time binder of the language determines the behavior. (In most
        //     cases, a run-time exception is thrown.)
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            return base.TryGetIndex(binder, indexes, out result);
        }

        //
        // Resumo:
        //     Provides the implementation for operations that get member values. Classes derived
        //     from the System.Dynamic.DynamicObject class can override this method to specify
        //     dynamic behavior for operations such as getting a value for a property.
        //
        // Parâmetros:
        //   binder:
        //     Provides information about the object that called the dynamic operation. The
        //     binder.Name property provides the name of the member on which the dynamic operation
        //     is performed. For example, for the Console.WriteLine(sampleObject.SampleProperty)
        //     statement, where sampleObject is an instance of the class derived from the System.Dynamic.DynamicObject
        //     class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies
        //     whether the member name is case-sensitive.
        //
        //   result:
        //     The result of the get operation. For example, if the method is called for a property,
        //     you can assign the property value to result.
        //
        // Devoluções:
        //     true if the operation is successful; otherwise, false. If this method returns
        //     false, the run-time binder of the language determines the behavior. (In most
        //     cases, a run-time exception is thrown.)
        // If you try to get a value of a property
        // not defined in the class, this method is called.
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if ((this as IDictionary<string, object>).ContainsKey(binder.Name))
            {
                return (this as IDictionary<string, object>).TryGetValue(binder.Name, out result);
            }
            else
            {
                return base.TryGetMember(binder, out result);
            }
        }

        //
        // Resumo:
        //     Provides the implementation for operations that invoke an object. Classes derived
        //     from the System.Dynamic.DynamicObject class can override this method to specify
        //     dynamic behavior for operations such as invoking an object or a delegate.
        //
        // Parâmetros:
        //   binder:
        //     Provides information about the invoke operation.
        //
        //   args:
        //     The arguments that are passed to the object during the invoke operation. For
        //     example, for the sampleObject(100) operation, where sampleObject is derived from
        //     the System.Dynamic.DynamicObject class, args[0] is equal to 100.
        //
        //   result:
        //     The result of the object invocation.
        //
        // Devoluções:
        //     true if the operation is successful; otherwise, false. If this method returns
        //     false, the run-time binder of the language determines the behavior. (In most
        //     cases, a language-specific run-time exception is thrown.
        public override bool TryInvoke(InvokeBinder binder, object[] args, out object result)
        {
            return base.TryInvoke(binder, args, out result);
        }

        //
        // Resumo:
        //     Provides the implementation for operations that invoke a member. Classes derived
        //     from the System.Dynamic.DynamicObject class can override this method to specify
        //     dynamic behavior for operations such as calling a method.
        //
        // Parâmetros:
        //   binder:
        //     Provides information about the dynamic operation. The binder.Name property provides
        //     the name of the member on which the dynamic operation is performed. For example,
        //     for the statement sampleObject.SampleMethod(100), where sampleObject is an instance
        //     of the class derived from the System.Dynamic.DynamicObject class, binder.Name
        //     returns "SampleMethod". The binder.IgnoreCase property specifies whether the
        //     member name is case-sensitive.
        //
        //   args:
        //     The arguments that are passed to the object member during the invoke operation.
        //     For example, for the statement sampleObject.SampleMethod(100), where sampleObject
        //     is derived from the System.Dynamic.DynamicObject class, args[0] is equal to 100.
        //
        //   result:
        //     The result of the member invocation.
        //
        // Devoluções:
        //     true if the operation is successful; otherwise, false. If this method returns
        //     false, the run-time binder of the language determines the behavior. (In most
        //     cases, a language-specific run-time exception is thrown.)
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if ((this as IDictionary<string, object>).ContainsKey(binder.Name) && (this as IDictionary<string, object>)[binder.Name] is Delegate)
            {
                result = ((this as IDictionary<string, object>)[binder.Name] as Delegate).DynamicInvoke(args);
                return true;
            }
            else
            {
                return base.TryInvokeMember(binder, args, out result);
            }
        }

        //
        // Resumo:
        //     Provides the implementation for operations that set a value by index. Classes
        //     derived from the System.Dynamic.DynamicObject class can override this method
        //     to specify dynamic behavior for operations that access objects by a specified
        //     index.
        //
        // Parâmetros:
        //   binder:
        //     Provides information about the operation.
        //
        //   indexes:
        //     The indexes that are used in the operation. For example, for the sampleObject[3]
        //     = 10 operation in C# (sampleObject(3) = 10 in Visual Basic), where sampleObject
        //     is derived from the System.Dynamic.DynamicObject class, indexes[0] is equal to
        //     3.
        //
        //   value:
        //     The value to set to the object that has the specified index. For example, for
        //     the sampleObject[3] = 10 operation in C# (sampleObject(3) = 10 in Visual Basic),
        //     where sampleObject is derived from the System.Dynamic.DynamicObject class, value
        //     is equal to 10.
        //
        // Devoluções:
        //     true if the operation is successful; otherwise, false. If this method returns
        //     false, the run-time binder of the language determines the behavior. (In most
        //     cases, a language-specific run-time exception is thrown.
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            return base.TrySetIndex(binder, indexes, value);
        }

        //
        // Resumo:
        //     Provides the implementation for operations that set member values. Classes derived
        //     from the System.Dynamic.DynamicObject class can override this method to specify
        //     dynamic behavior for operations such as setting a value for a property.
        //
        // Parâmetros:
        //   binder:
        //     Provides information about the object that called the dynamic operation. The
        //     binder.Name property provides the name of the member to which the value is being
        //     assigned. For example, for the statement sampleObject.SampleProperty = "Test",
        //     where sampleObject is an instance of the class derived from the System.Dynamic.DynamicObject
        //     class, binder.Name returns "SampleProperty". The binder.IgnoreCase property specifies
        //     whether the member name is case-sensitive.
        //
        //   value:
        //     The value to set to the member. For example, for sampleObject.SampleProperty
        //     = "Test", where sampleObject is an instance of the class derived from the System.Dynamic.DynamicObject
        //     class, the value is "Test".
        //
        // Devoluções:
        //     true if the operation is successful; otherwise, false. If this method returns
        //     false, the run-time binder of the language determines the behavior. (In most
        //     cases, a language-specific run-time exception is thrown.)
        // If you try to set a value of a property that is
        // not defined in the class, this method is called.
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (!(this as IDictionary<string, object>).ContainsKey(binder.Name))
            {
                (this as IDictionary<string, object>).Add(binder.Name, value);
            }
            else
            {
                (this as IDictionary<string, object>)[binder.Name] = value;
            }

            return true;
        }

        //
        // Resumo:
        //     Provides implementation for unary operations. Classes derived from the System.Dynamic.DynamicObject
        //     class can override this method to specify dynamic behavior for operations such
        //     as negation, increment, or decrement.
        //
        // Parâmetros:
        //   binder:
        //     Provides information about the unary operation. The binder.Operation property
        //     returns an System.Linq.Expressions.ExpressionType object. For example, for the
        //     negativeNumber = -number statement, where number is derived from the DynamicObject
        //     class, binder.Operation returns "Negate".
        //
        //   result:
        //     The result of the unary operation.
        //
        // Devoluções:
        //     true if the operation is successful; otherwise, false. If this method returns
        //     false, the run-time binder of the language determines the behavior. (In most
        //     cases, a language-specific run-time exception is thrown.)
        public override bool TryUnaryOperation(UnaryOperationBinder binder, out object result)
        {
            return base.TryUnaryOperation(binder, out result);
        }

        #endregion


        #region Implementação de IDictionary<string, object>

        object IDictionary<string, object>.this[string key]
        {
            get
            {
                var p = this.GetProperties().Where(x => x.Name == key)?.FirstOrDefault();
                if (p != null && (p is PropertyInfo))
                {
                    return p.GetValue(this);
                }
                return _dictionary[key];
            }
            set
            {
                var p = this.GetProperties().Where(x => x.Name == key && x.PropertyType.IsAssignableFrom(value.GetType()))?.FirstOrDefault();
                if (p != null && (p is PropertyInfo))
                {
                    p.SetValue(this, value);
                }
                else
                {
                    _dictionary[key] = value;
                }

            }
        }

        void IDictionary<string, object>.Add(string key, object value)
        {
            var p = this.GetProperties().Where(x => x.Name == key && x.PropertyType.IsAssignableFrom(value.GetType()))?.FirstOrDefault();
            if (p != null && (p is PropertyInfo))
            {
                p.SetValue(this, value);
            }
            else
            {
                _dictionary.Add(key, value);
            }
        }

        bool IDictionary<string, object>.ContainsKey(string key)
        {
            return this.GetProperties().Any(x => x.Name == key) || _dictionary.ContainsKey(key);
        }

        bool IDictionary<string, object>.Remove(string key)
        {

            if (_dictionary.ContainsKey(key))
            {
                return _dictionary.Remove(key);
            }
            return false;
        }

        bool IDictionary<string, object>.TryGetValue(string key, out object value)
        {
            var p = this.GetProperties().Where(x => x.Name == key)?.FirstOrDefault();
            if (p != null && (p is PropertyInfo))
            {
                value = p.GetValue(this);
                return true;
            }

            return _dictionary.TryGetValue(key, out value);
        }

        ICollection<string> IDictionary<string, object>.Keys => this.ToDictionary().Select(x => x.Key).ToList();

        ICollection<object> IDictionary<string, object>.Values => this.ToDictionary().Select(x => x.Value).ToList(); 

        #endregion



        #region Implementação de ICollection<KeyValuePair<string, object>>


        bool ICollection<KeyValuePair<string, object>>.IsReadOnly => ((ICollection<KeyValuePair<string, object>>)_dictionary).IsReadOnly;

        int ICollection<KeyValuePair<string, object>>.Count => this.GetCount();

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            (this as IDictionary<string, object>).Add(item.Key, item.Value);
            //((ICollection<KeyValuePair<string, object>>)_dictionary).Add(item);
        }

        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            _dictionary.Clear();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return _dictionary.Contains(item);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            (_dictionary as ICollection<KeyValuePair<string, object>>).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            return (this as IDictionary<string, object>).Remove(item.Key);
        }

        #endregion



        #region Implementação de IEnumerable<KeyValuePair<string, object>>

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return this.ToDictionary().GetEnumerator();
        }

        #endregion



        #region Implementação de IEnumerable

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.ToDictionary().GetEnumerator();
        }

        #endregion



        #region Implementação de INotifyPropertyChanged
        //event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        //{
        //    add
        //    {
        //        throw new NotImplementedException();
        //    }

        //    remove
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
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

        private int GetCount()
        {
            return this.GetProperties().Count + _dictionary.Count;
            //return this.ToDictionary().Count();
        }

        #endregion

    }
}
