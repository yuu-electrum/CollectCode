using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Game.Localization;
using Game.Playable;
using System.Text;

namespace Game.UI
{
    /// <summary>
    /// それぞれのFactory・Managerを監視しUIとして表示するクラス
    /// </summary>
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _originalGauge;

        [SerializeField]
        private GameObject _originalFinishedCanvas;

        #region Models

        [SerializeField]
        private GameManager _gameManager;

        private Localizer _localizer;

        [SerializeField]
        public ChunkFactory _chunkFactory;

        [SerializeField]
        public LifeManager _life; 

        #endregion

        #region Views

        [SerializeField]
        public Text _currentPhase;

        [SerializeField]
        public GameObject _lifeGauge;

        [SerializeField]
        public Text _remaining;

        #endregion

        private bool _isFinishedCanvasShown = false;

        public void Start()
        {
            _localizer = new Localizer();

            var lang         = PlayerPrefs.GetString("Language", "ja");
            var localizePath = string.Format(Application.streamingAssetsPath + "/Languages/{0}.txt", lang);

            _localizer.LoadLocalize(localizePath, lang);
            _localizer.Language = lang;
        }

        public void Update()
        {
            if(_isFinishedCanvasShown)  { return; }

            if(_chunkFactory.CurrentState == ChunkFactory.State.FINISHED && !_gameManager.IsGameover)
            {
                // ゲームクリアの画面を表示する
                var obj = Instantiate(_originalFinishedCanvas, this.transform).GetComponent<FinishedCanvas>();
                obj.Text.text             = _localizer.GetLocalize("Finish");
                obj.RetryLabel.text       = _localizer.GetLocalize("Retry");
                obj.BackToTitleLabel.text = _localizer.GetLocalize("BackToTitle");

                _isFinishedCanvasShown = true;
                _remaining.gameObject.SetActive(false);
                _lifeGauge.SetActive(false);
                return;
            }

            if(_gameManager.IsGameover)
            {
                // ゲームオーバーの画面を表示する
                var obj = Instantiate(_originalFinishedCanvas, this.transform).GetComponent<FinishedCanvas>();
                obj.Text.text             = _localizer.GetLocalize("GameOver");
                obj.RetryLabel.text       = _localizer.GetLocalize("Retry");
                obj.BackToTitleLabel.text = _localizer.GetLocalize("BackToTitle");

                _isFinishedCanvasShown = true;
                _remaining.gameObject.SetActive(false);
                _lifeGauge.SetActive(false);
                return;
            }

            _currentPhase.text = _localizer.GetFormattedString(
                "CurrentPhase",
                _chunkFactory.CurrentPhase,
                _chunkFactory.MaxPhase
            );

            //=====================================================
            // 残りのチャンク数を表示する
            //=====================================================
            _remaining.text = _localizer.GetFormattedString("Remaining", _gameManager.CurrentChunkCount);

            //=====================================================
            // 残機を表示する
            //=====================================================
            var gauges = GameObject.FindGameObjectsWithTag("LifeGauge").ToList();
            var diff = _life.Life - gauges.Count;

            if(diff > 0)
            {
                for(int i = 0; i < diff; i++)
                {
                    Instantiate(_originalGauge, _lifeGauge.transform).tag = "LifeGauge";
                }
            }

            if(diff < 0)
            {
                var deletedGauges = gauges.GetRange(0, Math.Abs(diff));
                foreach(var g in deletedGauges)
                {
                    Destroy(g);
                }
            }
        }
    }
}