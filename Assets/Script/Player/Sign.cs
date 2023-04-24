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
        //获得子物体上的组件
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
        //激活图标
        signSprite.GetComponent<SpriteRenderer>().enabled = canPress;
        //固定标识方向
        signSprite.transform.localScale = plaryTransform.localScale;
    }
    //识别不同设备的输入
    private void OnActionChange(object obj, InputActionChange actionChange)
    {
        if(actionChange == InputActionChange.ActionStarted)
        {
            Debug.Log(((InputAction)obj).activeControl.device);
            //切换不同设备输出
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

    //触发可以互动组件
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            canPress = true;
        }
    }
    //脱离触发
    private void OnTriggerExit2D(Collider2D other)
    {
        canPress = false;
    }
}
