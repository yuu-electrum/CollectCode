using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Playable
{
    /// <summary>
    /// ボールを表すクラス
    /// </summary>
    public class Ball : MonoBehaviour
    {
        private Vector3 _dir;
        private float   _speed;

        public float Speed
        {
            get { return _speed;  }
            set { _speed = value; }
        }

        public void Start()
        {
            Speed = 8.0f;
            _dir = new Vector3(-1.0f, -1.0f, 0.0f).normalized * Speed;
        }

        public void Update()
        {
            transform.position += _dir;
        }

        public void OnTriggerEnter(Collider other)
        {
            if(other.tag == "CodeChunk")
            {
                // 破壊可能なオブジェクトであった場合は削除する
                other.gameObject.GetComponent<IDestroyable>().Destroy();
                return;
            }

            if(other.tag == "Ball")
            {
                return;
            }

            // ボールを反射させる
            var colliderVec = other.transform.up;
            float distance  = Mathf.Abs(Vector3.Dot(_dir, colliderVec));

            _dir = _dir + 2.0f * colliderVec * distance;
        }
    }
}