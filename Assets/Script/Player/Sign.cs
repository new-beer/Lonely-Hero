using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XInput;

public class Sign : MonoBehaviour
{
    private PlayerInputController playerInput;
    private Animator animtor;
    public GameObject signSprite;
    public Transform plaryTransform;
    public bool canPress;

    private void Awake()
    {
        //����������ϵ����
        //animtor = GetComponentInChildren<Animator>();
        animtor = signSprite.GetComponent<Animator>();
        playerInput = new PlayerInputController();
        playerInput.Enable();
    }
    private void OnEnable()
    {
        InputSystem.onActionChange += OnActionChange;
    }

    

    private void Update()
    {
        //����ͼ��
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        //�̶���ʶ����
        signSprite.transform.localScale = plaryTransform.localScale;
    }
    //ʶ��ͬ�豸������
    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if(actionChange == InputActionChange.ActionStarted)
        {
            Debug.Log(((InputAction)obj).activeControl.device);
            //�л���ͬ�豸���
            var d = ((InputAction)obj).activeControl.device;
            switch (d.device)
            {
                case Keyboard:
                    animtor.Play("keyboard");
                    break;
                case XInputControllerWindows:
                    animtor.Play("ZS");
                    break;
            }
        }
    }

    //�������Ի������
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            canPress = true;
        }
    }
    //���봥��
    private void OnTriggerExit2D(Collider2D other)
    {
        canPress = false;
    }
}
