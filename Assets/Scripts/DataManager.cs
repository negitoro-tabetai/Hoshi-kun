using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    // 現在プレイ中のセーブデータ (1~3)
    int _saveData;

    // データ保存用文字列
    string DataKey
    {
        get
        {
            return "SaveData" + _saveData.ToString();
        }
    }
    new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Debug
        for (int i = 0; i < 3; i++)
        {
            int data = PlayerPrefs.GetInt("SaveData" + (i + 1).ToString());
            Debug.Log("SaveData" + (i + 1) + " : " + data);
        }
    }

    void Update()
    {
        // Debug 消したいデータの数字キーを押しながらクリック
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.Alpha1))
        {
            DeleteData(1);
        }
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.Alpha2))
        {
            DeleteData(2);
        }
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.Alpha3))
        {
            DeleteData(3);
        }
    }

    /// <summary>
    /// プレイするセーブデータを変更する。
    /// </summary>
    /// <param name="saveData">変更したいセーブデータ</param>
    public void SelectData(int saveData)
    {
        if (saveData < 1 || 3 < saveData)
        {
            Debug.LogError("セーブデータの値が正しくありません");
        }
        else
        {
            _saveData = saveData;
            Debug.Log(DataKey);
        }
    }

    /// <summary>
    /// クリア状況を更新する。
    /// ステージクリア時に呼び出す。
    /// </summary>
    /// <param name="stage"></param>
    public void Save(int stage)
    {
        if (PlayerPrefs.GetInt(DataKey) < stage)
        {
            Debug.Log(DataKey + " : " + stage);
            PlayerPrefs.SetInt(DataKey, stage);
        }
    }

    /// <summary>
    /// 選択したセーブデータを削除する
    /// </summary>
    /// <param name="saveData">削除するセーブデータ</param>
    public void DeleteData(int saveData)
    {
        string key = "SaveData" + saveData.ToString();
        Debug.Log(key + "を削除しました");
        PlayerPrefs.DeleteKey(key);
    }
}
