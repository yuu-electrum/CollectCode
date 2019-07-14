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
            set
            {
                _speed = value;
                _dir = new Vector3(-1.0f, -1.0f, 0.0f).normalized * Speed;
            }
        }

        public void Start()
        {
            Speed = 1.0f;
        }

        public void Update()
        {
            transform.position += _dir;
        }

        public void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);

            // ボールを反射させる
            var colliderVec = other.transform.up;
            float distance  = Mathf.Abs(Vector3.Dot(_dir, colliderVec));

            _dir = _dir + 2.0f * colliderVec * distance;
        }
    }
}