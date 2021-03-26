using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API
{
    public static class ArgumentChecks
    {
        public static T IsNotNull<T>(T value, string parameterName)
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
            return value;
        }

        public static string IsNotNullOrEmpty(string value, string parameterName)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(parameterName);
            return value;
        }

        public static SecureString ValueOrDefault(SecureString value, Func<SecureString> defaultFactory)
        {
            if (value == null || value.Length == 0)
                return defaultFactory();
            else
                return value;
        }
    }
}
