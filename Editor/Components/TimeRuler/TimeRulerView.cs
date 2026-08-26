using UnityEngine;
using UnityEngine.UIElements;


[UxmlElement]
public partial class TimeRulerView : VisualElement
{
    private TimeRulerViewModel viewModel;
    
    [UxmlAttribute] public bool showTimeCodes;
    
    private ControlData controlData;
    private VisualElement upperBarImguiContainer;
    private VisualElement backgroundImguiContainer;

    public void Bind(TimeRulerViewModel _viewModel)
    {
        viewModel = _viewModel;
        viewModel.OnStateChanged += MarkDirtyRepaint;
        MarkDirtyRepaint();
    }

    private void OnGenerateVisualContent(MeshGenerationContext ctx)
    {
        if (viewModel == null) return;

        // Time Bars
        Painter2D painter = ctx.painter2D;

        if (!showTimeCodes) return;
        //Time Codes
    }
    
    public TimeRulerView()
    {
        generateVisualContent += OnGenerateVisualContent;
    }
}
