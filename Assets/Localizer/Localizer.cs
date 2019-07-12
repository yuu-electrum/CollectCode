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
        private Dictionary<string, string> _localizes;

        /// <summary>
        /// ローカライズデータを取得する
        /// </summary>
        /// <param name="key">データの識別子</param>
        public string GetLocalize(string key)
        {
            return (_localizes.ContainsKey(key)) ? _localizes[key] : "A localization with a specified identifier seems not defined";
        }

        /// <summary>
        /// ローカライズデータを読み込む
        /// </summary>
        /// <param name="localizeTextFilePath">読み込むローカライズテキストデータのパス</param>
        public Localizer(string localizeTextFilePath)
        {
            // 生データ
            var raw = System.IO.File.ReadAllLines(localizeTextFilePath);

            _localizes = new Dictionary<string, string>();

            // 各行を空白で区切りコレクションに入れる
            foreach(var line in raw)
            {
                var ss = line.Split(" ".ToCharArray());
                if(ss.Length <= 1) { continue; }

                _localizes.Add(ss[0], ss[1]);
            }
        }
    }
}