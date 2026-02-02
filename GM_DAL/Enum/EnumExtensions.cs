using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System;

namespace GM_DAL.Enum
{
    public static class EnumExtensions
    {
        public static string GetDescription(this System.Enum GenericEnum) //Hint: Change the method signature and input paramter to use the type parameter T
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="seft"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IN(this object seft, params object[] list)
        {
            try
            {
                return list.Contains(seft);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
