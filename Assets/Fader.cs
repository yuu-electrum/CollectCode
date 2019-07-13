using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// フェードイン/アウトすべきGameObjectをまとめてフェードするラッパー
    /// </summary>
    public class Fader : MonoBehaviour
    {
        private List<IFade> _objs;

        public void Initialize(GameObject[] fadeObjs)
        {
            _objs = new List<IFade>();

            foreach(var obj in fadeObjs)
            {
                IFade c = obj.GetComponent<IFade>();
                if(c == null) continue;

                _objs.Add(c);
            }
        }

        public void StartFadein()
        {
            foreach(var o in _objs)
            {
                StartCoroutine(o.StartFadein());
            }
        }

        public void StartFadeout()
        {
            foreach(var o in _objs)
            {
                StartCoroutine(o.StartFadeout());
            }
        }

        /// <summary>
        /// すべてのオブジェクトのフェードアウトが終わったかどうかを取得するプロパティ
        /// </summary>
        /// <returns>終わっていればtrue、さもなくばfalse。</returns>
        public bool InCompleteAllObjectFadeout
        {
            get { return _objs.All(obj => obj.IsComplete); }
        }
    }
}