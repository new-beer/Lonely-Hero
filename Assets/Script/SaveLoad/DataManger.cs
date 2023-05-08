using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataManger : MonoBehaviour
{
    public static DataManger instance;
    [Header("��������")]
    public VoidEVentSO saveDataEvent;
    private List<ISaveable> saveableList = new List<ISaveable>();
    private Data saveData;

    private void Awake()
    {
        //����Ϸ�տ�ʼʱ���Դ洢��ֵ
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        saveData = new Data();
    }
    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += save;
    }
    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= save;
    }
    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            load();
        }
    }


    public void RegisterSaveData(ISaveable saveable)
    {
        if (!saveableList.Contains(saveable))
        {
            saveableList.Add(saveable);
        }
    }

    public void UnRegisterSaveData(ISaveable saveable)
    {
        saveableList.Remove(saveable);
    }

    public void save()
    {
        foreach(var saveable in saveableList) { 
            saveable.GetSaveData(saveData);
        }
    }
    public void load()
    {
        foreach (var saveable in saveableList)
        {
            saveable.LoadData(saveData);
        }
    }
}
