using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionEnemy : MonoBehaviour
{
    public GameObject action;
    public TextMeshProUGUI textAction;
    public Image icon;

    public List<Sprite> listIcon;

    public void UnShow()
    {
        gameObject.SetActive(false);
    }
    public void AttackShow(int damage)
    {
        action.SetActive(true);
        textAction.text = damage.ToString();
        icon.sprite = listIcon[0];
    }

    public void ShieldShow()
    {
        action.SetActive(true);
        icon.sprite = listIcon[1];
        textAction.text = "";
    }
}
