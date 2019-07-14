﻿using System;
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
        private const int MAX_PHASE = 1;
        private const int UNIT      = 1;

        public enum State { HAS_REMAINING_PHASE, NO_REMAINING_PHASE, FINISHED }

        private Dictionary<int, string> _chunks;
        private List<int> _generatedChunkIndexes;

        private int _currentChunk;
        private int _currentPhase;
        private int _unit;

        [SerializeField]
        private GameObject _origin;

        public int MaxPhase     { get { return MAX_PHASE;     } }
        public int CurrentPhase { get { return _currentPhase; } }

        public void Initialize(string filePath)
        {
            _currentPhase = 0;   

            _chunks = new Preprocessor.FileSeparator(filePath).ChunkFiles(UNIT);
            _generatedChunkIndexes = new List<int>();
        }

        /// <summary>
        /// すべてのチャンクリストを取得する
        /// </summary>
        public Dictionary<int, string> GetAllChunks()
        {
            return _chunks;
        }

        /// <summary>
        /// 次のフェーズのチャンクを生成する
        /// </summary>
        public List<GameObject> GetNextPhase()
        {
            _currentPhase++;

            if(CurrentState == State.FINISHED) { return null; }

            // まだ生成されていないチャンクを選ぶ
            var unselected = _chunks.Where((item) => {
                return !_generatedChunkIndexes.Contains(item.Key);
            }).ToList();

            // まだ生成されていないチャンクの中から選ぶ
            // TODO: めっちゃ短い時にはUNITの値を変えなければならない
            var pendings = new List<int>();
            while(pendings.Count < UNIT)
            {
                int index = UnityEngine.Random.Range(0, unselected.Count - 1);
                if(pendings.Contains(index)) continue;

                pendings.Add(index);
            }

            // 生成済みリストに追加する
            _generatedChunkIndexes.AddRange(pendings);

            var newlyGeneratedChunks = new List<GameObject>();

            foreach(var idx in pendings)
            {
                var obj = Instantiate(_origin);

                obj.GetComponent<Chunk>().Text.text = _chunks[idx];
                newlyGeneratedChunks.Add(obj);
            }

            return newlyGeneratedChunks;
        }

        /// <summary>
        /// ゲームの状態を返すメソッド
        /// </summary>
        public State CurrentState
        {
            get
            {
                if(CurrentPhase <  MAX_PHASE) { return State.HAS_REMAINING_PHASE; }
                if(CurrentPhase == MAX_PHASE) { return State.NO_REMAINING_PHASE ; }
                return State.FINISHED;
            }
        }
    }
}