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
}
