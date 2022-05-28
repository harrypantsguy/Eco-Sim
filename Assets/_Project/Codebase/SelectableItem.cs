using System;
using CustomUI;
using TMPro;
using UnityEngine;

namespace _Project.Codebase
{
    [Serializable]
    public class SelectableItem : CustomButton
    {
        public event EventHandler onSelect;

        protected virtual void Select()
        {
            //onSelect.Invoke(this, EventArgs.Empty);
        }

        protected override void OnTrigger()
        {
            base.OnTrigger();

            Select();
        }

        protected override void OnUntrigger()
        {
            base.OnUntrigger();
            
            Select();
        }
    }
}