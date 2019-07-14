using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 途中で解像度が変わった時のためのViewportを監視するためのクラス
    /// </summary>
    [RequireComponent(typeof(FarViewportToWorldPoint))]
    public class FarViewportObserver : MonoBehaviour
    {
        [SerializeField]
        private FarViewportToWorldPoint _wpViewport;

        public void Start()
        {
            _wpViewport.UpdatePosition();
        }

        public void Update()
        {
            _wpViewport.UpdatePosition();
        }
    }
}