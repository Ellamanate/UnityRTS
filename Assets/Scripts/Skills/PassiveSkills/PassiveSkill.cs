using UnityEditor;
using UnityEngine;

public abstract class PassiveSkill : ProtoSkill
{
    public PassiveSkill() : base(new SelectNobody(), false) { }
}