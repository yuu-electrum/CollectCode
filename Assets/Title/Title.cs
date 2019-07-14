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

            // 言語設定をPlayerPrefsから読み込む
            var lang = PlayerPrefs.GetString("Language", "ja");

            _localizer.LoadLocalize(string.Format(Application.streamingAssetsPath + "/Languages/{0}.txt", lang), lang);
            _localizer.Language = lang;

            _startButton.Label.text = _localizer.GetLocalize("StartButton");
            
            _fader.Initialize(_fadingObjs);
            _fader.StartFadein();
        }

        public void Update()
        {
            // フェードアウトがすべて終わった時点で画面を遷移させる
            if(_fader.InCompleteAllObjectFadeout)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(1);
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

        public void ChangeLanguage(string lang)
        {
            PlayerPrefs.SetString("Language", lang);
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
