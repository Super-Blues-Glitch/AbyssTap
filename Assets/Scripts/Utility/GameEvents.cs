using System;
using UnityEngine;

public static class GameEvents
{
    public struct Game
    {
        public static Action GameStarted;
        public static Action GamePaused;
        public static Action GameResumed;
        public static Action GameEnded;
        public static Action GameWon;
        public static Action GameEnd;
    }
}
