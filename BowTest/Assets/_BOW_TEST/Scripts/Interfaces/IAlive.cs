using System;

public interface IAlive
{
    event Action OnDeadEvent;
    event Action OnHealthChangedEvent;

    float MaxHealth { get; }
    float Health { get; set; }
    bool IsDead { get; }
}
