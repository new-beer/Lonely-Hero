using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu(menuName ="Event/VoidEventSO")]
public class VoidEVentSO : ScriptableObject
{
    public UnityAction OnEventRaised;
    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}
