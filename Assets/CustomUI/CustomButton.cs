using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CustomUI
{
    public class CustomButton : CustomElement
    {
        public UnityEvent toggleEvent;
        public UnityEvent untoggleEvent;
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _hoverColor;
        [SerializeField] private Color _triggeredColor;
        [SerializeField] protected RawImage _backgroundImage;

        protected override void Start()
        {
            base.Start();
            
            UpdateGraphics();
        }

        private void UpdateGraphics()
        {
            if (triggered)
            {
                _backgroundImage.color = _triggeredColor;
            }
            else
            {
                if (CursorInside)
                    _backgroundImage.color = _hoverColor;
                else
                    _backgroundImage.color = _normalColor;
            }
        }
        
        protected override void OnCursorEnter()
        {
            base.OnCursorEnter();

            if (!triggered)
                _backgroundImage.color = _hoverColor;
        }

        protected override void OnCursorExit()
        {
            base.OnCursorExit();
            
            if (!triggered)
                _backgroundImage.color = _normalColor;
        }
        
        public void SetTriggerState(bool state)
        {
            triggered = state;

            UpdateGraphics();
        }

        protected override void OnTrigger()
        {
            base.OnTrigger();

            _backgroundImage.color = _triggeredColor;
            
            toggleEvent.Invoke();
        }

        public override void CancelTrigger()
        {
            base.CancelTrigger();
            
            if (CursorInside)
                _backgroundImage.color = _hoverColor;
            else
                _backgroundImage.color = _normalColor;
        }

        protected override void OnUntrigger()
        {
            base.OnUntrigger();

            if (CursorInside)
                _backgroundImage.color = _hoverColor;
            else
                _backgroundImage.color = _normalColor;
            
            untoggleEvent.Invoke(); // order matters here
        }
    }
}