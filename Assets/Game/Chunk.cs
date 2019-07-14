using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Playable
{
    /// <summary>
    /// ソースコードのチャンクを表すクラス
    /// </summary>
    public class Chunk : MonoBehaviour, IDestroyable
    {
        private int _index;

        [SerializeField]
        private TextMesh _text;

        public void Initialize(int index)
        {
            _index = index;
        }

        public TextMesh Text  { get { return _text;  } }
        public int ChunkIndex { get { return _index; } }
        public void Destroy() { Destroy(this.gameObject); }
    }
}