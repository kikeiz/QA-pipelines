namespace QA.ClientInterface.Utils
{
    class EnumParser
    {
        public static TEnum ParseEnum<TEnum>(string value, bool ignoreCase = true, TEnum defaultValue = default)
        where TEnum : struct, Enum
        {
            if (Enum.TryParse(value, ignoreCase, out TEnum result))
            {
                return result;
            }
            return defaultValue;        }

    }
}