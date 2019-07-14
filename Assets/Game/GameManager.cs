using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Game.Playable
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        Timer _timer;

        [SerializeField]
        ChunkFactory _factory;

        [SerializeField]
        ChunkOrganizer _organizer;

        [SerializeField]
        GameObject _chunkParent;

        private SortedDictionary<int, string> _collectedChunks;

        public void Start()
        {
            // .metaファイルを除いたソースコードを列挙する
            var directories = new List<string>(
                System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/SourcecodeFiles/")
            );

            var targets = directories.Where(str => str.IndexOf(".meta", StringComparison.Ordinal) < 0).ToList();

            // 列挙されたコードの中から一つを選ぶ
            var index   = UnityEngine.Random.Range(0, targets.Count);

            _factory.Initialize(targets[index]);

            _collectedChunks = new SortedDictionary<int, string>();
        }
    }
}