using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIActionEnemy : MonoBehaviour
{
    [SerializeField] private ActionEnemy actionEnemyPrefab; 
    [SerializeField] private List<ActionEnemy> listAction; 

    public void SetAction(Sprite icon, int posX, int damage, Enemy enemy)
    {
        ActionEnemy actionEnemy = Instantiate(actionEnemyPrefab, new Vector3(posX, transform.position.y, 0f), Quaternion.identity, transform);
        RectTransform rectTransform = actionEnemy.GetComponent<RectTransform>();

        rectTransform.localPosition = new Vector3(posX, 0f, rectTransform.localPosition.z);
        actionEnemy.SetUIAction(icon, damage);
        listAction.Add(actionEnemy);
    }
    public void UnActionIndexEnemy(int index)
    {
        listAction[index].UnShow();
    }
    public void ClearAllActionList()
    {
        listAction.Clear();
    }
    public void UnShowActionEnemy()
    {
        gameObject.SetActive(false);
    }

    public void Execute(int index)
    {
        listAction[index].UnShow();
    }
}

