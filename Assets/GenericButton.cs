using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    /// <summary>
    /// 一般的なボタンを表すクラス
    /// </summary>
    public class GenericButton : MonoBehaviour
    {
        [SerializeField]
        private Button _button;

        [SerializeField]
        private Text _label;

        public Button Button { get { return _button; } }
        public Text   Label  { get { return _label;  } }
    }
}