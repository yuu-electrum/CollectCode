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

            var verticalScale   = _wpViewpoint.TopLeft.y  - _wpViewpoint.BottomLeft.y;
            var horizontalScale = _wpViewpoint.TopRight.x - _wpViewpoint.TopLeft.x;

            var walls = new List<WallObject>()
            {
                // 左
                new WallObject(
                    new Vector3(_wpViewpoint.TopLeft.y - _wpViewpoint.BottomLeft.y, 0.0f, 0.0f),
                    new Vector3(
                        _wpViewpoint.TopLeft.x,
                        _wpViewpoint.TopLeft.y - verticalScale / 2.0f,
                        _wpViewpoint.TopLeft.z
                    ),
                    new Vector3(0.0f, 0.0f, -90.0f)
                ),
                // 右
                new WallObject(
                    new Vector3(_wpViewpoint.TopLeft.y - _wpViewpoint.BottomLeft.y, 0.0f, 0.0f),
                    new Vector3(
                        _wpViewpoint.TopRight.x,
                        _wpViewpoint.TopLeft.y - verticalScale / 2.0f,
                        _wpViewpoint.TopLeft.z
                    ),
                    new Vector3(0.0f, 0.0f, 90.0f)
                ),
                // 上
                new WallObject(
                    new Vector3(horizontalScale, 0.0f, 0.0f),
                    new Vector3(
                        _wpViewpoint.TopRight.x - horizontalScale / 2.0f,
                        _wpViewpoint.TopLeft.y,
                        _wpViewpoint.TopLeft.z
                    ),
                    new Vector3(0.0f, 0.0f, -180.0f)
                )
            };

            foreach(var w in walls)
            {
                var obj = Instantiate(_origin, _root.transform);
                obj.transform.Rotate(w.Rotation);

                obj.transform.position   = w.Position;
                obj.transform.localScale = w.Scale;
            }

            /*
            
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

    public class WallObject
    {
        public Vector3 Scale      { get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }

        public WallObject(Vector3 scale, Vector3 pos, Vector3 rotation)
        {
            Scale    = scale;
            Position = pos;
            Rotation = rotation;
        }
    }
}