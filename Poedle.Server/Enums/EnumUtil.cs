using System.Globalization;

namespace Poedle.Enums
{
    public static class EnumUtil
    {
        public static T GetEnumByName<T>(string pName, Dictionary<string, T> pExceptions) where T : Enum
        {
            if (pExceptions.TryGetValue(pName, out T? value))
            {
                return value;
            }

            return GetEnumByName<T>(pName);
        }

        public static T GetEnumByName<T>(string pName) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), pName, true);
        }

        public static string GetNameByValue<T>(T pEnum, Dictionary<T, string> pExceptions) where T : Enum
        {
            if (pExceptions.TryGetValue(pEnum, out string? value))
            {
                return value;
            }

            return GetNameByValue(pEnum);
        }

        public static string GetNameByValue<T>(T pEnum) where T : Enum
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            return textInfo.ToTitleCase(nameof(pEnum));
        }
    }
}
