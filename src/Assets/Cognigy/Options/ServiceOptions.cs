using System;
using UnityEngine;

public enum ServiceType
{
    Cognigy_AI,
    Speech_To_Text,
    Text_To_Speech
}

[Serializable]
public class ServiceOptions : ScriptableObject
{
    [SerializeField]
    public virtual ServiceType ServiceType
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    [SerializeField]
    public virtual string ServiceName
    {
        get
        {
            throw new NotImplementedException();
        }
    }

    [SerializeField]
    public virtual Type ServiceImplementation
    {
        get
        {
            throw new NotImplementedException();
        }
    }
}
