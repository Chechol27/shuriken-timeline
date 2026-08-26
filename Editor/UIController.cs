using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class UIController<TParams> : ScriptableObject where TParams : struct , ITuple
{
    public virtual void Init(TParams args){}
}

public abstract class UIController : ScriptableObject
{
    public virtual void Init(){}
}
