using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour,IInteractable
{
    [Header("广播")]
    public VoidEVentSO LoadGameEvent;

    [Header("变量参数")]
    public SpriteRenderer spriteRenderer;
    public GameObject lightObj;
    public Sprite darkSprite;
    public Sprite lightSprite;
    public bool isDone;


    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? lightSprite : darkSprite;
        lightObj.SetActive(isDone);
    }
    public void TirggerAction()
    {
        if (!isDone)
        {
            spriteRenderer.sprite = lightSprite;
            isDone = true;
            lightObj.SetActive (true);
            //TODO 实现保存
            LoadGameEvent.RaiseEvent();

            this.gameObject.tag = "Untagged";
        }
    }

}
