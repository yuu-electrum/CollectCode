using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Title
{
    /// <summary>
    /// タイトルロゴを担当するクラス
    /// </summary>
    public class Logo : MonoBehaviour, Game.IFade
    {
        private const string TITLE_LOGO      = "CollectCode";
        private const float  FADEIN_INTERVAL = 0.25f;

        private int currentColumn = 1;

        [SerializeField]
        private Text _fadingLogo;

        public void Start()
        {
            _fadingLogo.text = "";
        }

        public IEnumerator StartFadein()
        {
            // 徐々にタイトルを表示する
            for(;;)
            {
                yield return new WaitForSeconds(FADEIN_INTERVAL);

                _fadingLogo.text = TITLE_LOGO.Substring(0, currentColumn);
                currentColumn++;
                if(currentColumn > TITLE_LOGO.Length) { yield break; }
            }
        }

        public IEnumerator StartFadeout()
        {
            yield break;
        }

        public bool IsComplete
        {
            get { return false; }
        }
    }
}