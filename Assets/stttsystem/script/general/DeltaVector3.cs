using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DeltaFloat�@Vector3�o�[�W����
/// </summary>
public class DeltaVector3
{
    private DeltaFloat _delta = new DeltaFloat();
    private Vector3 _target;
    private Vector3 _before;

    /// <summary>
    /// �ړ���
    /// </summary>
    /// <returns></returns>
    public bool IsActive() { return _delta.IsActive(); }

    /// <summary>
    /// ���ݒl
    /// </summary>
    /// <returns></returns>
    public Vector3 Get()
    {
        var now = _delta.Get();
        return _before + (_target - _before) * now;
    }

    /// <summary>
    /// �����ύX
    /// </summary>
    /// <param name="val"></param>
    public void Set(Vector3 val)
    {
        _before = val;
        _target = val;
        _delta.Set(0);
    }

    /// <summary>
    /// �I�t�Z�b�g�ǉ�
    /// </summary>
    /// <param name="ofs"></param>
    public void AddOffset(float ofs) { _delta.AddOffset(ofs); }

    /// <summary>
    /// �w�莞�ԂŒl��ς���
    /// </summary>
    /// <param name="_val"></param>
    /// <param name="_time"></param>
    /// <param name="_moveType"></param>
    public void MoveTo(Vector3 _val, float _time, DeltaFloat.MoveType _moveType)
    {
        _before = Get();

        _target = _val;
        _delta.Set(0);
        _delta.MoveTo(1, _time, _moveType);
    }

    /// <summary>
    /// �X�V
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update(float deltaTime)
    {
    }
}
