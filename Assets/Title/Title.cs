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

        public void Start()
        {
            // ローカライザーの設定
            _localizer = new Localization.Localizer();

            _localizer.LoadLocalize(Application.streamingAssetsPath + "/Languages/ja.txt", "ja");
            _localizer.Language = "ja";

            _startButton.Label.text = _localizer.GetLocalize("StartButton");

            // フェードインを開始する
            if(_fadingObjs.Length <= 0) { return; }
            foreach(var obj in _fadingObjs)
            {
                IFade f;
                if((f = obj.GetComponent<IFade>()) == null) continue;

                StartCoroutine(f.StartFadein());
            }
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
