using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TargetEventSystem : MonoBehaviour
{
    public static TargetEventSystem current;

    private void Awake()
    {
        current = this;
    }

    public event Action<GameObject> onConfirmTargetSelect;

    public void ConfirmTargetSelect(GameObject obj)
    {
        onConfirmTargetSelect?.Invoke(obj);

        // if (onConfirmTargetSelect != null)
        // {
        //     onConfirmTargetSelect(obj);
        // }
    }
}
