using UnityEditor;
using UnityEngine;

namespace Skills
{
    public abstract class PassiveSkill : ProtoSkill
    {
        public PassiveSkill() : base(new SelectNobody(), false) { }
    }
}