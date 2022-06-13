using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MonsterStat
{
	public int id;
	public string name;
	public float maxHp;
	public int attack;
	public float hp;
	public float defence;

	public MonsterStat(Monster m)
    {
		this.attack = 1;
		this.hp = (int)m.maxHp;
		this.maxHp = m.maxHp;
		this.id = m.id;
		this.name = m.name;
		this.defence = m.defense;
    }
}

public class MonsterController : BaseController
{                
	private Animator animator;                          
	public Transform _target;
	public bool _skipMove;
	public MonsterStat _mstat;

	public Vector3 _randomPos;

	public Coroutine _patrolCor = null;
	public Coroutine _attackCor = null;

	//Behavior tree

	float _speed = 0.2f;
	float _detectRange = 6.0f;
	float _attackRange = 5.0f;
	float _attackCoolTime = 2.0f;

	public MonsterStat Stat
    {
		get
        {
			return _mstat;
        }
		set
        {
			_mstat = value;
        }

    }


	public Dictionary<Define.State, bool> StateDict = new Dictionary<Define.State, bool>() {
		{ Define.State.isIdle, true },
		{ Define.State.isPossiveIdle, true },

		{ Define.State.isMove, false },
		{ Define.State.isPossiveMove, true },

		{ Define.State.isAttack, false },
		{ Define.State.isPossiveAttack, true },

		{ Define.State.isDead, false },
		{ Define.State.isPossiveDead, true },

		{ Define.State.isDetected, false },
		{ Define.State.isAttacked, false },

		{ Define.State.isSkilled, false },

		{ Define.State.isHostile, false },
		{ Define.State.isRangedAttack, true },
		{ Define.State.isRangedIn, false },

		{ Define.State.isDeadNormal, false },
		{ Define.State.isDeadExplode, false },
		{ Define.State.isDeadRevive, false },
	};


	protected override void Init()
    {
		base.Init();
		//_target = GameObject.FindGameObjectWithTag("Player").transform;
		Monster data = Managers.Data.MonsterDict[Utils.GetMonsterType(gameObject.name)];
		_mstat = new MonsterStat(data);
		StateDict[Define.State.isHostile] = true;
		StateDict[Define.State.isDeadNormal] = true;

		
	}


    protected override void UpdateController()
    {
		// �ൿ ��������
		DetectCheck();
		AttackRangeCheck();
		Managers.AI.MonsterAct(gameObject);

		// ����Ʈ��

	}

	public void IdleToMove()
	{
		StateDict[Define.State.isIdle] = false;
		StateDict[Define.State.isMove] = true;
	}

	public void IdleToAttack()
	{
		StateDict[Define.State.isIdle] = false;
		StateDict[Define.State.isAttack] = true;
	}

	public void MoveToIdle()
	{
		StateDict[Define.State.isIdle] = true;
		StateDict[Define.State.isMove] = false;
	}

	public void MoveToAttack()
	{
		StateDict[Define.State.isMove] = false;
		StateDict[Define.State.isAttack] = true;
	}

	public void AttackToMove()
	{
		StateDict[Define.State.isMove] = true;
		StateDict[Define.State.isAttack] = false;
	}


	public void AttackedCheck()
    {
		
		StateDict[Define.State.isDetected] = true;
		StateDict[Define.State.isAttacked] = true;
	}

	public void AllToDead()
    {
		StateDict[Define.State.isMove] = false;
		StateDict[Define.State.isAttack] = false;
		StateDict[Define.State.isIdle] = false;
		StateDict[Define.State.isDead] = false;
	}

	public void DetectCheck()
	{
		Debug.Log(_detectRange);
		Collider2D cols = Physics2D.OverlapCircle(transform.position, _detectRange, LayerMask.GetMask("Player"));

		if (cols != null)
        {
			if (StateDict[Define.State.isDetected] == false)
            {
				// Ž�� ���� �ȿ� ����
				_target = cols.transform;
				float dist = (_target.position - transform.position).magnitude;
				Debug.Log("Detect Range : " + dist);
				StateDict[Define.State.isDetected] = true;
			}
		}
		else
        {
			// ���� ������ ������
			if (StateDict[Define.State.isDetected] != false && StateDict[Define.State.isHostile] != true)
			{
				StateDict[Define.State.isDetected] = false;
				_target = null;
			}
		}
	}

	IEnumerator _patrolCool(float time)
	{
		yield return new WaitForSeconds(time);

		if (StateDict[Define.State.isIdle] == true)
        {
			IdleToMove();
			_patrolCor = null;
		}
		else
        {
			MoveToIdle();
			_patrolCor = null;
		}
	}

	IEnumerator _attackCool(float time)
    {
		yield return new WaitForSeconds(time);
		StateDict[Define.State.isPossiveAttack] = true;
		_attackCor = null;
	}


	public void RandomPosition()
    {
		_randomPos = (Vector3)Random.insideUnitCircle;
	}

	public void MoveRandomPos()
    {
		transform.position += _randomPos * _speed * 3 * Time.deltaTime;
	}

	public void MoveTarget()
    {
		if (_target != null && StateDict[Define.State.isDetected] == true)
        {
			Vector3 dest = transform.position + (_target.position - transform.position).normalized * _speed * Time.deltaTime;

			if (Managers.Map.CanGo(dest))
			{
				transform.position = dest;
			}
		}
	}

	

	public void StartPatrol()
    {
		if (_patrolCor == null)
		{
			RandomPosition();
			_patrolCor = StartCoroutine(_patrolCool(2.0f));
		}
			
	}

	public void EndPatrol()
	{
		MoveRandomPos();
		if (_patrolCor == null)
		{
			_patrolCor = StartCoroutine(_patrolCool(2.0f));
		}

	}

	public void AttackRangeCheck()
	{

		if (_target != null && StateDict[Define.State.isDetected] == true)
        {
			float dist = (_target.position - transform.position).magnitude;
			Debug.Log(dist);

			Collider2D cols = Physics2D.OverlapCircle(transform.position, _attackRange, LayerMask.GetMask("Player"));


			if (cols != null)
			{
				// Ž�� ���� �ȿ� ����
				StateDict[Define.State.isRangedIn] = true;
				MoveToAttack();
			}
			else
			{

				StateDict[Define.State.isRangedIn] = false;
				AttackToMove();
				
			}

			

			//if (dist < _attackRange)
			//{
			//	// ���� ���� �ȿ� ����
			//	Debug.Log("range in");
			//	StateDict[Define.State.isRangedIn] = true;
			//	MoveToAttack();
			//}
			//else
			//{
			//	// ���� ������ ������ 
			//	StateDict[Define.State.isRangedIn] = false;
			//	AttackToMove();
			//}
		}
	}

	public void MonsterAttack(int type)
    {
		switch(type)
        {
			case 1:
				OneWayShoot();
				break;

			case 2:
				NWayShoot(3);
				break;

			case 3:
				CircleShoot(7);
				break;

			case 4:
				SplitShoot();
				break;

			case 5:
				HomingShoot();
				break;

		}
    }

	protected void CreateBullet(Vector3 dir, BulletType type = BulletType.Normal, Transform target = null)
    {
		GameObject bullet = Managers.Resource.Instantiate($"Bullets/MonsterBullet");
		if (bullet == null)
		{
			return;
		}
		bullet.transform.position = transform.position;
		bullet.name = "bullet";
		BulletController bs = bullet.GetComponent<BulletController>();
		bs._dir = dir;
		bs._attacker = transform;
		bs.SetBulletType(type);
		bs.SetTarget(target);
	}
	
	public void OneWayShoot()
    {
		if (_target && StateDict[Define.State.isPossiveAttack])
        {
			Vector3 targetDir = (_target.position - transform.position).normalized;
			CreateBullet(targetDir);

			StateDict[Define.State.isPossiveAttack] = false;
			_attackCor = StartCoroutine(_attackCool(_attackCoolTime));

		}
		else if(_target == false || StateDict[Define.State.isPossiveAttack] == false)
        {
			MoveRandomPos();
		}
    }
	public void NWayShoot(int bulletCount)
	{
		if (_target && StateDict[Define.State.isPossiveAttack])
        {
			float theta = 30.0f;
			float radStep = Mathf.PI / 180 * theta;
			float rad;

			Vector3 targetDir = (_target.position - transform.position).normalized;
			
			if (bulletCount % 2 != 0)
			{
				rad = (-1 * bulletCount / 2 * radStep);
			}
			else
			{
				rad = ((float)((-1 * bulletCount / 2 + 0.5) * radStep));
			}

			for (int i = 0; i < bulletCount; i++, rad += radStep)
			{
				float x = (targetDir.x * Mathf.Cos(rad)) - (targetDir.y * Mathf.Sin(rad));
				float y = (targetDir.x * Mathf.Sin(rad)) + (targetDir.y * Mathf.Cos(rad));

				Vector3 temp = new Vector3(x, y, 0);

				CreateBullet(temp);
			}

			StateDict[Define.State.isPossiveAttack] = false;
			_attackCor = StartCoroutine(_attackCool(_attackCoolTime));
		}
		else if (_target == false || StateDict[Define.State.isPossiveAttack] == false)
		{
			MoveRandomPos();
		}

	}

	public void CircleShoot(int bulletCount)
	{
		if (_target && StateDict[Define.State.isPossiveAttack])
        {
			float radStep = Mathf.PI * 2 / bulletCount;
			float rad;

			Vector3 targetDir = (_target.position - transform.position).normalized;

			if (bulletCount % 2 != 0)
			{
				rad = radStep / 2;
			}
			else
			{
				rad = 0.0f;
			}

			for (int i = 0; i < bulletCount; i++, rad += radStep)
            {
				float x = Mathf.Cos(rad);
				float y = Mathf.Sin(rad);

				Vector3 temp = new Vector3(x, y, 0);

				CreateBullet(temp);
			}

			StateDict[Define.State.isPossiveAttack] = false;
			_attackCor = StartCoroutine(_attackCool(_attackCoolTime));

		}
		else if (_target == false || StateDict[Define.State.isPossiveAttack] == false)
		{
			MoveRandomPos();
		}
	}

	public void SplitShoot()
	{

	}

	public void HomingShoot()
	{
		if (_target && StateDict[Define.State.isPossiveAttack])
        {
			Vector3 targetDir = (_target.position - transform.position).normalized;
			CreateBullet(targetDir, BulletType.Homing, _target);

			StateDict[Define.State.isPossiveAttack] = false;
			_attackCor = StartCoroutine(_attackCool(_attackCoolTime));
		}
		else if (_target == false || StateDict[Define.State.isPossiveAttack] == false)
		{
			MoveRandomPos();
		}
	}

	public override void Damaged(float damage)
    {
		base.Damaged(damage);
		Stat.hp = Mathf.Clamp(Stat.hp - damage, 0.0f, Stat.hp - damage);

		if (Stat.hp == 0.0f)
        {
			AllToDead();
			AttackedCheck();
		}
	}

    private void OnDrawGizmos()
    {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, 6.0f);
	}

    public void Dead()
    {
		//todo: �ִϸ��̼� ����
		Managers.Resource.Destroy(gameObject);
	}

	public void ExplosionDead()
    {
		//todo: �ִϸ��̼� ����
	}

	public void ReviveDead()
    {
		//todo: �ִϸ��̼� ����

	}

}




#region Idle State
//bool isIdle; // ������ �ִ� �����ΰ�?
//bool isPossiveIdle; // ������ �ִ°� �����Ѱ�?
//bool isHostile; // �÷��̾ ����� �ϴ°�?

#endregion

#region Move State
//bool isMove; // ���� �����̴� �����ΰ�?
//bool isPossiveMove; // ���� �����̴°� �����Ѱ�?
//bool isDetect; // ��ǥ�� �����ߴ°�?

#endregion

#region Attack State
//bool isAttack; // ������ �����Ѱ�?
//bool isPossiveAttack; // ���� ������ �����Ѱ�?
//bool isRanged; // �÷��̾�� �Ÿ��� �����ϴ� �����ΰ�? (���Ÿ� Ÿ��)

#endregion

#region Dead State
//bool isDead; // ���� �����ΰ�?
//bool isPossiveDead; // �״°� ������ �����ΰ�? (���� ���� ���)
//bool isExplode; // ���� �� �����ϴ� Ÿ���ΰ�?
//bool isRevive; // ���� �� ��Ȱ�ϴ� Ÿ���ΰ�?

#endregion
