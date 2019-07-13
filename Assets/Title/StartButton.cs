using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Title
{
    public class StartButton : MonoBehaviour, IFade
    {

        private GameObject _me;
        private Transform _transform;

        public void Start()
        {
            _me = this.gameObject;
            _transform = _me.transform;
        }

        public IEnumerator StartFadein()
        {
            var currentPos = _transform.localPosition;

            for(;;)
            {
                yield return new WaitForSeconds(0.01f);

                currentPos.x += 0.05f;
                _transform.localPosition = currentPos;
            }
        }

        public IEnumerator StartFadeout()
        {
            yield return null;
        }

        public bool IsComplete { get { return true;} }
    }
}