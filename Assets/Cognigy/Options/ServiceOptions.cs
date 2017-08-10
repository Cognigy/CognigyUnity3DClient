using System;
using UnityEngine;

[Serializable]
public class ServiceOptions : ScriptableObject
{
    [SerializeField]
    public string ServiceName;

    [SerializeField]
    protected Type ServiceType;

    public virtual Type GetServiceType()
    {
        return this.ServiceType;
    }
}
