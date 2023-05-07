using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour,IInteractable
{
    [Header("�㲥")]
    public VoidEVentSO LoadGameEvent;

    [Header("��������")]
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
            //TODO ʵ�ֱ���
            LoadGameEvent.RaiseEvent();

            this.gameObject.tag = "Untagged";
        }
    }

}
