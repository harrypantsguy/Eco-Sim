using _Project.Codebase;
using UnityEngine;

namespace CustomUI
{
    public class UIManager : Manager
    {
        [HideInInspector] public bool MouseInsideUI;
        public bool InteractIsPressed { get; private set; }
        public bool InteractIsHeld { get; private set; }
        public bool InteractIsReleased { get; private set; }
        public bool CancelIsPressed { get; private set; }
        public bool CancelIsHeld { get; private set; }
        public bool CancelIsReleased { get; private set; }
        private void Update()
        {
            InteractIsPressed = GameControls.InteractUI.IsPressed;
            InteractIsHeld = GameControls.InteractUI.IsHeld;
            InteractIsReleased = GameControls.InteractUI.IsReleased;
            
            CancelIsPressed = GameControls.CancelInteractUI.IsPressed;
            CancelIsHeld = GameControls.CancelInteractUI.IsHeld;
            CancelIsReleased = GameControls.CancelInteractUI.IsReleased;

            MouseInsideUI = false;
            
            foreach (CustomElement element in CustomElement.elements)
            {
                bool mouseInside = false;
                if (element.IsWorldSpace)
                {
                    /*
                    foreach (CameraController cameraController in CameraController.playerControlledCameras)
                    {
                        Vector2 mousePos = new Vector2(Screen.width/2f, Screen.height/2f);

                        bool cursorInRect = RectTransformUtility.RectangleContainsScreenPoint(element.RectTransform,
                                                mousePos, cameraController.Camera) && 
                                            Vector3.Dot(cameraController.Camera.transform.forward, 
                                                transform.forward) > 0;
                        if (cursorInRect)
                            mouseInside = true;
                    }
                    */
                }
                else
                {
                    element.cursorInRect = 
                        RectTransformUtility.RectangleContainsScreenPoint(element.RectTransform, Input.mousePosition, null);
                    
                    if (element.cursorInRect && element.FlagMouseInsideUI)
                        MouseInsideUI = true;
                    continue;
                }

                element.cursorInRect = mouseInside;
            }
        }
    }
}