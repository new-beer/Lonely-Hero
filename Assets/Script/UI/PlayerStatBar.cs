using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    public Character currentCharacter;
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;
    private bool isRecovering;


    public void Update()
    {
        //血量减少UI
        if(healthDelayImage.fillAmount > healthDelayImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime;
        }
        //能量恢复UI
        if(isRecovering)
        {
            float persentage = currentCharacter.currentPower / currentCharacter.maxPower;
            powerImage.fillAmount = persentage;

            if (persentage >= 1)
            {
                isRecovering = false;
                return;
            }
        }
    }

    /// <summary>
    /// 接受Health的变更百分比
    /// </summary>
    /// <param name="persentage">百分比：Current/Max</param>
    public void OnHealthChange(float persentage)
    {
        healthImage.fillAmount = persentage;
    }
    //接收UI传回的数据进行计算
    public void OnPowerChange(Character character)
    {
        isRecovering = true;
        currentCharacter = character;

    }
}
