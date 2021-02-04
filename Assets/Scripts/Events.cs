using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public static class Events
{
    public static EventAggregator<SkillManager> OnSkillActivate = new EventAggregator<SkillManager>();
    public static EventAggregator<BaseItem> OnItemCollect = new EventAggregator<BaseItem>();
    public static EventAggregator<int> OnItemDrop = new EventAggregator<int>();
    public static EventAggregator<IDamageable> OnDamageableHPChange = new EventAggregator<IDamageable>();
    public static EventAggregator<SkillCaster> OnSkillCasterMPChange = new EventAggregator<SkillCaster>();
    public static EventAggregator<IReadOnlyList<ISelectable>> OnSelectedChange = new EventAggregator<IReadOnlyList<ISelectable>>();
    public static EventAggregator<IDamageable> OnUnitDestroy = new EventAggregator<IDamageable>();
    public static EventAggregator<IReadOnlyList<string>> OnChangeAlliance = new EventAggregator<IReadOnlyList<string>>();
}

public class EventAggregator<T>
{
    private readonly List<Action<T>> _callbacks = new List<Action<T>>();

    public void Subscribe(Action<T> callback)
    {
        _callbacks.Add(callback);
    }

    public void Publish(T unit)
    {
        foreach (Action<T> callback in _callbacks)
        {
            callback(unit);
        }
    }

    public void UnSubscribe(Action<T> callback)
    {
        _callbacks.Remove(callback);
    }
}
