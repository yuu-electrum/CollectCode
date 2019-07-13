using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public void Start()
        {
            // .metaファイルを除いたソースコードを列挙する
            var directories = new List<string>(
                System.IO.Directory.GetFiles(Application.streamingAssetsPath + "/SourcecodeFiles/")
            );

            var targets = directories.Where(str => str.IndexOf(".meta", StringComparison.Ordinal) < 0).ToList();

            // 列挙されたコードの中から一つを選ぶ
            var index   = UnityEngine.Random.Range(0, targets.Count);
        }
    }
}