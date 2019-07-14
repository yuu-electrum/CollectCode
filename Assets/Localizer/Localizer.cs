using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Game.Localization
{
    public class Localizer
    {
        /// <summary>
        /// ローカライズデータを維持するコレクション
        /// </summary>
        private Dictionary<string, Dictionary<string, string>> _localizes;

        /// <summary>
        /// 現在設定されている言語
        /// </summary>
        private string currentLanguage = null;

        /// <summary>
        /// ローカライズデータを取得する
        /// <returns>ローカライズされた文字列。識別子に対応するローカライズがないか、既定の言語が設定されていない場合は空文字。</returns>
        /// <param name="key">データの識別子</param>
        /// </summary>
        public string GetLocalize(string key)
        {
            bool hasCurrentLanguageAndKey =
                currentLanguage != null                 &&
                _localizes.ContainsKey(currentLanguage) &&
                _localizes[currentLanguage].ContainsKey(key);

            return (hasCurrentLanguageAndKey) ? _localizes[currentLanguage][key] : "";
        }

        /// <summary>
        /// 書式付き文字列をフォーマットして返す
        /// </summary>
        public string GetFormattedString(string key, params object[] args)
        {
            return string.Format(GetLocalize(key), args);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Localizer()
        {
            _localizes = new Dictionary<string, Dictionary<string, string>>();
        }

        /// <summary>
        /// ローカライズデータを読み込む
        /// </summary>
        /// <returns>ファイルが存在しない場合はfalse。成功したらtrue。</returns>
        public bool LoadLocalize(string localizeTextFilePath, string lang)
        {
            if(!System.IO.File.Exists(localizeTextFilePath))
            {
                return false;
            }

            // 生データ
            var raw = System.IO.File.ReadAllLines(localizeTextFilePath);
            _localizes.Add(lang, new Dictionary<string, string>());

            // 各行を空白で区切りコレクションに入れる
            foreach(var line in raw)
            {
                var center = line.IndexOf(" ");

                var key = line.Substring(0, center);
                var val = line.Substring(center + 1);

                _localizes[lang].Add(key, val);
            }

            return true;
        }

        /// <summary>
        /// 既定の言語を取得または設定する
        /// </summary>
        public string Language
        {
            get { return currentLanguage;  }
            set { currentLanguage = value; }
        }
    }
}