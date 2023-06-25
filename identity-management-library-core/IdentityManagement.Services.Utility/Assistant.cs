//
//  Assistant.cs
//
//  Wiregrass Code Technology 2020-2023
//
using System.Globalization;
using System.Reflection;

namespace IdentityManagement.Services.Utility
{
    public static class Assistant
    {
        public static string GetBooleanValue(bool value)
        {
            return value ? "true" : "false";
        }

        public static int GetIntegerValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            return int.TryParse(value, out var number) ? number : 0;
        }

        public static double GetFloatingPointValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0.0;
            }
            return double.TryParse(value, out var number) ? number : 0.0;
        }

        public static string? GetMethodFullName(MethodBase methodInfo)
        {
            if (methodInfo == null)
            {
                return string.Empty;
            }
            return methodInfo.DeclaringType != null ? string.Format(CultureInfo.InvariantCulture, "{0}.{1}()", methodInfo.DeclaringType.FullName, methodInfo.Name) : null;
        }
    }
}