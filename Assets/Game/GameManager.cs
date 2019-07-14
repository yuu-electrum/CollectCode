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
        BallFactory _ball;

        [SerializeField]
        GameObject _chunkParent;

        [SerializeField]
        GameObject[] _fadingObjs;

        [SerializeField]
        Fader _fader;

        // 再生成したチャンク数
        public List<GameObject> _generatedChunks;

        // ゲームが終了しているか
        private bool _finished = false;

        public void Start()
        {
            // .metaファイルを除いたソースコードを列挙する
            var directories = new List<string>(
                System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/SourcecodeFiles/")
            );

            var targets = directories.Where(str => str.IndexOf(".meta", StringComparison.Ordinal) < 0).ToList();

            // 列挙されたコードの中から一つを選ぶ
            var index   = UnityEngine.Random.Range(0, targets.Count);

            // チャンクの生成準備をする
            _factory.Initialize(targets[index]);

            _fader.Initialize(_fadingObjs);

            // 最初のボールを生成する
            _ball.Generate();

            _generatedChunks = new List<GameObject>();
        }

        public void Update()
        {
            Debug.Log(_factory.CurrentState);

            if(_generatedChunks.Count == 0 && _factory.CurrentState != ChunkFactory.State.FINISHED)
            {
                // 次のフェーズが取得できなかったらゲーム終了
                var objs = _factory.GetNextPhase();
                if(objs == null) return;

                _organizer.Organize(objs);
                _generatedChunks.AddRange(objs);
            }

            if(!_finished && _factory.CurrentState == ChunkFactory.State.FINISHED)
            {
                // ゲームの終了処理をする
                _finished = true;
                _fader.StartFadeout();
            }

            _generatedChunks.RemoveAll(obj => obj == null);
        }
    }
}