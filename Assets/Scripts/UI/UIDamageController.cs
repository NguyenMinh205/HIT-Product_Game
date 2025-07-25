using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum UIDamage
{
       ShowDamageText,
}
public class UIDamageController : Singleton<UIDamageController>
{
    [SerializeField] private TextMeshProUGUI damageTextPrefab;
    [SerializeField] private Transform textParentDamage;

    [SerializeField] private float timeDelayDeSpawn;


    public void ShowDamageText(int damage, object obj)
    {
        Debug.Log("Show Damage Text: " + damage);

        Vector3 pos = new Vector3(0f,0f,0f);
        if(obj is Player player)
        {
            Debug.Log("Show Damage Text for Player: " + player.name);
            pos = player.transform.position;
        }
        else if (obj is Enemy enemy)
        {
            Debug.Log("Enemy Show Damage Text");
            pos = enemy.transform.position;
        }

        TextMeshProUGUI textDamage = PoolingManager.Spawn(damageTextPrefab, pos, Quaternion.identity, textParentDamage);

        textDamage.text = damage.ToString(); // Gán giá trị sát thương

        var seq = DOTween.Sequence();
        textDamage.transform.localScale = Vector3.one * 0.3f; // Đặt scale ban đầu

        seq.Append(textDamage.transform.DOScale(Vector3.one * 1.2f, 0.7f)) // To ra
           .Append(textDamage.transform.DOMoveY(textDamage.transform.position.y - 4f, 1f).SetEase(Ease.OutQuad)) 
           .AppendInterval(0.2f)
           .OnComplete(() =>
           {
               PoolingManager.Despawn(textDamage.gameObject);
           });
    }
}

