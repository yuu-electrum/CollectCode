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

        #region Models

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

        #endregion

        public void Start()
        {
            _localizer = new Localizer();

            _localizer.LoadLocalize(Application.streamingAssetsPath + "/Languages/ja.txt", "ja");
            _localizer.Language = "ja";
        }

        public void Update()
        {
            _currentPhase.text = _localizer.GetFormattedString(
                "CurrentPhase",
                _chunkFactory.CurrentPhase,
                _chunkFactory.MaxPhase
            );

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