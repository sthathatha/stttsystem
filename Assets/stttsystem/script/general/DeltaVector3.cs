using UnityEngine;

/// <summary>
/// DeltaFloat　Vector3バージョン
/// </summary>
public class DeltaVector3
{
    private DeltaFloat _delta = new DeltaFloat();
    private Vector3 _target;
    private Vector3 _before;

    /// <summary>
    /// 移動中
    /// </summary>
    /// <returns></returns>
    public bool IsActive() { return _delta.IsActive(); }

    /// <summary>
    /// 現在値
    /// </summary>
    /// <returns></returns>
    public Vector3 Get()
    {
        var now = _delta.Get();
        return _before + (_target - _before) * now;
    }

    /// <summary>
    /// 即時変更
    /// </summary>
    /// <param name="val"></param>
    public void Set(Vector3 val)
    {
        _before = val;
        _target = val;
        _delta.Set(0);
    }

    /// <summary>
    /// 指定時間で値を変える
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
    /// 渡した値で更新（操作できるがfloatの誤差が出る）
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update(float deltaTime)
    {
        _delta.Update(deltaTime);
    }

    /// <summary>
    /// リアルタイムでの更新（正確だが止まらない）
    /// </summary>
    public void Update()
    {
        _delta.Update();
    }
}
