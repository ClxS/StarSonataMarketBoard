namespace SSMB.Blazor.Utility
{
    using System;
    using System.ComponentModel;

    public static class EnumExtensions
    {
        public static string ToDescription(this Enum @this)
        {
            var type = @this.GetType();

            var memInfo = type.GetMember(@this.ToString());

            if (memInfo == null || memInfo.Length <= 0)
            {
                return @this.ToString();
            }

            var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attrs != null && attrs.Length > 0)
            {
                return ((DescriptionAttribute)attrs[0]).Description;
            }

            return @this.ToString();
        }
    }
}