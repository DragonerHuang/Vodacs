using Microsoft.Extensions.Configuration;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Vodace.Core.Enums;
using Vodace.Core.ManageUser;

namespace Vodace.Core.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private readonly string _localizationPath;
        private readonly Dictionary<string, Dictionary<string, string>> _localizations;
        private string _currentLanguage;

        public LocalizationService(IConfiguration configuration)
        {
            _localizationPath = configuration["LocalizationSettings:Path"] ?? "Localization";
            _localizations = new Dictionary<string, Dictionary<string, string>>();
            _currentLanguage = configuration["LocalizationSettings:DefaultLanguage"] ?? "zh-CN";
            LoadLanguageFiles();
        }

        private void LoadLanguageFiles()
        {
            if (!Directory.Exists(_localizationPath))
            {
                Directory.CreateDirectory(_localizationPath);
            }

            var files = Directory.GetFiles(_localizationPath, "*.json");
            foreach (var file in files)
            {
                var cultureName = Path.GetFileNameWithoutExtension(file);
                var jsonString = File.ReadAllText(file);
                var translations = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString);
                _localizations[cultureName] = translations;
            }
        }

        public string GetString(string key)
        {
            //取用户配置的语言类型
            switch (UserContext.Current.Lang)
            {
                case (int)LangType.zh_CN:
                    _currentLanguage = "zh-CN";
                    break;
                case (int)LangType.en_US:
                    _currentLanguage = "en-US";
                    break;
                default:
                    _currentLanguage = "zh-CN";
                    break;
            }
            var res = _localizations.TryGetValue(_currentLanguage, out var translations);

            if (res && translations.TryGetValue(key, out var translation))
            {
                if (UserContext.Current.Lang == (int)LangType.zh_TW || UserContext.Current.Lang==null)
                {
                    string traditional = ChineseConverter.Convert(translation, ChineseConversionDirection.SimplifiedToTraditional);
                    return traditional;
                }
                else
                {
                    return translation;
                }
            }
            return key;
        }

        public string GetString(string key, params object[] args)
        {
            //var translation = GetString(key);
            //return args != null && args.Length > 0 ? string.Format(translation, args) : translation;

            switch (UserContext.Current.Lang)
            {
                case (int)LangType.zh_CN:
                    _currentLanguage = "zh-CN";
                    break;
                case (int)LangType.en_US:
                    _currentLanguage = "en-US";
                    break;
                default:
                    _currentLanguage = "zh-CN";
                    break;
            }

            // 获取翻译文本
            var res = _localizations.TryGetValue(_currentLanguage, out var translations);
            if (res && translations.TryGetValue(key, out var translation))
            {
                if (args != null && args.Length > 0)
                {
                    translation = string.Format(translation, args);
                }

                string result = translation;

                if (UserContext.Current.Lang == (int)LangType.zh_TW || UserContext.Current.Lang == null)
                {
                    result = ChineseConverter.Convert(translation, ChineseConversionDirection.SimplifiedToTraditional);
                }
                return result;
            }

            return key;
        }

        public void SetLanguage(string cultureName)
        {
            if (_localizations.ContainsKey(cultureName))
            {
                _currentLanguage = cultureName;
            }
            else
            {
                throw new ArgumentException($"Language {cultureName} is not available.");
            }
        }

        public string GetCurrentLanguage()
        {
            return _currentLanguage;
        }

        public List<string> GetAvailableLanguages()
        {
            return new List<string>(_localizations.Keys);
        }
    }
}