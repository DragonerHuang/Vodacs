using System.Collections.Generic;

namespace Vodace.Core.Localization
{
    public interface ILocalizationService
    {
        /// <summary>
        /// 获取当前语言的翻译文本
        /// </summary>
        /// <param name="key">翻译键值</param>
        /// <returns>翻译后的文本</returns>
        string GetString(string key);

        /// <summary>
        /// 获取当前语言的翻译文本（带格式化参数）
        /// </summary>
        /// <param name="key">翻译键值</param>
        /// <param name="args">格式化参数</param>
        /// <returns>翻译后的文本</returns>
        string GetString(string key, params object[] args);

        /// <summary>
        /// 切换语言
        /// </summary>
        /// <param name="cultureName">语言文化名称（如：zh-CN, en-US）</param>
        void SetLanguage(string cultureName);

        /// <summary>
        /// 获取当前语言文化名称
        /// </summary>
        /// <returns>当前语言文化名称</returns>
        string GetCurrentLanguage();

        /// <summary>
        /// 获取所有可用的语言列表
        /// </summary>
        /// <returns>语言列表</returns>
        List<string> GetAvailableLanguages();
    }
}