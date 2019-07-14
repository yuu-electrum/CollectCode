using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Game.Playable
{
    /// <summary>
    /// カメラのViewport内にGameObjectを並べるクラス
    /// </summary>
    public class ChunkOrganizer : MonoBehaviour
    {
        [SerializeField]
        FarViewportToWorldPoint _wpViewport;

        public void Start()
        {
            _wpViewport.UpdatePosition();
        }

        public void Organize(List<GameObject> organizedObjects)
        {
            foreach(var obj in organizedObjects)
            {
                var newPos = new Vector3(
                    UnityEngine.Random.Range(_wpViewport.TopLeft.x, _wpViewport.TopRight.x),
                    UnityEngine.Random.Range(_wpViewport.TopLeft.y, _wpViewport.BottomLeft.y + 192.0f),
                    _wpViewport.TopLeft.z
                );

                obj.transform.position = newPos;
            }
        }
    }
}