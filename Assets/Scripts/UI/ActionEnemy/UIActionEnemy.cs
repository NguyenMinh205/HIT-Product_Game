using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIActionEnemy : MonoBehaviour
{
    [SerializeField] private List<ActionEnemy> listAciton; 

    public void SetAction(Sprite icon, int posX, int damage, Enemy enemy)
    {
        ActionEnemy actionEnemy = null;

        for (int i = 0; i < listAciton.Count; i++)
        {
            if (!listAciton[i].gameObject.activeSelf)
            {
                actionEnemy = listAciton[i];
                actionEnemy.gameObject.SetActive(true);
                break;
            }
        }

        if (actionEnemy == null)
        {
            Debug.LogWarning("No available ActionEnemy in list to reuse.");
            return;
        }

        RectTransform rectTransform = actionEnemy.GetComponent<RectTransform>();
        rectTransform.localPosition = new Vector3(posX, 0f, rectTransform.localPosition.z);
        actionEnemy.SetUIAction(icon, damage);
    }

    public void UnActionIndexEnemy(int index)
    {
        if (index >= 0 && index < listAciton.Count)
            listAciton[index].UnShow();
    }

    public void ClearAllActionList()
    {
        foreach (var action in listAciton)
        {
            action.UnShow(); 
        }
    }

    public void UnShowActionEnemy()
    {
        gameObject.SetActive(false);
    }

    public void Execute(int index)
    {
        if (index >= 0 && index < listAciton.Count)
            listAciton[index].UnShow();
    }
}
