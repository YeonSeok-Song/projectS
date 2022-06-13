using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public static int MAPSIZE = 32;
    public enum Scene
    {
        Unknown,
        Loading,
        Lobby,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,
    }

    public enum UIEvent
    {
        Click,
        Drag,
    }

    public enum EventObject
    {
        Stand,
        Gate,
        Respawn
    }

    public enum BehaviorState
    {
        Idle,
        Move,
        Attack,
        Dead
    }
    public enum BulletType
    {
        Normal,
        Homing,
    }

    public static int LIFE_MAX_COUNT = 10;
    public static float EVENT_RANGE = 2.0f;
    public static float EVENT_RADIUS = 2.0f;

    public enum State
    {
        isIdle,
        isPossiveIdle,

        isMove,
        isPossiveMove,

        isAttack,
        isPossiveAttack,

        isDead,
        isPossiveDead,

        isDetected,

        isAttacked,

        isSkilled,

        isHostile,
        isRangedAttack,
        isRangedIn,

        isDeadNormal,
        isDeadExplode,
        isDeadRevive,

        none,
    }
}
