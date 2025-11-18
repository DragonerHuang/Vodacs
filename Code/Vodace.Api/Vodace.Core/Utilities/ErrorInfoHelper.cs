using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Vodace.Core.Utilities
{
    public static class ErrorInfoHelper
    {
        /// <summary>
        /// 获取当前实例方法的完整错误信息字符串
        /// </summary>
        /// <param name="instance">调用方法的实例对象</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>格式化的错误信息字符串</returns>
        public static string GetErrorInfo(object instance, string errorMessage)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            Type type = instance.GetType();
            string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

            // 组合命名空间.类名.方法名.Error格式的字符串
            return $"{type.Namespace}.{type.Name}.{methodName}.Error: {errorMessage}";
        }

        /// <summary>
        /// 获取静态方法的完整错误信息字符串
        /// </summary>
        /// <param name="type">包含静态方法的类型</param>
        /// <param name="methodName">方法名</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns>格式化的错误信息字符串</returns>
        public static string GetStaticMethodErrorInfo(Type type, string methodName, string errorMessage)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            // 组合命名空间.类名.方法名.Error格式的字符串
            return $"{type.Namespace}.{type.Name}.{methodName}.Error: {errorMessage}";
        }
    }
}
