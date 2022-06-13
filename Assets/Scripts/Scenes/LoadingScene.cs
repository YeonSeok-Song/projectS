using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : BaseScene
{

    GameObject _processBar;
    GameObject _processText;
    GameObject _backGround;

    Coroutine ImageChangeCor;

    public override void Clear()
    {
        
    }

    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.Loading;

        GameObject go = GameObject.Find("LoadingUI");
        if (go == null)
        {
            return;
        }

        _processBar = Utils.FindChild(go, "ProcessBar");
        _backGround = Utils.FindChild(go, "BackGround");

        _processText = Utils.FindChild(_processBar, "Text");

    }

    protected override void Start()
    {
        base.Start();
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator ImageChange()
    {
        yield return new WaitForSeconds(5.0f);
        BackImageChange();
        ImageChangeCor = null;
    }

    protected void BackImageChange()
    {
        Image image = _backGround.GetComponent<Image>();
        //image.sprite = 
        //todo: 이미지 랜덤 선택 후 랜더링
    }

    IEnumerator LoadSceneProcess()
    {
        if (ImageChangeCor != null)
        {
            ImageChangeCor = StartCoroutine(ImageChange());
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(Managers.Scene._nextSceneName);
        Slider s = _processBar.GetComponent<Slider>();
        Text t = _processText.GetComponent<Text>();



        op.allowSceneActivation = false;

        float timer = 0.0f;
        while(!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                s.value = op.progress;
                double temp = Math.Round((op.progress * 100.0f), 2);
                t.text = temp.ToString();
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                s.value = Mathf.Lerp(0.9f, 1f, timer);
                t.text = (s.value * 100.0f).ToString();

                if (s.value >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }

            }
        }
    }

    // 백그라운드 이미지 선택해서 저장
    // 혹은 일정 시간에 이미지 바뀌기 가능

}
