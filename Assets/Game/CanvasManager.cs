using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Game.Localization;
using Game.Playable;

namespace Game.UI
{
    /// <summary>
    /// それぞれのFactory・Managerを監視しUIとして表示するクラス
    /// </summary>
    public class CanvasManager : MonoBehaviour
    {
        #region Models

        private Localizer _localizer;

        [SerializeField]
        public ChunkFactory _chunkFactory;

        #endregion

        #region Views

        [SerializeField]
        public Text _currentPhase;

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

            if(_chunkFactory.CurrentState == ChunkFactory.State.FINISHED) { _currentPhase.gameObject.SetActive(false); }
        }
    }
}