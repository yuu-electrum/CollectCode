using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Title
{
    public class Title : MonoBehaviour
    {
        private Game.Localization.Localizer _localizer;

        [SerializeField]
        private GenericButton _startButton;

        [SerializeField]
        private GameObject[] _fadingObjs;

        [SerializeField]
        private GameObject _cursor;

        [SerializeField]
        private Fader _fader;

        public void Start()
        {
            // ローカライザーの設定
            _localizer = new Localization.Localizer();

            _localizer.LoadLocalize(Application.streamingAssetsPath + "/Languages/ja.txt", "ja");
            _localizer.Language = "ja";

            _startButton.Label.text = _localizer.GetLocalize("StartButton");
            
            _fader.Initialize(_fadingObjs);
            _fader.StartFadein();
        }

        public void Update()
        {
            // フェードアウトがすべて終わった時点で画面を遷移させる
            if(_fader.InCompleteAllObjectFadeout)
            {
                
            }
        }

        public void OnClickStartButton()
        {
            _cursor.SetActive(false);
            _fader.StartFadeout();
        }

        public void OnPointerEnterAtStartButton()
        {
            _cursor.SetActive(true);
        }

        public void OnPointerExitAtStartButton()
        {
            _cursor.SetActive(false);
        }
    }
}
