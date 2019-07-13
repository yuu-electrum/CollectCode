using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Title
{
    public class Start : MonoBehaviour, IFade
    {

        private GameObject me;

        public void 
        {
            me = this.gameObject;
        }

        public IEnumerator StartFadein()
        {

        }

        public IEnumerator StartFadeout()
        {
            yield return null;
        }

        public bool IsComplete { get { return true;} }
    }
}