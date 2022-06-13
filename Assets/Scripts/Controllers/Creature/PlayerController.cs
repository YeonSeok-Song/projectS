using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Data;

public class PlayerStat
{
    public int id;
    public string name;
    public float maxHp;
    public float attack;
    public int hp;

    public PlayerStat(Player p) 
    {
        this.attack = p.attack;
        this.hp = (int)(p.maxHp / 2);
        this.maxHp = p.maxHp;
        this.id = p.id;
        this.name = p.name;
    }

} 

public class PlayerController : BaseController
{
    int _coin = 9999;
    float _speed = 5.0f;
   
    float _dash = 2.0f;
    bool _dashCool = false;

    State _playerState;
    protected Animator _animator;
    Rigidbody2D _rigid;
    Coroutine _dashCoroutine;
    Velo _velo = new Velo();
    GameObject _position;
    GameObject _hitbox;
    GameObject _interfaceText;
    PlayerStat _stat;


    public enum State
    {
        IDLE,
        MOVE,
        DASH,
        DEAD

    }

    IEnumerator DashCoolTime(float time)
    {
        PlayerState = State.IDLE;
        yield return new WaitForSeconds(time);
        Debug.Log("Dash");
        _dashCoroutine = null;
        _dashCool = false;
    }

    public void ChangeDirAnim(MoveDir dir)
    {
        if (dir == MoveDir.LEFT)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    public PlayerStat Stat
    {
        get
        {
            return _stat;
        }
        set
        {
            _stat = value;
        }
    }

    public State PlayerState
    {
        get
        {
            return _playerState;
        }
        set
        {
            _playerState = value;
            ChangeDirAnim(DirState);
            AnimateUpdate();
        }
    }

    public void AnimateUpdate()
    {
        switch (_playerState)
        {
            case State.IDLE :
                _animator.Play("PlayerIdle");
                break;

            case State.MOVE:
                _animator.Play("PlayerMove");
                break;

            case State.DASH:
                _animator.Play("PlayerMove");
                break;

            case State.DEAD:
                _animator.Play("PlayerMove");
                break;
        }
    }

    //#region PC Controller
    //void GetMouseInput()
    //{
    //    if (Input.GetKeyDown(KeyCode.Mouse0))
    //    {
    //        Vector3 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //        mousePoint.z = 0;

    //        Vector3 mouseD = mousePoint - transform.position;
    //        //todo: 마우스방향으로 총을쏜다.
    //        GameObject obj = Resources.Load<GameObject>($"Prefabs/Shuriken");
    //        if (obj == null)
    //        {
    //            Debug.Log($"Failed to load prefab : Assets/Resources/Prefabs/Shuriken.prefab");
    //            return;
    //        }
    //        GameObject bullet = Instantiate(obj, null);
    //        bullet.transform.position = transform.position + mouseD.normalized;
    //        bullet.name = "suriken";
    //        BulletController bs = bullet.GetComponent<BulletController>();
    //        bs._dir = mouseD;
    //    }
    //}

    //void GetDashInput()
    //{
    //    if (Input.GetKeyDown(KeyCode.Mouse1) && !_dashCool)
    //    {
    //        if (!_dashCool)
    //        {
    //            PlayerState = State.DASH;
    //            _dashCool = true;
    //            //todo: 캔고 해서 도착위치가 못하는곳이면 1씩 빼서 될때까지 연산
    //            //todo: 갈수있으면 거기고 바로 이동
    //            //todo: 만약 갈수있는곳이 없으면 끝
    //            transform.position += new Vector3(_velo.x, _velo.y, 0).normalized * _dash;
    //            _dashCoroutine = StartCoroutine("DashCoolTime", 1f);
    //        }
    //        //todo: 서버 연결시 이동 패킷 전송
    //    }
    //    //todo: 서버 연결시 이동 패킷 전송


    //    //if (Input.GetKey(KeyCode.W))
    //    //{
    //    //    transform.position += transform.position.normalized + (Vector3.up * _speed * Time.deltaTime);
    //    //    DirState = MoveDir.UP;
    //    //}

    //    //if (Input.GetKeyDown(KeyCode.Mouse1) && !_dashCool)
    //    //{
    //    //    //transform.Translate(new Vector3(x, y, 0).normalized * 1.0f);
    //    //    // 이동은 상관없는데 점프같은 경우는 무조건 velocity를 사용
    //    //    ////todo: 쿨타임
    //    //    _dashCoroutine = StartCoroutine("DashCoolTime", 1f);
    //    //}
    //}

    //}

    public void GetMoveInput(Vector3 dir)
    {
        
        // 방향백터 수정
        float x = -1 * dir.x;
        float y = -1 * dir.y;

        if (x != 0 || y != 0)
        {
            if (x < 0)
            {
                DirState = MoveDir.LEFT;
            }

            else if (x > 0)
            {
                DirState = MoveDir.RIGHT;
            }
            _velo.setVelo(x, y);
            PlayerState = State.MOVE;
        }
        else
        {
            PlayerState = State.IDLE;
        }

    }

    public void Attack()
    {
        //todo: 조이스틱 방향으로 발싸
        //GameObject go = _scene.LeastRangeTarget();
        //if (go == null)
        //{
        //    return;
        //}

        //Vector3 targetDir = go.transform.position - transform.position;
        ////todo: 마우스방향으로 총을쏜다.
        //GameObject obj = Resources.Load<GameObject>($"Prefabs/Bullets/Shuriken");
        //if (obj == null)
        //{
        //    Debug.Log($"Failed to load prefab : Assets/Resources/Prefabs/Shuriken.prefab");
        //    return;
        //}
        //GameObject bullet = Instantiate(obj, null);
        //bullet.transform.position = transform.position + targetDir.normalized;
        //bullet.name = "suriken";
        //BulletController bs = bullet.GetComponent<BulletController>();
        //bs._dir = targetDir;
    }

    protected override void UpdateController()
    {
        // 캐릭터의 동작을 업데이트 하는것도
        // state로 관리하면 오작동할 확률이 낮다
        base.UpdateController();
        //GetTouchInput();

        #region PC Controller
        //GetMoveInput();
        //GetDashInput();
        //GetMouseInput();
        #endregion

        AnimateUpdate();

        switch (_playerState)
        {
            case State.IDLE:
                UpdateIdle();
                break;
            case State.MOVE:
                UpdateMoving();
                break;
            case State.DASH:
                UpdateDash();
                break;
            case State.DEAD:
                UpdateDead();
                break;
        }

        UpdateInferfaceText();
    }

    protected override void Init()
    {
        base.Init();
        _animator = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();

        _scene = GameObject.Find("GameScene").GetComponent<GameScene>();

        GameObject camera = GameObject.Find("CameraUI");
        _interfaceText = Utils.FindChild(camera, "InterfaceText");

        Player data = Managers.Data.PlayerDict[Utils.GetPlayerType(gameObject.name)];
        _stat = new PlayerStat(data);

    }

    protected void UpdateIdle()
    {
        _velo.SetVeloZero();
        _rigid.velocity = _velo.getVeloVector2();
    }

    protected void UpdateMoving()
    {
        Vector3 dest = transform.position + new Vector3(_velo.x, _velo.y, 0).normalized * _speed * Time.deltaTime;

        if (Managers.Map.CanGo(dest))
        {
            transform.position = dest;
        }
    }

    protected void UpdateDead()
    {
        // 부활 후 다시 시작
        _scene.ChangeMap(3);
        Stat.hp = (int)(Stat.maxHp / 2);
        PlayerState = State.IDLE;
    }

    protected void UpdateDash()
    {
        PlayerState = State.MOVE;
    }

    protected override void UpdateInferfaceText()
    {
        Collider2D cols = Physics2D.OverlapCircle(transform.position, Define.EVENT_RADIUS, LayerMask.GetMask("Object"));
        //Debug.Log($"close {cols}");
        if (cols != null)
        {
            base.UpdateInferfaceText();

            string[] parse = cols.name.Split('_');

            if (Int32.Parse(parse[1]) == (int)Define.EventObject.Gate) {

                _interfaceText.GetComponent<Text>().text = Managers.Data.MapDict[Int32.Parse(parse[2])].name;

            }
        }
        else
        {
            _interfaceText.GetComponent<Text>().text = "";
        }
    }

    public void Damaged(int damage)
    {
        Stat.hp = (int)Mathf.Clamp(Stat.hp - damage, 0.0f, Stat.hp - damage);

        if (Stat.hp == 0.0f)
        {
            PlayerState = State.DEAD;
        }

    }

    protected override void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(transform.position, Define.EVENT_RADIUS);
    }

}
