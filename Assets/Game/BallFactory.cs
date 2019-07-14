using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Playable
{
    /// <summary>
    /// ボールを管理するクラス
    /// </summary>
    public class BallFactory : MonoBehaviour
    {
        [SerializeField]
        private FarViewportToWorldPoint _wpViewport;

        [SerializeField]
        private GameObject _origin;

        public void Start()
        {
            _wpViewport.UpdatePosition();

            var obj = Instantiate(_origin);
            var pos = new Vector3(
                _wpViewport.TopLeft.x + (_wpViewport.TopRight.x - _wpViewport.TopLeft.x) / 2.0f,
                _wpViewport.BottomLeft.y + 256.0f,
                _wpViewport.TopLeft.z
            );

            obj.transform.position = pos;
        }
    }
}