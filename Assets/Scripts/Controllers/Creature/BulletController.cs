using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Define;

public class BulletController : BaseController
{
    Coroutine _homingCor;

    float _speed = 2f;
    public Vector3 _dir;
    CircleCollider2D _col;
    Transform _target;
    public Transform _attacker;
    BulletType _type;

    public void SetBulletType(BulletType t)
    {
        _type = t;
    }
    public void SetTarget(Transform t)
    {
        _target = t;
    }

    IEnumerator Homing(float time)
    {
        yield return new WaitForSeconds(time);

        
    }

    public Vector3 HomingBullet()
    {
        float theta = 30.0f;
        float rad = Mathf.PI / 180 * theta;
        Vector3 dest = (_target.position - transform.position).normalized;

        // 뭔가 죽을때까지 따라간다... 수정이 필요하면 바꿀것.

        // 각도 구하기
        float dot = Vector3.Dot(transform.up, dest);
        // 내적(dot)이 1이면 같은 방향이니까 구할 필요 없음
        if (dot < 1.0f)
        {
            float x;
            float y;
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            
            // 외적(Cross)
            Vector3 cross = Vector3.Cross(transform.up, dest);

            // 외적 값에 따라 시계방향인지 반시계인지 판별
            // 반시계
            if (cross.z > 0.0f)
            {
                if (angle > rad)
                {
                    x = (dest.x * Mathf.Cos(rad)) + (Mathf.Sin(rad) * dest.y);
                    y = (-1 * dest.x * Mathf.Sin(rad)) + (Mathf.Cos(rad) * dest.y);
                }
                else
                {
                    x = (dest.x * Mathf.Cos(angle)) + (Mathf.Sin(angle) * dest.y);
                    y = (-1 * dest.x * Mathf.Sin(angle)) + (Mathf.Cos(angle) * dest.y);
                }
            }
            // 시계
            else
            {
                if (angle > rad)
                {
                    x = (dest.x * Mathf.Cos(rad)) - (Mathf.Sin(rad) * dest.y);
                    y = (dest.x * Mathf.Sin(rad)) + (Mathf.Cos(rad) * dest.y);
                }
                else
                {
                    x = (dest.x * Mathf.Cos(angle)) - (Mathf.Sin(angle) * dest.y);
                    y = (dest.x * Mathf.Sin(angle)) + (Mathf.Cos(angle) * dest.y);
                }
            }

            return new Vector3(x, y, 0).normalized;

        }

        return dest.normalized;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (_attacker.gameObject.CompareTag("Player"))
        {
            if (col.gameObject.CompareTag("Monster") || col.gameObject.CompareTag("Collision"))
            {
                if (col.gameObject.CompareTag("Monster"))
                {
                    float damage = _attacker.gameObject.GetComponent<PlayerController>().Stat.attack;
                    col.gameObject.GetComponent<MonsterController>().Damaged(damage);
                }

                Object.Destroy(this.gameObject);
            }
        }
        else if (_attacker.gameObject.CompareTag("Monster"))
        {
            if (col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Collision"))
            {
                if (col.gameObject.CompareTag("Player"))
                {
                    int damage = _attacker.gameObject.GetComponent<MonsterController>().Stat.attack;
                    col.gameObject.GetComponent<PlayerController>().Damaged(damage);
                }

                Object.Destroy(this.gameObject);
            }
        }
    }

    protected override void Init()
    {
        _col = GetComponent<CircleCollider2D>();
    }

    protected override void UpdateController()
    {
        if (_dir != null)
        {
            switch (_type) {

                case BulletType.Normal:
                    transform.position += _dir.normalized * _speed * Time.deltaTime;
                    break;

                case BulletType.Homing:
                    transform.position += HomingBullet() * _speed * Time.deltaTime;
                    break;

            }
        }

    }

}
