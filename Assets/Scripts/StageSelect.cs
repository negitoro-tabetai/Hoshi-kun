using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [System.Serializable]
    struct Stage
    {
        public string name;
        public Image image;
    }

    [SerializeField] Stage[] _stages;
    [SerializeField] Color _normalColor, _selectedColor;

    int _stageNumber;

    public int StageNumber
    {
        get
        {
            return _stageNumber;
        }
        set
        {
            _stageNumber = value;
            _stageNumber = (int)Mathf.Repeat(_stageNumber, _stages.Length);
        }
    }

    float _previousInput;
    float _input;

    public bool MoveRight
    {
        get
        {
            return _previousInput == 0 && _previousInput < _input;
        }
    }
    public bool MoveLeft
    {
        get
        {
            return _previousInput == 0 && _previousInput > _input;
        }
    }

    void Start()
    {
        StageNumber = 0;
        UpdateImage();
    }

    void Update()
    {

        // 左右入力
        _input = Input.GetAxisRaw("Horizontal");
        if (MoveLeft)
        {
            StageNumber--;
            UpdateImage();
        }
        if (MoveRight)
        {
            StageNumber++;
            UpdateImage();
        }

        if (Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene(_stages[StageNumber].name);
        }

        //入力判定用
        _previousInput = _input;
    }

    /// <summary>
    /// イメージのカラーを更新する
    /// </summary>
    void UpdateImage()
    {
        for (int i = 0; i < _stages.Length; i++)
        {
            if (i == StageNumber)
            {
                _stages[i].image.color = _selectedColor;
            }
            else
            {
                _stages[i].image.color = _normalColor;
            }
        }
    }
}
