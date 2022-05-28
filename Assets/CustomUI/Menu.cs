using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CustomUI
{
    public class Menu : CustomElement
    {
        public UnityEvent OnHidden;
        public UnityEvent OnUnhidden;
        // List of ControlOverrides
        [SerializeField] private bool _controlParentHiddenState;

        protected List<CustomElement> childElements = new List<CustomElement>();
        private Dictionary<CustomElement, bool> elementHiddenStates = new Dictionary<CustomElement, bool>();
        private Dictionary<CustomElement, bool> elementInteractableStates = new Dictionary<CustomElement, bool>();

        public void AddElement(CustomElement element)
        {
            childElements.Add(element);
            element.parentMenu = this;
        }

        public bool TryRemoveElement(CustomElement element)
        {
            if (!childElements.Contains(element)) return false;
            
            childElements.Remove(element);
            element.parentMenu = null;

            return true;
        }
        
        public override void SetHiddenState(bool hidden)
        {
            base.SetHiddenState(hidden);

            if (_controlParentHiddenState && parentMenu != null)
            {
                parentMenu.SetHiddenState(!hidden);
            }
            
            foreach (CustomElement element in childElements)
            {
                if (hidden)
                {
                    if (element.TryGetComponent(out Menu childMenu))
                    {
                        if (childMenu._controlParentHiddenState) continue;
                    }
                    
                    if (!elementHiddenStates.ContainsKey(element))
                        elementHiddenStates.Add(element, element.Hidden);
                    element.SetHiddenState(true);
                }
                else
                {
                    if (elementHiddenStates.ContainsKey(element))
                    {
                        element.SetHiddenState(elementHiddenStates[element]);
                    }
                }
            }

            if (hidden)
            {
                OnHidden.Invoke();
            }
            else
            {
                OnUnhidden.Invoke();
                elementHiddenStates.Clear();
            }
        }

        public override void SetInteractableState(bool interactable)
        {
            base.SetInteractableState(interactable);

            foreach (CustomElement element in childElements)
            {
                if (!interactable)
                {
                    if (!elementInteractableStates.ContainsKey(element))
                        elementInteractableStates.Add(element, element.Interactable);
                    element.SetInteractableState(false);
                }
                else
                {
                    bool state = !elementInteractableStates.ContainsKey(element) || elementInteractableStates[element];

                    element.SetInteractableState(state);
                }
            }
            
            if (interactable)
                elementInteractableStates.Clear();
        }
    }
}