using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName ="Event/SeceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneSO, Vector3, bool> LoadRequestEvent;

    /// <summary>
    /// 场景加载请求
    /// </summary>
    /// <param name="locationToload">要加载的场景</param>
    /// <param name="posToGo">Player的目的坐标</param>
    /// <param name="fadeScreen">是否渐入渐出</param>
    public void RaiseLoadRequestEvent(GameSceneSO locationToload,Vector3 posToGo,bool fadeScreen)
    {
        LoadRequestEvent?.Invoke(locationToload,posToGo,fadeScreen);
    }
}
