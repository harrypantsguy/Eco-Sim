using System.Collections.Generic;
using _Project.Codebase;
using UnityEngine;

namespace CustomUI
{
    public abstract class CustomElement : MonoBehaviour
    {
        public static List<CustomElement> elements = new List<CustomElement>();
        
        [SerializeField] private bool _dontFindParentMenuOnAwake;
        public bool IsWorldSpace;
        [SerializeField] protected GameObject _graphicsObject;
        [SerializeField] private bool _startHidden;
        [HideInInspector] public bool cursorInRect;
        public bool Hidden { get; private set; }
        public bool Interactable { get; private set; }
        public bool IsToggleTrigger;
        public bool FlagMouseInsideUI;

        public float GraphicsAlpha
        {
            get => _graphicsGroupComponent.alpha;
            set => _graphicsGroupComponent.alpha = value;
        }
        
        public Vector2 GraphicsScale
        {
            get => _graphicsObject.transform.localScale;
            set => _graphicsObject.transform.localScale = value;
        }
        [HideInInspector] public Menu parentMenu;
        protected bool CursorInside { get; private set; }
        protected bool CursorEnterThisFrame { get; private set; }
        protected bool CursorExitThisFrame { get; private set; }
        protected bool triggeredThisFrame;
        protected bool triggered;
        protected bool untriggeredThisFrame;
        
        protected UIManager _UIManager;
        
        private CanvasGroup _graphicsGroupComponent;
        public RectTransform RectTransform { get; private set; }
        public Rect Rect => RectTransform.rect;

        protected virtual void Awake()
        {
            elements.Add(this);
            
            if (!_dontFindParentMenuOnAwake)
            {
                Transform parent = transform.parent;
                while (parent != null)
                {
                    if (parent.TryGetComponent(out Menu menu))
                    {
                        menu.AddElement(this);
                        break;
                    }

                    parent = parent.parent;
                }
            }
            if (_graphicsObject != null)
                _graphicsGroupComponent = _graphicsObject.GetComponent<CanvasGroup>();
            RectTransform = GetComponent<RectTransform>();
        }
        
        protected virtual void Start()
        {
            Managers.TryGetManager(out _UIManager);

            SetInteractableState(true);
            SetHiddenState(_startHidden);
        }

        protected virtual void Update()
        {
            if (Hidden || !Interactable) return;
            
            if (cursorInRect && !CursorInside)
                OnCursorEnter();
            else if (cursorInRect && CursorInside)
                OnCursorStay();
            else if (!cursorInRect && CursorInside)
                OnCursorExit();
            else
            {
                CursorExitThisFrame = false;
            }

            bool toggleKeyDown = _UIManager.InteractIsPressed;
            bool toggleKeyHeld = _UIManager.InteractIsHeld;
            bool toggleKeyReleased = _UIManager.InteractIsReleased;
            
            if (CursorInside && !triggered && toggleKeyDown && !_UIManager.CancelIsHeld)
            {
                OnTrigger();
            }
            else if (!IsToggleTrigger && triggered && _UIManager.CancelIsPressed)
            {
                CancelTrigger();
            }
            else if (IsToggleTrigger && CursorInside && triggered && toggleKeyDown 
                     || !IsToggleTrigger && triggered && toggleKeyReleased)
            {
                OnUntrigger();
            }
            else if (IsToggleTrigger && triggered)
            {
                WhileTriggered();
            }
            else
            {
                untriggeredThisFrame = false;
            }
        }

        protected virtual void LateUpdate()
        {
        }
        
        protected virtual void FixedUpdate()
        {
        }

        protected virtual void OnEnable()
        {
            
        }

        protected virtual void OnDisable()
        {
            CancelTrigger();
            CursorExitThisFrame = false;
            CursorEnterThisFrame = false;
            triggeredThisFrame = false;
            untriggeredThisFrame = false;
        }

        protected virtual void OnDestroy()
        {
            elements.Remove(this);
        }

        protected virtual void OnCursorEnter()
        {
            CursorInside = true;
            CursorEnterThisFrame = true;
        }

        protected virtual void OnCursorStay()
        {
            CursorEnterThisFrame = false;
        }
        
        protected virtual void OnCursorExit()
        {
            CursorInside = false;
            CursorExitThisFrame = true;
        }

        protected virtual void OnTrigger()
        {
            triggered = true;
            triggeredThisFrame = true;
        }
        
        protected virtual void WhileTriggered()
        {
            triggeredThisFrame = false;
        }

        public virtual void CancelTrigger()
        {
            triggered = false;
            triggeredThisFrame = false;
        }
        
        protected virtual void OnUntrigger()
        {
            triggered = false;
            untriggeredThisFrame = true;
        }

        public virtual void FlipHiddenState()
        {
            SetHiddenState(!Hidden);
        }

        public virtual void SetHiddenState(bool hidden)
        {
            Hidden = hidden;
            if (_graphicsObject != null)
                _graphicsObject.SetActive(!hidden);
        }
        
        public virtual void SetInteractableState(bool interactable)
        {
            Interactable = interactable;
        }
    }
}