using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public enum MoveDir
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
    public GameScene _scene;

    public class Velo
    {
        public float x = 0;
        public float y = 0;

        public void SetVeloZero()
        {
            this.x = 0;
            this.y = 0;
        }

        public Vector2 getVeloVector2()
        {
            return new Vector2(x, y);
        }

        public void setVelo(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
    }

    public MoveDir _direction;

    public MoveDir DirState
    {
        get
        {
            return _direction;
        }

        set
        {
            _direction = value;
        }
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        UpdateController();
    }

    protected virtual void Init()
    {
        _scene = GameObject.Find("GameScene").GetComponent<GameScene>();
    }

    protected virtual void UpdateController()
    {

    }

    protected virtual void OnMouseEnter()
    {

    }

    protected virtual void OnMouseExit()
    {

    }
    protected virtual void UpdateInferfaceText() { }

    protected virtual void OnDrawGizmos() { }

    public virtual void Damaged(float damage)
    {

    }
}
