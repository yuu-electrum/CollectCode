using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 時間を表すクラス
    /// </summary>
    public class Timer : MonoBehaviour
    {
        private float _elapsedTime = 0.0f;

        /// <summary>
        /// シーンが始まってからの時間を取得する
        /// </summary>
        public float ElapsedTime { get { return _elapsedTime; } }

        public void Update()
        {
            _elapsedTime += Time.deltaTime;
        }
    }
}