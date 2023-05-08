using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDefinition : MonoBehaviour
{
    public PersistentType PersistentType;
    public string ID;


    private void OnValidate()
    {
        if(PersistentType == PersistentType.ReadWrite)
        {
            //���IDΪ��ֱ�����ɶ���ID
            if (ID == string.Empty)
            {
                ID = System.Guid.NewGuid().ToString();
            }
        }
        else
        {
            ID = string.Empty;
        }             
    }
}
