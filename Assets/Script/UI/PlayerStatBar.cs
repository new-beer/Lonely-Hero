using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;


    public void Update()
    {
        if(healthDelayImage.fillAmount > healthDelayImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime;
        }
    }

    /// <summary>
    /// ����Health�ı���ٷֱ�
    /// </summary>
    /// <param name="persentage">�ٷֱȣ�Current/Max</param>
    public void OnHealthChange(float persentage)
    {
        healthImage.fillAmount = persentage;
    }
}
