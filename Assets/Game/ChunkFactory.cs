using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Game.Playable
{
    /// <summary>
    /// 分割されたソースコードをGameObjectとしてInstantiateするクラス
    /// </summary>
    public class ChunkFactory : MonoBehaviour
    {
        private Dictionary<int, string> _chunks;
        private int _currentChunk;

        [SerializeField]
        private GameObject _origin;

        public void Initialize(string filePath)
        {
            _chunks = new Preprocessor.FileSeparator(filePath).ChunkFiles(4);
        }

        /// <summary>
        /// 次のチャンク群を取得する
        /// </summary>
        /// <param name="chunkCount">取得するチャンク数</param>
        public List<GameObject> GetNextChunks(int chunkCount, Transform dest = null)
        {
            var start = _currentChunk;
            var end   = _currentChunk + chunkCount;

            if(end >= _chunks.Count) { return null; }

            var objs       = new List<GameObject>();
            var defaultPos = new Vector3(0.0f, 0.0f, 0.0f);

            foreach(var c in _chunks)
            {
                if(c.Key < start || c.Key > end - 1) { continue; }

                // 範囲内にあるチャンク
                var newObj = (dest == null) ? Instantiate(_origin) : Instantiate(_origin, dest);

                // チャンクを識別するための情報を付与する
                var comp = newObj.GetComponent<Chunk>();

                comp.Initialize(c.Key);
                comp.Text.text = c.Value;

                objs.Add(newObj);
            }

            _currentChunk = end;
            return objs;
        }
    }
}