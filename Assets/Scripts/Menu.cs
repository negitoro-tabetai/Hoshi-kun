﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [System.Serializable]
    struct Element
    {
        public Image image;
        public UnityEvent _decide, _select;
    }

    enum InputAxis
    {
        Horizontal,
        Vertical
    }
    [SerializeField] InputAxis _inputAxis;

    [SerializeField] Element[] _elements;
    [SerializeField] Color _normalColor;
    [SerializeField] Color _selectedColor;

    int _cursor;

    public int Cursor
    {
        get
        {
            return _cursor;
        }
        set
        {
            _cursor = value;
            _cursor = (int)Mathf.Repeat(_cursor, _elements.Length);
        }
    }

    Vector2 _previousInput;
    Vector2 _input;

    public bool Right
    {
        get
        {
            return _previousInput.x == 0 && _previousInput.x < _input.x;
        }
    }
    public bool Left
    {
        get
        {
            return _previousInput.x == 0 && _previousInput.x > _input.x;
        }
    }
    public bool Up
    {
        get
        {
            return _previousInput.y == 0 && _previousInput.y < _input.y;
        }
    }
    public bool Down
    {
        get
        {
            return _previousInput.y == 0 && _previousInput.y > _input.y;
        }
    }

    void Start()
    {
        Cursor = 0;
        UpdateImage();
    }

    void Update()
    {
        _input.x = Input.GetAxisRaw(_inputAxis.ToString());
        _input.y = Input.GetAxisRaw(_inputAxis.ToString());
        if (_inputAxis == InputAxis.Horizontal)
        {
            if (Left)
            {
                Cursor--;
                UpdateImage();
            }
            if (Right)
            {
                Cursor++;
                UpdateImage();
            }
        }
        else
        {
            if (Up)
            {
                Cursor--;
                UpdateImage();
            }
            if (Down)
            {
                Cursor++;
                UpdateImage();
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            _elements[Cursor]._decide.Invoke();
        }

        _elements[Cursor]._select.Invoke();


        //入力判定用
        _previousInput = _input;
    }

    /// <summary>
    /// イメージのカラーを更新する
    /// </summary>
    void UpdateImage()
    {
        for (int i = 0; i < _elements.Length; i++)
        {
            if (i == Cursor)
            {
                _elements[i].image.color = _selectedColor;
            }
            else
            {
                _elements[i].image.color = _normalColor;
            }
        }
    }
}