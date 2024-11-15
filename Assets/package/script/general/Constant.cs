using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 定数
/// </summary>
public partial class Constant
{
    #region 共通

    /// <summary>
    /// サインカーブ
    /// </summary>
    public enum SinCurveType : int
    {
        /// <summary>加速</summary>
        Accel = 0,
        /// <summary>減速</summary>
        Decel,
        /// <summary>加減速</summary>
        Both,
    }

    /// <summary>
    /// 画面幅
    /// </summary>
    public const float SCREEN_WIDTH = 960f;
    /// <summary>
    /// 画面高さ
    /// </summary>
    public const float SCREEN_HEIGHT = 540f;

    /// <summary>
    /// 向き
    /// </summary>
    public enum Direction2D : int
    {
        None = 0,
        Up,
        Down,
        Left,
        Right,
    }

    #endregion
}
