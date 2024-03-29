﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

        private List<GameObject> _balls;

        public void Start()
        {
            _balls = new List<GameObject>();
        }

        public void Update()
        {
            if(IsAllOutOfStage)
            {
                _balls.ForEach((x) => { Destroy(x.gameObject); });
                _balls.Clear();
            }
        }

        public void Generate()
        {
            var obj = Instantiate(_origin);
            var pos = new Vector3(
                _wpViewport.TopLeft.x + (_wpViewport.TopRight.x - _wpViewport.TopLeft.x) / 2.0f,
                _wpViewport.TopLeft.y - 32.0f,
                _wpViewport.TopLeft.z
            );

            obj.transform.position = pos;
            _balls.Add(obj);
        }

        public bool IsAllOutOfStage
        {
            get
            {
                return _balls.All((b) => {
                    var vp = _wpViewport.Camera.WorldToViewportPoint(b.transform.position);
                    return vp.y < 0.0f;
                });
            }
        }
    }
}