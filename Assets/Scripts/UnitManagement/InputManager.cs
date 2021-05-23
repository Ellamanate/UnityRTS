using UnityEngine;
using Skills;


namespace UnitManagement
{
    [RequireComponent(typeof(UnitSelector))]
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private string[] _skillHotKeys;
        private SkillManager _currentSkillCaster;
        private DefaultOrder _currentOrder;
        [SerializeField] private bool _selectingSkillTarget = false;
        [SerializeField] private bool _ordering = false;

        public void SkillTargetSelecting(SkillManager skillCaster)
        {
            _selectingSkillTarget = true;
            _currentSkillCaster = skillCaster;
        }

        private void Awake()
        {
            _currentOrder = new DefaultOrder();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Select();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                Command();
            }
            else if (UnitSelector.Instance.CanControlSelected)
            {
                KeyDown();
            }
        }

        private void Select()
        {
            if (_selectingSkillTarget && _currentSkillCaster != null)
            {
                _selectingSkillTarget = false;
                _currentSkillCaster.SelectTarget();
            }
            else if (_ordering && UnitSelector.Instance.CanControlSelected)
            {
                _ordering = false;
                _currentOrder.Command();
                DropOrder();
            }
            else
            {
                UnitSelector.Instance.Select();
            }
        }

        private void Command()
        {
            if (DropState(ref _selectingSkillTarget) || DropState(ref _ordering))
            {
                DropOrder();
                return;
            }
            else if (UnitSelector.Instance.CanControlSelected)
            {
                _currentOrder.Command();
            }
        }

        private void KeyDown()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                _currentOrder = new Attack();
                _ordering = true;
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                _currentOrder = new Collect();
                _ordering = true;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                _currentOrder = new Stop();
                _ordering = false; 
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                _currentOrder = new HoldPosition();
                _ordering = false;
            }

            /*if (!_selectingSkillTarget)
                _unitOrdering.IsPressSkillHotkey(UnitSelector.Instance.SelectedUnits);*/
        }

        private void DropOrder() => _currentOrder = new DefaultOrder();

        private bool DropState(ref bool state)
        {
            if (state)
            {
                state = false;
                return true;
            }

            return false;
        }
    }
}