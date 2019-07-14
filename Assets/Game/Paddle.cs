using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Playable
{
    /// <summary>
    /// プレイヤーが動かせる板
    /// </summary>
    public class Paddle : MonoBehaviour
    {
        [SerializeField]
        private FarViewportToWorldPoint _wpViewport;

        private const float VELOCITY = 15.0f; // 移動速度

        public void Start()
        {
            var pos = new Vector3();

            _wpViewport.UpdatePosition();

            // 画面の中央に配置する
            pos.x = _wpViewport.TopLeft.x + ((_wpViewport.TopRight.x - _wpViewport.TopLeft.x) / 2.0f);
            pos.y = _wpViewport.BottomLeft.y;
            pos.z = _wpViewport.TopLeft.z;

            this.transform.position = pos;
        }

        public void Update()
        {
            var pos = this.transform.position;

            // Shiftを押している間は移動速度を落とす
            var modifier = (Input.GetKey(KeyCode.LeftShift)) ? 0.25f : 1.0f;

            if(Input.GetKey(KeyCode.LeftArrow))
            {
                // 左に移動
                pos.x -= VELOCITY * modifier;
            } else if(Input.GetKey(KeyCode.RightArrow)) {
                // 右に移動
                pos.x += VELOCITY * modifier;
            }

            Vector3 vp = _wpViewport.Camera.WorldToViewportPoint(pos);

            // Viewportの範囲外にいるなら直す
            if(vp.x < 0.0f) { vp.x = 0.0f; }
            if(vp.x > 1.0f) { vp.x = 1.0f; }

            this.transform.position = _wpViewport.Camera.ViewportToWorldPoint(vp);
        }
    }
}