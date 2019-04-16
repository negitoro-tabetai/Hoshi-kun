using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarManager : MonoBehaviour
{
  
    [SerializeField] GameObject _invincibleText;
    [SerializeField] Player _player;

   int health;
    [SerializeField] int numHealth;

    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite emptyHeart;

    const float Percent = 0.01f;
    void Start()
    {
        health = _player.Hp;
    }

    void Update()
    {

        if (_player.IsInvincible)
        {
            _invincibleText.SetActive(true);
        }
        else
        {
            _invincibleText.SetActive(false);
        }
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < _player.Hp)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;

            }
            if (i < numHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }

        }
    }
}
