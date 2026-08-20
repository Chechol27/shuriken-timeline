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
        if (evt.button == 0 && control.worldBound.Contains(control.parent.LocalToWorld(evt.mousePosition)))
        {
            isDragging = true;
        }
    }

    void SetNotDragging(MouseUpEvent evt)
    {
        if (evt.button == 0)
        {
            isDragging = false;
        }
    }

    void Drag(MouseMoveEvent evt)
    {
        if (!isDragging) return;
        float xDelta = evt.mouseDelta.x;
        if (leftPanel.resolvedStyle.width >= leftPanel.resolvedStyle.maxWidth.value) return;
        //TODO: Resize left panel
        //Move and resize right panel
        control.style.translate = new Vector2(control.style.translate.value.x.value + xDelta, 0);
        leftPanel.style.width = Mathf.Max(0, leftPanel.resolvedStyle.width + xDelta);
        evt.StopPropagation();
    }
    
    public void Init(VisualElement control, VisualElement leftPanel, VisualElement rightPanel)
    {
        this.control = control;
        this.leftPanel = leftPanel;
        this.rightPanel = rightPanel;
        control.parent.RegisterCallback<MouseDownEvent>(SetIsDragging);
        control.parent.RegisterCallback<MouseUpEvent>(SetNotDragging);
        control.parent.RegisterCallback<MouseMoveEvent>(Drag);
    }
}
