using System;

public static class EventHandler
{
    public static event Action<int> GetPointEvent; 
    public static event Action GameOverEvent;
    public static event Action JumpEvent;
    
    public static void CallGameOverEvent()
    {
        GameOverEvent?.Invoke();
    }
    
    public static void CallGetPointEvent(int score)
    {
        GetPointEvent?.Invoke(score);
    }
    
    public static void CallJumpEvent()
    {
        JumpEvent?.Invoke();
    }
}
