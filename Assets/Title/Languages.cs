using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Title
{
    /// <summary>
    /// 言語選択一覧を表示するクラス
    /// </summary>
    public class Languages : MonoBehaviour, IFade
    {
        private bool isComplete = false;

        [SerializeField]
        private FarViewportToWorldPoint _wpViewport;

        public IEnumerator StartFadein()
        {
            var rectTrans = this.GetComponent<RectTransform>();
            for(;;)
            {
                var pos = rectTrans.localPosition;
                pos.x += 2.0f;
                rectTrans.localPosition = pos;

                if(pos.x > -375.0f) { yield break; }

                yield return null;
            }
        }

        public IEnumerator StartFadeout()
        {
            var rb = this.gameObject.AddComponent<Rigidbody>();
            rb.AddForce(new Vector3(0.0f, 128.0f, 0.0f));

            for(;;)
            {
                
            }
        }

        public bool IsComplete { get { return isComplete; } }
    }
}