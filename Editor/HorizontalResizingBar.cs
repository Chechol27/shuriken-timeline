using UnityEngine;
using UnityEngine.UIElements;

public class HorizontalResizingBar : ScriptableObject
{
    private VisualElement control;
    private VisualElement leftPanel;
    private VisualElement rightPanel;
    
    private bool isDragging;

    void SetIsDragging(MouseDownEvent evt)
    {
        if (evt.button == 0) isDragging = true;
    }

    void SetNotDragging(PointerCancelEvent evt)
    {
        isDragging = false;
    }

    void Drag(MouseMoveEvent evt)
    {
        float xDelta = evt.mouseDelta.x;
        leftPanel.style.width = new Length(leftPanel.style.width)
        
    }
    
    public void Init(VisualElement control, VisualElement leftPanel, VisualElement rightPanel)
    {
        this.control = control;
        this.leftPanel = leftPanel;
        this.rightPanel = rightPanel;
        control.RegisterCallback<MouseDownEvent>(SetIsDragging);
        control.RegisterCallback<PointerCancelEvent>(SetNotDragging);
        control.RegisterCallback<MouseMoveEvent>(Drag);
    }
}
