using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarManager : MonoBehaviour
{
    Image _hpBarImage;
    [SerializeField] GameObject _invincibleText;
    [SerializeField] Player _player;

    const float Percent = 0.01f;
    void Start()
    {
        _hpBarImage = GetComponent<Image>();
    }

    void Update()
    {
        _hpBarImage.fillAmount = _player.Hp * Percent;
        if (_player.IsInvincible)
        {
            _invincibleText.SetActive(true);
        }
        else
        {
            _invincibleText.SetActive(false);
        }
    }
}
