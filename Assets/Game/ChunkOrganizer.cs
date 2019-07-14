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

        public void Organize(List<GameObject> organizedObjects)
        {
            if(organizedObjects == null) { return; }

            foreach(var obj in organizedObjects)
            {
                var newPos = new Vector3(
                    UnityEngine.Random.Range(0.10f, 0.90f),
                    UnityEngine.Random.Range(0.30f, 0.95f),
                    _wpViewport.Camera.farClipPlane
                );
                /*
                var width  = obj.transform.localScale.x * 8.0f;
                var height = obj.transform.localScale.y * 8.0f;
                */

                /*
                var newPos = new Vector3(
                    UnityEngine.Random.Range(_wpViewport.TopLeft.x + width , _wpViewport.TopRight.x - width),
                    UnityEngine.Random.Range(_wpViewport.TopLeft.y - height, _wpViewport.BottomLeft.y + 192.0f),
                    _wpViewport.TopLeft.z
                );
                */

                obj.transform.position = _wpViewport.Camera.ViewportToWorldPoint(newPos);
            }
        }
    }
}