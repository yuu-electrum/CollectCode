using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Game.Playable
{
    /// <summary>
    /// 残機を管理するクラス
    /// </summary>
    public class LifeManager : MonoBehaviour
    {

        public int Life { get { return _life; } }

        private int _life;

        public void Initialize(int maxLife)
        {
            _life = maxLife;
        }

        /// <summary>
        /// 残機を減らす
        /// </summary>
        public void Kill()
        {
            if(_life > 0) { _life--; }
        }

        /// <summary>
        /// ゲームオーバーになるべきか
        /// </summary>
        public bool IsGameOver { get { return Life <= 0; } }
    }
}