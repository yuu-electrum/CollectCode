using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// Farなカメラの描画範囲を取得するクラス
    /// </summary>
    public class FarViewportToWorldPoint : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        private Vector3 _topLeft;
        private Vector3 _topRight;
        private Vector3 _bottomLeft;
        private Vector3 _bottomRight;

        public Vector3 TopLeft     { get { return _topLeft;     } }
        public Vector3 TopRight    { get { return _topRight;    } }
        public Vector3 BottomLeft  { get { return _bottomLeft;  } }
        public Vector3 BottomRight { get { return _bottomRight; } }

        public void UpdateViewport()
        {
            if(_camera == null) { return; }

            _topLeft     = _camera.ViewportToWorldPoint(new Vector3(0.0f, 1.0f, _camera.farClipPlane));
            _topRight    = _camera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, _camera.farClipPlane));
            _bottomLeft  = _camera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, _camera.farClipPlane));
            _bottomRight = _camera.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, _camera.farClipPlane));
        }
    }
}