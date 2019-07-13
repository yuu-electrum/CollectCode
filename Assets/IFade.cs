using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public interface IFade
    {
        IEnumerator StartFadein();
        IEnumerator StartFadeout();
        bool IsComplete { get; }
    }
}