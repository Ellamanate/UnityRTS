using System.Collections.Generic;
using UnityEngine;


namespace UnitManagement
{
    public class DefaultOrder
    {
        public void Command() 
        {
            IReadOnlyList<ISelectable> selectedUnits = UnitSelector.Instance.SelectedUnits;

            if (Raycaster.Instance.OrderMouseRaycast(out RaycastHit hitInfo, out Rigidbody attachedRigidbody))
            {
                if (attachedRigidbody == null)
                {
                    if (selectedUnits.Count == 1)
                    {
                        SelectVector(selectedUnits[0], hitInfo.point);
                    }
                    else
                    {
                        SelectVectorGroup(selectedUnits, hitInfo.point, selectedUnits.Count);
                    }
                }
                else
                {
                    if (TypeChecker<ItemPrefab>.CheckGameObject(attachedRigidbody.gameObject, out ItemPrefab itemPrefab))
                    {
                        SelectItem(selectedUnits[0], itemPrefab);
                    }
                    else if (TypeChecker<IDamageable>.CheckGameObject(attachedRigidbody.gameObject, out IDamageable damageable))
                    {
                        SelectDamageable(selectedUnits, damageable);
                    }
                }
            }
        }

        protected virtual void SelectVector(ISelectable selectable, Vector3 point)
        {
            if (TypeChecker<IManageable>.CheckGameObject(selectable.GameObject, out IManageable manageable))
                manageable.Move(point);
        }

        protected virtual void SelectVectorGroup(IReadOnlyList<ISelectable> selectedUnits, Vector3 point, int selectedCount)
        {
            Vector3[] points = point.CalculateGroupPoints(2, selectedCount);

            for (int i = 0; i < points.Length; i++)
            {
                if (TypeChecker<IManageable>.CheckGameObject(selectedUnits[i].GameObject, out IManageable manageable))
                    manageable.Move(points[i]);
            }
        }

        protected virtual void SelectDamageable(IReadOnlyList<ISelectable> selectedUnits, IDamageable damageable)
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                if (TypeChecker<IManageable>.CheckGameObject(selectedUnits[i].GameObject, out IManageable manageable))
                    manageable.Move(damageable);
            }
        }

        protected virtual void SelectItem(ISelectable selectable, ItemPrefab item)
        {
            if (TypeChecker<IManageable>.CheckGameObject(selectable.GameObject, out IManageable manageable))
                manageable.Collect(item);
        }
    }

    public class Attack : DefaultOrder
    {
        public Attack()
        {

        }

        protected override void SelectVector(ISelectable selectable, Vector3 point) 
        {
            if (TypeChecker<IManageable>.CheckGameObject(selectable.GameObject, out IManageable manageable))
                manageable.Attack(point);
        }

        protected override void SelectVectorGroup(IReadOnlyList<ISelectable> selectedUnits, Vector3 point, int selectedCount)
        {
            Vector3[] points = point.CalculateGroupPoints(2, selectedCount);

            for (int i = 0; i < points.Length; i++)
            {
                if (TypeChecker<IManageable>.CheckGameObject(selectedUnits[i].GameObject, out IManageable manageable))
                    manageable.Attack(points[i]);
            }
        }

        protected override void SelectDamageable(IReadOnlyList<ISelectable> selectedUnits, IDamageable damageable)
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                if (TypeChecker<IManageable>.CheckGameObject(selectedUnits[i].GameObject, out IManageable manageable))
                    manageable.Attack(damageable);
            }
        }

        protected override void SelectItem(ISelectable selectable, ItemPrefab item)
        {
            if (TypeChecker<IManageable>.CheckGameObject(selectable.GameObject, out IManageable manageable))
                manageable.Attack(item);
        }
    }

    public class Collect : DefaultOrder
    {
        public Collect()
        {

        }

        protected override void SelectVector(ISelectable selectable, Vector3 point)
        {
            Debug.Log("Не верная цель");
        }

        protected override void SelectVectorGroup(IReadOnlyList<ISelectable> selectedUnits, Vector3 point, int selectedCount)
        {
            Debug.Log("Не верная цель");
        }

        protected override void SelectDamageable(IReadOnlyList<ISelectable> selectedUnits, IDamageable damageable)
        {
            Debug.Log("Не верная цель");
        }
    }

    public class Stop : DefaultOrder
    {
        public Stop()
        {
            IReadOnlyList<ISelectable> selectedUnits = UnitSelector.Instance.SelectedUnits;

            for (int i = 0; i < selectedUnits.Count; i++)
            {
                if (TypeChecker<IManageable>.CheckGameObject(selectedUnits[i].GameObject, out IManageable manageable))
                    manageable.Stop();
            }
        }
    }

    public class HoldPosition : DefaultOrder
    {
        public HoldPosition()
        {
            IReadOnlyList<ISelectable> selectedUnits = UnitSelector.Instance.SelectedUnits;

            for (int i = 0; i < selectedUnits.Count; i++)
            {
                if (TypeChecker<IManageable>.CheckGameObject(selectedUnits[i].GameObject, out IManageable manageable))
                    manageable.HoldPosition();
            }
        }
    }
}
