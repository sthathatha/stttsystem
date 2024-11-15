using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �萔
/// </summary>
public partial class Constant
{
    #region ����

    /// <summary>
    /// �T�C���J�[�u
    /// </summary>
    public enum SinCurveType : int
    {
        /// <summary>����</summary>
        Accel = 0,
        /// <summary>����</summary>
        Decel,
        /// <summary>������</summary>
        Both,
    }

    /// <summary>
    /// ��ʕ�
    /// </summary>
    public const float SCREEN_WIDTH = 960f;
    /// <summary>
    /// ��ʍ���
    /// </summary>
    public const float SCREEN_HEIGHT = 540f;

    /// <summary>
    /// ����
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
