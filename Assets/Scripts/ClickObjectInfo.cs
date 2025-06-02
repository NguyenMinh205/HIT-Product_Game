using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObjectInfo : MonoBehaviour
{
    [SerializeField] private GameObject Info;

    private bool isShowInfo;
    private void Awake()
    {
        isShowInfo = false;
    }
    private void Update()
    {
        CheckClick();
    }
    public void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickItem(GetObject());
        }
        TurnOffShowInfo(GetObject());
    }
    public GameObject GetObject()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.down);

        if (hit.collider != null && hit.collider.CompareTag("Item"))
        {
            return hit.collider.gameObject;
        }

        return null;
    }
    public Vector3 GetMousePos()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;

        return pos;
    }
    public void ClickItem(GameObject gameObject)
    {
        if (gameObject != null)
        {
            Info.SetActive(true);
            Info.transform.position = GetMousePos() + new Vector3(1.2f, 0.5f, 0);
            this.isShowInfo = true;
        }
    }

    public void TurnOffShowInfo(GameObject gameObject)
    {
        if(gameObject == null)
        {
            Info.SetActive(false);
            this.isShowInfo = false;
        }
    }
}
