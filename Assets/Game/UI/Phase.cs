using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Game.UI
{
    /// <summary>
    /// フェーズを示す文字列を管理するクラス
    /// </summary>
    public class Phase : MonoBehaviour, IFade
    {
        private bool isFadedOut = false;

        public IEnumerator StartFadein()
        {
            yield break;
        }

        public IEnumerator StartFadeout()
        {
            this.gameObject.SetActive(false);
            isFadedOut = true;
            yield break;
        }

        public bool IsComplete { get { return isFadedOut; } }
    }
}