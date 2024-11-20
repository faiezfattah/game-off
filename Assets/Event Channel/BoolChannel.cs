using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Bool Channel", menuName = "Event Channel/Bool Channel")]
public class BoolChannel : ScriptableObject
{
    public Action<bool> OnEventRaised;

    public void RaiseEvent(bool value) {
        OnEventRaised?.Invoke(value);
    }
}
