using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Title
{
    public class StartButton : MonoBehaviour, IFade
    {

        private GameObject _me;
        private Transform _transform;
        private bool isFadedOut = false;

        [SerializeField]
        private GenericButton _button;

        public void Start()
        {
            _me = this.gameObject;
            _transform = _me.transform;
        }

        public IEnumerator StartFadein()
        {
            // GenericButtonのTextコンポーネントを取得する
            Text t = _button.Label;
            if(t == null) { yield break; }

            // ボタンのラベルのパラメータを取得する
            int frame = 0;
            Color col = t.color;

            // 最初は透明
            col.a = 0.0f;

            // nフレームで完全に不透明になる
            const int PERIOD = 160;

            for(;;)
            {
                col.a = 1.0f * ((float)frame / (float)PERIOD);
                t.color = col;
                frame++;

                if(frame > PERIOD) { yield break; }
                yield return null;
            }
        }

        public IEnumerator StartFadeout()
        {
            var pos = this.transform.localPosition;

            // 単に下にやるだけ
            for(;;)
            {
                pos.y -= 2.5f;
                this.transform.localPosition = pos;

                yield return null;

                if(pos.y < -800.0f)
                {
                    isFadedOut = true;
                    yield break;
                }
            }
        }

        public bool IsComplete { get { return isFadedOut; } }
    }
}