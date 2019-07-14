using System;
using UnityEngine;

/// <summary>
/// PlayerPrefsのラッパー
/// </summary>
public static class Preferences
{
    public class Preference
    {
        public Type   Type    { get; set; }
        public object Content { get; set; }
    }

    /// <summary>
    /// 設定を読み込む。ない場合は作成する
    /// </summary>
    /// <param name="key">設定の識別子</param>
    /// <param name="type">型</param>
    public static Preference LoadPreference(string key, Type type)
    {
        var  pref  = new Preference();
        bool isNew = false;

        pref.Type  = type;

        if(type == typeof(int))
        {
            int saved = PlayerPrefs.GetInt(key, int.MinValue);

            if(saved == int.MinValue)
            {
                isNew = true;
                PlayerPrefs.SetInt(key, 0);
                pref.Content = 0;
            } else {
                pref.Content = saved;
            }
        } else if(type == typeof(float)) {
            float saved = PlayerPrefs.GetFloat(key, float.MinValue);

            if(saved - float.MinValue < float.Epsilon)
            {
                isNew = true;
                PlayerPrefs.SetFloat(key, 0.0f);
                pref.Content = 0.0f;
            } else {
                pref.Content = saved;
            }
        } else {
            string saved = PlayerPrefs.GetString(key, null);

            if(saved == null)
            {
                isNew = true;
                PlayerPrefs.SetString(key, "");
                pref.Content = "";
            } else {
                pref.Content = saved;
            }
        }

        if(isNew)
        {
            Debug.Log(string.Format("New key {0} has been created and its value is set to {1}.", key, pref));
        }

        return pref;
    }
}