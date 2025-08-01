using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionEnemy : MonoBehaviour
{
    [Header("UI Action")]
    public TextMeshProUGUI textActionEnemy;
    public Image icon;

    public void UnShow()
    {
        Destroy(gameObject);
    }
    
    public void SetUIAction(Sprite sprite, int damage)
    {
        icon.gameObject.SetActive(true);
        icon.sprite = sprite;

        if(damage > 0)
        {
            textActionEnemy.gameObject.SetActive(true);
            textActionEnemy.text = damage.ToString();
        }
        else
        {
            textActionEnemy.gameObject.SetActive(false);
        }
    }
}
