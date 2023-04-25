using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public Sprite closeSprite;
    public bool isDone;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    //���䲥�Ż���
    private void OnEnable()
    {
        spriteRenderer.sprite = isDone? openSprite : closeSprite; 
    }
    public void TirggerAction()
    {
        //Debug.Log("Open Chest");
        if (!isDone)
        {
            OpenChest();
        }
    }
    private void OpenChest()
    {
        spriteRenderer.sprite = openSprite;
        isDone = true;
        //���俪���󣬲���ʾ������ʶ
        this.gameObject.tag = "Untagged";
    }
   
}
