using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Game.Localization;

namespace Game.UI
{
    /// <summary>
    /// ゲームオーバーになった・ゲームクリアした時の画面を表すクラス
    /// </summary>
    public class FinishedCanvas : MonoBehaviour
    {
        [SerializeField]
        private Text _text;

        [SerializeField]
        private Text _retrybutton;

        [SerializeField]
        private Text _backToTitle;

        public Text Text       { get { return _text;   } }
        public Text RetryLabel { get { return _retrybutton; } } 
        public Text BackToTitleLabel { get { return _backToTitle; } }

        public void OnClickRetry()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

        public void OnClickBackToTitle()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}