using System;

namespace NotifierService.Infrastructure
{
    public static class StringExtensions
    {
        public static TTo SafeConvert<TTo>(this string val)
        {
            try
            {
                return (TTo) Convert.ChangeType(val, typeof (TTo));
            }
            catch (Exception)
            {
                return default(TTo);
            }
        }
    }
}
