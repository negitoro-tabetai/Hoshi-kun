using System.Collections;
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
        public UnityEvent _event;
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

    float _previousInput;
    float _input;

    public bool Increase
    {
        get
        {
            return _previousInput == 0 && _previousInput < _input;
        }
    }
    public bool Decrease
    {
        get
        {
            return _previousInput == 0 && _previousInput > _input;
        }
    }

    void Start()
    {
        Cursor = 0;
        UpdateImage();
    }

    void Update()
    {
        if (_inputAxis == InputAxis.Horizontal)
        {
            _input = Input.GetAxisRaw(_inputAxis.ToString());
        }
        else
        {
            _input = Input.GetAxisRaw(_inputAxis.ToString()) * -1;
        }
        if (Decrease)
        {
            Cursor--;
            UpdateImage();
        }
        if (Increase)
        {
            Cursor++;
            UpdateImage();
        }

        if (Input.GetButtonDown("Jump"))
        {
            _elements[Cursor]._event.Invoke();
        }

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
