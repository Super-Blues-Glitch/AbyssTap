using UnityEngine;

public static class GameEnums
{
    public enum InputKey
    {
        Move, Jump
    }

    public enum CollisionType
    {
        CollEnter, CollStay, CollExit, TriggerEnter, TriggerStay, TriggerExit
    }

    public struct Audio
    {
        public enum AudioChannel
        {
            Master, Music, SFX, Voices
        }
    }
}
