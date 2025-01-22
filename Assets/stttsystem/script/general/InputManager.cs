using UnityEngine.InputSystem;

/// <summary>
/// キーボード・ゲームパッド管理
/// </summary>
public class InputManager
{
    #region 定数
    public enum Keys : int
    {
        /// <summary>十字キー上</summary>
        Up = 0,
        /// <summary>十字キー下</summary>
        Down,
        /// <summary>十字キー右</summary>
        Right,
        /// <summary>十字キー左</summary>
        Left,
        /// <summary>４ボタンの下</summary>
        South,
        /// <summary>４ボタンの右</summary>
        East,
        /// <summary>４ボタンの左</summary>
        West,
        /// <summary>４ボタンの上</summary>
        North,
    }
    #endregion

    #region インスタンス
    private static InputManager _instance = null;
    public static InputManager GetInstance()
    {
        if (_instance == null)
        {
            _instance = new InputManager();
        }
        return _instance;
    }
    #endregion

    #region 押し状態判定
    /// <summary>
    /// キー入力維持中
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool GetKey(Keys key)
    {
        var keyboard = Keyboard.current;
        var gamepad = Gamepad.current;

        switch (key)
        {
            case Keys.Up:
                return (keyboard?.upArrowKey.isPressed == true ||
                    gamepad?.leftStick.up.isPressed == true ||
                    gamepad?.dpad.up.isPressed == true);
            case Keys.Down:
                return (keyboard?.downArrowKey.isPressed == true ||
                    gamepad?.leftStick.down.isPressed == true ||
                    gamepad?.dpad.down.isPressed == true);
            case Keys.Right:
                return (keyboard?.rightArrowKey.isPressed == true ||
                    gamepad?.leftStick.right.isPressed == true ||
                    gamepad?.dpad.right.isPressed == true);
            case Keys.Left:
                return (keyboard?.leftArrowKey.isPressed == true ||
                    gamepad?.leftStick.left.isPressed == true ||
                    gamepad?.dpad.left.isPressed == true);
            case Keys.South:
                return (keyboard?.enterKey.isPressed == true ||
                    gamepad?.buttonSouth.isPressed == true);
            case Keys.East:
                return (keyboard?.backspaceKey.isPressed == true ||
                    gamepad?.buttonEast.isPressed == true);
            case Keys.West:
                return (keyboard?.zKey.isPressed == true ||
                    gamepad?.buttonWest.isPressed == true);
            case Keys.North:
                return (keyboard?.xKey.isPressed == true ||
                    gamepad?.buttonNorth.isPressed == true);
        }

        return false;
    }

    /// <summary>
    /// 押した瞬間
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool GetKeyPress(Keys key)
    {
        var keyboard = Keyboard.current;
        var gamepad = Gamepad.current;

        switch (key)
        {
            case Keys.Up:
                return (keyboard?.upArrowKey.wasPressedThisFrame == true ||
                    gamepad?.leftStick.up.wasPressedThisFrame == true ||
                    gamepad?.dpad.up.wasPressedThisFrame == true);
            case Keys.Down:
                return (keyboard?.downArrowKey.wasPressedThisFrame == true ||
                    gamepad?.leftStick.down.wasPressedThisFrame == true ||
                    gamepad?.dpad.down.wasPressedThisFrame == true);
            case Keys.Right:
                return (keyboard?.rightArrowKey.wasPressedThisFrame == true ||
                    gamepad?.leftStick.right.wasPressedThisFrame == true ||
                    gamepad?.dpad.right.wasPressedThisFrame == true);
            case Keys.Left:
                return (keyboard?.leftArrowKey.wasPressedThisFrame == true ||
                    gamepad?.leftStick.left.wasPressedThisFrame == true ||
                    gamepad?.dpad.left.wasPressedThisFrame == true);
            case Keys.South:
                return (keyboard?.enterKey.wasPressedThisFrame == true ||
                    gamepad?.buttonSouth.wasPressedThisFrame == true);
            case Keys.East:
                return (keyboard?.backspaceKey.wasPressedThisFrame == true ||
                    gamepad?.buttonEast.wasPressedThisFrame == true);
            case Keys.West:
                return (keyboard?.zKey.wasPressedThisFrame == true ||
                    gamepad?.buttonWest.wasPressedThisFrame == true);
            case Keys.North:
                return (keyboard?.xKey.wasPressedThisFrame == true ||
                    gamepad?.buttonNorth.wasPressedThisFrame == true);
        }

        return false;
    }
    #endregion
}
