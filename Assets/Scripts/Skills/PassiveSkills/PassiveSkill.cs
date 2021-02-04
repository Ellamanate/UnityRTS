using UnityEditor;
using UnityEngine;

public class PassiveSkill : ProtoSkill
{
    public PassiveSkill() 
    { 
        SkillTargetSelector = new SelectNobody(this);
        CanActivate = false;
    }
}