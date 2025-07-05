using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHealthBarController : Singleton<UIHealthBarController>
{
    [SerializeField] private List<HealthBar> _uiHealthBarEnemy;
    [SerializeField] private HealthBar _healthBarPlayer;

    public void InitHealthBarToObjectBase(Object obj)
    {
        if(obj is Enemy enemy)
        {
            foreach(HealthBar health in _uiHealthBarEnemy)
            {
                if(!health.gameObject.activeSelf)
                {
                    health.InitHealthBar(enemy);
                    enemy.Health = health;
                    return;
                }
            }
        }
        else if (obj is Player player)
        {
            Debug.Log("Player Init Health Bar");
            _healthBarPlayer.InitHealthBar(player);
            player.Health = _healthBarPlayer;
            return;
        }
    }

}
