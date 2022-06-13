using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeController : UIBaseController
{
    [SerializeField]
    public Sprite _heartImage;

    [SerializeField]
    public Sprite _plusImage;

    List<GameObject> _lifes;
    PlayerController _player;

    int _lifeCount;

    public int LifeCount
    {
        get
        {
            return _lifeCount;
        }
        set
        {
            if (_lifeCount != value)
            {
                _lifeCount = value;
            }
        }
    }

    protected override void Init()
    {
        base.Init();
        _lifes = Utils.FindChilds(transform.gameObject, "heart", false, true);
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _lifeCount = Define.LIFE_MAX_COUNT;
    }

    protected override void UpdateController()
    {
        base.UpdateController();
        UpdateHpUI();
    }

    void UpdateHpUI()
    {
        if (LifeCount != _player.Stat.hp)
        {
            if (LifeCount > Define.LIFE_MAX_COUNT)
            {
                int dif = _player.Stat.hp - LifeCount;

                if (dif < 0)
                {
                    if (LifeCount - dif <= Define.LIFE_MAX_COUNT)
                    {
                        _lifes[9].GetComponent<Image>().sprite = _heartImage;

                        for(int i = 0; i < (-1 * dif); i++)
                        {
                            _lifes[LifeCount - i - 1].SetActive(false);
                        }
                    }
                }
            }
            else
            {
                int dif = _player.Stat.hp - LifeCount;

                if (dif > 0)
                {
                    if (dif + LifeCount > Define.LIFE_MAX_COUNT)
                    {
                        // 마지막 하트 +로 바꿈
                        _lifes[9].GetComponent<Image>().sprite = _plusImage;
                    }
                    for (int i = 0; i < dif; i++)
                    {
                        _lifes[LifeCount + i - 1].SetActive(true);
                    }
                }
                else
                {
                    //그냥 차이만큼 뺀다.
                    for (int i = 0; i < (-1 * dif); i++)
                    {
                        _lifes[LifeCount - i - 1].SetActive(false);
                    }
                }

            }

            LifeCount = _player.Stat.hp;

        }
    }
}
