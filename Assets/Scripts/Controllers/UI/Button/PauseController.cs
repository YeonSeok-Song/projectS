using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [SerializeField]
    private Sprite _playIcon;

    [SerializeField]
    private Sprite _StopIcon;

    public bool Pause { get; private set; } = false;

    public void PuaseToggle()
    {
        if(Pause == true)
        {
            Time.timeScale = 1;
            Pause = false;
            transform.gameObject.GetComponent<Image>().sprite = _StopIcon;
        }
        else
        {
            Time.timeScale = 0;
            Pause = true;
            transform.gameObject.GetComponent<Image>().sprite = _playIcon;
        }
    }

}
