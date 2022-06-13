using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class UIManager
{
    public int ScreenWidth { get; private set; }
    public int ScreenHeight { get; private set; }

    ScreenOrientation _deviceDir;

    public enum CursorType
    {
        NORMAL,
        ATTACK,
        OBJECT
    }

    public enum DeviceDir
    {
        LANDSCAPE,
        PORTRAIT
    }

    CursorType _cursorType = CursorType.NORMAL;

    public RaycastHit2D _hit;
    Vector2 _ray;

    public Sprite[] _inputs;
    public Sprite[] _actions;
    public Sprite[] _rewards;
    public Sprite[] _stats;
    public Sprite[] _substances;

    public Texture2D _cursorAttack;
    public Texture2D _cursorNormal;
    public Texture2D _cursorObject;

    public void Init()
    {
        _inputs = Resources.LoadAll<Sprite>("Textures/Sprite/input");
        _actions = Resources.LoadAll<Sprite>("Textures/Sprite/input");
        _rewards = Resources.LoadAll<Sprite>("Textures/Sprite/input");
        _stats = Resources.LoadAll<Sprite>("Textures/Sprite/input");
        _substances = Resources.LoadAll<Sprite>("Textures/Sprite/input");

        _cursorAttack = Resources.Load<Texture2D>("Textures/Sprite/cursor_attack");
        _cursorNormal = Resources.Load<Texture2D>("Textures/Sprite/cursor_normal");
        _cursorObject = Resources.Load<Texture2D>("Textures/Sprite/cursor_object");

        SetMyCursor(CursorType.NORMAL);

        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;

        _deviceDir = Screen.orientation;

        SetDeviceDir();
        SetScreenSize(1920, 1080);
        //setScreen();
    }

    public void SetScreenSize(int width, int height)
    {
        ScreenWidth = width;
        ScreenHeight = height;
    }

    public void SetMyCursor(CursorType type)
    {
        //todo: UI용 커서도 추가
        switch (type)
        {
            case CursorType.NORMAL:
                Cursor.SetCursor(_cursorNormal, Vector2.zero, CursorMode.Auto);
                break;

            case CursorType.ATTACK:
                Cursor.SetCursor(_cursorAttack, Vector2.zero, CursorMode.Auto);
                break;

            case CursorType.OBJECT:
                Cursor.SetCursor(_cursorObject, Vector2.zero, CursorMode.Auto);
                break;
        }

    }

    private void SetDeviceDir()
    {
        
    }

    public void RotateScreen()
    {
        if (_deviceDir != Screen.orientation)
        {
            setScreen();
            _deviceDir = Screen.orientation;
        }
    }

    public void setScreen()
    {
        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        Screen.SetResolution(ScreenWidth, (int)( ((float)deviceHeight / deviceWidth) * ScreenWidth), true);

        if ((float)ScreenWidth / ScreenHeight < (float)deviceWidth / deviceHeight)
        {
            float newWidth = ((float)ScreenWidth / ScreenHeight) / ((float)deviceWidth / deviceHeight);

            Camera.main.rect = new Rect((1.0f - newWidth) / 2f, 0f, newWidth, 1.0f);
        }

        else
        {
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)ScreenWidth / ScreenHeight);
            Camera.main.rect = new Rect(0f, (1.0f - newHeight) / 2f, 1f, newHeight);
        }

    }

    public void setMainUI()
    {

    }
}


