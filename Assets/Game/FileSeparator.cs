using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Preprocessor
{
    /// <summary>
    /// ソースコードを適当な単位に分解するクラス
    /// </summary>
    public class FileSeparator
    {
        private string _raw;
        private string _encoding;
        private Dictionary<int, string> _chunkedFile;

        /// <summary>
        /// <param name="filePath">読み込むべきファイルパス</param>
        /// </summary>
        public FileSeparator(string filePath)
        {
            _raw = System.IO.File.ReadAllText(filePath);
        }

        /// <summary>
        /// ファイルを指定された単位で分割する
        /// <param name="unit">分割する単位文字数</param>
        /// </summary>
        public Dictionary<int, string> ChunkFiles(int unit)
        {
            unit = (_raw.Length < unit) ? _raw.Length : unit;
            if(unit == 0) { return null; }

            var results = new Dictionary<int, string>();

            for(int i = 0; i < _raw.Length; i++)
            {
                var start = i * unit;
                var end   = (i + 1) * unit;

                if(end > _raw.Length) { break; }

                results.Add(i, _raw.Substring(start, unit));
            }

            return results;
        } 
    }
}