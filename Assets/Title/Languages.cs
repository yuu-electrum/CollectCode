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
            var objs     = new List<GameObject>(GameObject.FindGameObjectsWithTag("NationalFlag"));
            var buttons  = new List<Button>();

            objs.ForEach((x) => { buttons.Add(x.GetComponent<Button>()); });

            if(objs == null) yield break;

            var frame = 0;

            // 40フレームかけて透明になっていく
            for(;;)
            {
                foreach(var btn in buttons)
                {
                    var pos = btn.gameObject.transform.position;
                    pos.x -= 4.0f;
                    btn.gameObject.transform.position = pos;

                    var colors = btn.colors;
                    colors.normalColor = new Color(1.0f, 1.0f, 1.0f, 1 * (float)(frame / 40));
                    btn.colors = colors;
                }

                frame++;
                if(frame == 40)
                {
                    isComplete = true;
                    yield break;
                }

                yield return null;
            }
        }

        public bool IsComplete { get { return isComplete; } }
    }
}