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
        private Dictionary<int, string> _chunkedFile;

        /// <summary>
        /// <param name="filePath">読み込むべきファイルパス</param>
        /// </summary>
        public FileSeparator(string filePath)
        {
            _raw = System.IO.File.ReadAllText(filePath);
        }
    }
}