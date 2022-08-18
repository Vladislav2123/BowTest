using System.Collections.Generic;
using System;

public class TargetsHolder
{
    public event Action OnTargetsCountChangedEvent;

    public List<Target> Targets { get; private set; }
    public bool IsEmpty => Targets.Count == 0;

    public TargetsHolder()
    {
        Targets = new List<Target>();
    }

    public void TryAddTarget(Target target)
    {
        if (Targets.Contains(target)) return;

        Targets.Add(target);
        OnTargetsCountChangedEvent?.Invoke();
    }

    public void TryRemoveTarget(Target target)
    {
        if (Targets.Contains(target) == false) return;

        Targets.Remove(target);
        OnTargetsCountChangedEvent?.Invoke();
    }
}
