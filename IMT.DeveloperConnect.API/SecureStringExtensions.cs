using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API
{
    public static class SecureStringExtensions
    {
        public static SecureString ToSecureString(this string value)
        {
            if (value == null)
                return null;
            var secureString = new SecureString();
            foreach (char c in value) secureString.AppendChar(c);
            return secureString;
        }

        public static string AsString(this SecureString value)
        {
            if (value == null)
                return null;
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }
    }
}
