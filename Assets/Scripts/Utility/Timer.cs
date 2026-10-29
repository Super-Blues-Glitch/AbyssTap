using System;
using UnityEngine;

public enum TimerType
{
    Countdown,
    Stopwatch
}

public enum TimerEvent
{
    Start, Update, End
}

namespace Utils
{
    public class Timer
    {
        public TimerType TimerType
        {
            get => timerType;
            set => timerType = value;
        }

        private TimerType timerType;

        public float TTime
        {
            get => time;
            set => time = value;
        }

        private float time;
        private bool ended = true;

        private bool stopTimer;

        public Action timerStarted;
        public Action timerUpdated;
        public Action timerEnded;


        public void Update()
        {
            if (timerType == TimerType.Countdown && !ended && !stopTimer)
            {
                DoCountdown();
            }
            else if (timerType == TimerType.Stopwatch)
            {
                DoStopwatch();
            }
        }

        public void Countdown(float initial_Time)
        {
            time = initial_Time;
            stopTimer = false;
            ended = false;
        }

        private void DoCountdown()
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                timerUpdated?.Invoke();
            }
            else if (time <= 0)
            {
                ended = true;
                timerEnded?.Invoke();
            }
        }

        private void DoStopwatch()
        {
            time += Time.deltaTime;
            timerUpdated?.Invoke();
        }

        public void timeToUnits(out int mins, out int scs)
        {
            mins = (int)(time / 60);
            scs = (int)(time - mins * 60f);
        }

        public void RemoveTimer()
        {
            stopTimer = true;
            timerEnded?.Invoke();
        }

        public void StopTimer()
        {
            stopTimer = true;
        }
    }
}
