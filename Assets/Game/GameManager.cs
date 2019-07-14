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

        [SerializeField]
        LifeManager _life;

        // 再生成したチャンク数
        public List<GameObject> _generatedChunks;

        // ゲームが終了しているか
        private bool _finished = false;

        // ゲームオーバーになったか
        private bool _isGameover = false;

        public void Start()
        {
            // .metaファイルを除いたソースコードを列挙する
            var directories = new List<string>(
                System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/SourcecodeFiles/")
            );
            var targets = directories.Where(str => str.IndexOf(".meta", StringComparison.Ordinal) < 0).ToList();
            var index   = UnityEngine.Random.Range(0, targets.Count);
            _factory.Initialize(targets[index]);

            _fader.Initialize(_fadingObjs);
            _generatedChunks = new List<GameObject>();

            // 初期残機は3
            _life.Initialize(2);

            // 最初のボールを生成する
            _ball.Generate();
        }

        public void Update()
        {
            //--------------------------------------------
            // ミスした時の処理
            //--------------------------------------------
            if(_ball.IsAllOutOfStage && _factory.CurrentState != ChunkFactory.State.FINISHED && !_isGameover && !_life.IsGameOver)
            {
                // 残機が残っているなら新たにボールを出す
                _life.Kill();
                _ball.Generate();
                return;
            }

            //--------------------------------------------
            // ゲームオーバー時の処理
            //--------------------------------------------
            if(_ball.IsAllOutOfStage && _factory.CurrentState != ChunkFactory.State.FINISHED && !_isGameover)
            {
                // ゲームオーバーになった瞬間に、残っている文字列をすべて落とす下向きベクトルを加える
                _generatedChunks.ForEach((x) => {
                    var rb = x.GetComponent<Rigidbody>();
                    Destroy(x.GetComponent<BoxCollider>());

                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.AddForce(new Vector3(0.0f, -1024.0f, 0.0f));
                });

                _isGameover = true;
                _fader.StartFadeout();
                return;
            }

            //--------------------------------------------
            // ゲーム続行中の処理
            //--------------------------------------------
            if(_generatedChunks.Count == 0 && _factory.CurrentState != ChunkFactory.State.FINISHED)
            {
                // 次のフェーズが取得できなかったらゲーム終了
                var objs = _factory.GetNextPhase(_chunkParent.transform);
                if(objs == null) return;

                _organizer.Organize(objs);
                _generatedChunks.AddRange(objs);
            }

            //--------------------------------------------
            // ゲームクリアの処理
            //--------------------------------------------
            if(!_finished && _factory.CurrentState == ChunkFactory.State.FINISHED)
            {
                // ゲームの終了処理をする
                _finished = true;
                _fader.StartFadeout();
            }

            _generatedChunks.RemoveAll(obj => obj == null);
        }

        public void OnDestroy()
        {
            PlayerPrefs.Save();
        }
    }
}