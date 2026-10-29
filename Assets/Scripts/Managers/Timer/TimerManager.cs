using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class TimerManager : MonoBehaviour
{
    private Dictionary<string, Timer> timers = new();

    private void OnDisable()
    {
        foreach (var timer in timers)
        {
            timer.Value.timerStarted = null;
            timer.Value.timerUpdated = null;
            timer.Value.timerEnded = null;
        }
        
        timers.Clear();
    }

    private void Update()
    {
        foreach (var timer in timers)
        {
            timer.Value.Update();
        }
    }

    public void CreateTimer(string timerName, TimerType type, TimerEvent evt, Action func)
    {
        if (timers.ContainsKey(timerName)) 
        { 
            Debug.LogWarning("Existing key, adding to original");
            AddToTimer(timerName, evt, func); 
            return;
        }
            
        Timer new_Timer = new();
        new_Timer.TimerType = type;

        switch (evt)
        {
            case TimerEvent.Start:
                new_Timer.timerStarted += func;
                break;
            
            case TimerEvent.Update:
                new_Timer.timerUpdated += func;
                break;
            
            case TimerEvent.End:
                new_Timer.timerEnded += func;
                break;
        }
        
        timers.Add(timerName, new_Timer);
    }

    public void AddToTimer(string timerName, TimerEvent evt, Action func)
    {
        if (!timers.ContainsKey(timerName)) { return; }
        
        switch (evt)
        {
            case TimerEvent.Start:
                timers[timerName].timerStarted += func;
                break;
            
            case TimerEvent.Update:
                timers[timerName].timerUpdated += func;
                break;
            
            case TimerEvent.End:
                timers[timerName].timerEnded += func;
                break;
        }
    }
    
    public void StopTimer(string timerName)
    {
        if (!timers.ContainsKey(timerName)) { return; }
        
        timers[timerName].StopTimer();
    }
    
    public void RemoveFucTimer(string timerName, TimerEvent evt, Action func)
    {
        if (!timers.ContainsKey(timerName)) { return; }
        
        switch (evt)
        {
            case TimerEvent.Start:
                timers[timerName].timerStarted -= func;
                break;
            
            case TimerEvent.Update:
                timers[timerName].timerUpdated -= func;
                break;
            
            case TimerEvent.End:
                timers[timerName].timerEnded -= func;
                break;
        }
    }

    public void RemoveTimer(string timerName, TimerEvent evt, Action func)
    {
        if (!timers.ContainsKey(timerName)) { return; }
        
        timers[timerName].RemoveTimer();
        
        switch (evt)
        {
            case TimerEvent.Start:
                timers[timerName].timerStarted -= func;
                break;
            
            case TimerEvent.Update:
                timers[timerName].timerUpdated -= func;
                break;
            
            case TimerEvent.End:
                timers[timerName].timerEnded -= func;
                break;
        }
        
        timers.Remove(timerName);
    }
    
    public void timeToUnits(string timerName, out int mins, out int scs)
    {
        if (!timers.ContainsKey(timerName))
        {
            mins = 0;
            scs = 0;
            return;
        }
        
        timers[timerName].timeToUnits(out mins, out scs);
    }

    public void StartCountdown(string timerName, float time)
    {
        if (!timers.ContainsKey(timerName)) { return; }
        
        timers[timerName].Countdown(time);
    }

    public float TimerTime(string timerName)
    {
        if (!timers.ContainsKey(timerName)) { return -100; }

        return timers[timerName].TTime;
    }
}
