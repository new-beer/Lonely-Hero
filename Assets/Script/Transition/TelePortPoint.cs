using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelePortPoint : MonoBehaviour, IInteractable
{
    public SceneLoadEventSO loadEventSO;
    public GameSceneSO sceneToGo;
    public Vector3 positionToGo;
    public void TirggerAction()
    {
        Debug.Log("´«ËÍ£¡");
        loadEventSO.RaiseLoadRequestEvent(sceneToGo,positionToGo,true);
    }
}
