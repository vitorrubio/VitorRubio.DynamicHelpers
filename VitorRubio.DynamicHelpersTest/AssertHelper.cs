using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VitorRubio.DynamicHelpersTest
{
    public static class AssertHelper
    {
        public static void AssertThrows<T>(Action act, bool exactException = true, string message = "") where T : Exception
        {
            AssertThrows(act, typeof(T), exactException, message);
        }

        public static void AssertThrows(Action act, Type exceptionType = null, bool exactException = true, string message = "")
        {
            try
            {
                act();
                if (!string.IsNullOrWhiteSpace(message))
                {
                    Assert.Fail(message);
                }
                else
                {
                    Assert.Fail();
                }

            }
            catch (Exception err)
            {
                if (exceptionType == null)
                {
                    return;
                }
                else
                {
                    if (exactException)
                    {
                        if (!string.IsNullOrWhiteSpace(message))
                        {
                            Assert.AreEqual(exceptionType, err.GetType(), message);
                        }
                        else
                        {
                            Assert.AreEqual(exceptionType, err.GetType());
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(message))
                        {
                            Assert.IsTrue(exceptionType.IsAssignableFrom(err.GetType()), message);
                        }
                        else
                        {
                            Assert.IsTrue(exceptionType.IsAssignableFrom(err.GetType()));
                        }
                    }
                }
            }
        }
    }
}
