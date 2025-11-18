using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Extensions
{
    public static class EnumExtensions
    {
        #region 转化

        /// <summary>
        /// 将int转换为指定枚举类型（不检查有效性，可能返回无效枚举值）
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">整数</param>
        /// <returns>对应的枚举值</returns>
        public static T ToEnum<T>(this int value) where T : struct, Enum
        {
            // 直接强制转换（简单但不安全，当int值无对应枚举成员时仍会返回值）
            return (T)(object)value;
        }

        /// <summary>
        /// 安全地将int转换为指定枚举类型（带有效性检查）
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">整数</param>
        /// <param name="result">转换成功的枚举值</param>
        /// <returns>是否转换成功</returns>
        public static bool TryConvertToEnum<T>(this int value, out T result) where T : struct, Enum
        {
            // 检查值是否在枚举定义的范围内
            if (Enum.IsDefined(typeof(T), value))
            {
                result = (T)(object)value;
                return true;
            }

            result = default;
            return false;
        }

        /// <summary>
        /// 安全地将int转换为指定枚举类型（带异常处理）
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">整数</param>
        /// <returns>对应的枚举值</returns>
        /// <exception cref="ArgumentException">当int值无对应枚举成员时抛出</exception>
        public static T ConvertToEnum<T>(this int value) where T : struct, Enum
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                throw new ArgumentException($"整数 {value} 不是枚举 {typeof(T).Name} 的有效成员");
            }

            return (T)(object)value;
        }

        #endregion

        /// <summary>
        /// 获取枚举成员的Description属性值
        /// </summary>
        /// <param name="enumValue">枚举值</param>
        /// <returns>如果存在Description则返回其值，否则返回枚举的ToString()结果</returns>
        public static string GetDescription(this Enum enumValue)
        {
            // 获取枚举成员的类型信息
            MemberInfo[] memberInfo = enumValue.GetType().GetMember(enumValue.ToString());

            if (memberInfo.Length > 0)
            {
                // 尝试获取Description属性
                object[] attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributes.Length > 0)
                {
                    // 如果存在Description属性，则返回其值
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }

            // 如果没有Description属性，则返回枚举的字符串表示
            return enumValue.ToString();
        }

        /// <summary>
        /// 根据枚举类型和Description获取对应的枚举值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="description">Description属性值</param>
        /// <returns>对应的枚举值</returns>
        public static T GetEnumValueByDescription<T>(string description) where T : struct, Enum
        {
            foreach (var enumValue in Enum.GetValues<T>())
            {
                if (enumValue.GetDescription() == description)
                {
                    return enumValue;
                }
            }

            throw new ArgumentException($"在枚举 {typeof(T).Name} 中未找到Description为 {description} 的成员");
        }
    }
}
