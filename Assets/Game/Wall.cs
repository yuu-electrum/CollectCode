using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Playable
{
    /// <summary>
    /// 壁を管理するクラス
    /// </summary>
    public class Wall : MonoBehaviour
    {
        [SerializeField]
        private GameObject _root;

        [SerializeField]
        private GameObject _origin;

        [SerializeField]
        private FarViewportToWorldPoint _wpViewpoint;

        public void Start()
        {
            _wpViewpoint.UpdatePosition();

            /*
            var origScale = _origin.transform.localScale;
            origScale.x = _wpViewpoint.TopLeft.y - _wpViewpoint.BottomLeft.y;

            var obj = Instantiate(_origin, _root.transform);
            obj.transform.Rotate(0.0f, 0.0f, -90.0f);

            // カメラの描画境界ぴったりに置く
            var pos = new Vector3(
                _wpViewpoint.TopLeft.x,
                _wpViewpoint.TopLeft.y - (origScale.x / 2.0f),
                _wpViewpoint.TopLeft.z
            );

            obj.transform.position   = pos;
            obj.transform.localScale = origScale;
            */
        }
    }
}