using UnityEngine;

/// <summary>
/// 時間変化する値
/// </summary>
public class DeltaFloat
{
    /// <summary>
    /// 変化タイプ
    /// </summary>
    public enum MoveType : int
    {
        /// <summary>直線</summary>
        LINE = 0,
        /// <summary>加速</summary>
        ACCEL,
        /// <summary>減速</summary>
        DECEL,
        /// <summary>加減速</summary>
        BOTH,
    }

    private MoveType moveType;
    private float startTime;
    private float endTime;
    private float nowTime;

    private float beforeValue;
    private float afterValue;
    private bool active;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public DeltaFloat()
    {
        startTime = 0;
        nowTime = 0;
        endTime = 0;

        active = false;
    }

    /// <summary>
    /// 移動中
    /// </summary>
    /// <returns></returns>
    public bool IsActive() { return active; }

    /// <summary>
    /// 現在値
    /// </summary>
    /// <returns></returns>
    public float Get()
    {
        if (IsActive() == false)
        {
            return afterValue;
        }

        if (nowTime >= endTime)
        {
            active = false;
            return afterValue;
        }

        var timePer = nowTime / endTime;
        if (timePer < 0f) { timePer = 0f; }
        float valPer = moveType switch
        {
            MoveType.ACCEL => Util.SinCurve(timePer, Constant.SinCurveType.Accel),
            MoveType.DECEL => Util.SinCurve(timePer, Constant.SinCurveType.Decel),
            MoveType.BOTH => Util.SinCurve(timePer, Constant.SinCurveType.Both),
            _ => timePer
        };

        return Util.CalcBetweenFloat(valPer, beforeValue, afterValue);
    }

    /// <summary>
    /// 即時変更
    /// </summary>
    /// <param name="val"></param>
    public void Set(float val)
    {
        beforeValue = val;
        afterValue = val;
        nowTime = 0;
        endTime = 0;
        active = false;
    }

    /// <summary>
    /// 指定時間で値を変える
    /// </summary>
    /// <param name="_val"></param>
    /// <param name="_time"></param>
    /// <param name="_moveType"></param>
    public void MoveTo(float _val, float _time, MoveType _moveType)
    {
        beforeValue = Get();

        startTime = Time.realtimeSinceStartup;

        afterValue = _val;
        nowTime = 0;
        endTime = _time;

        active = true;
        moveType = _moveType;
    }

    /// <summary>
    /// 渡した値で更新（操作できるがfloatの誤差が出る）
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update(float deltaTime)
    {
        if (!IsActive())
        {
            return;
        }

        nowTime += deltaTime;
        if (nowTime >= endTime)
        {
            active = false;
            return;
        }
    }

    /// <summary>
    /// リアルタイムでの更新（正確だが止まらない）
    /// </summary>
    public void Update()
    {
        if (!IsActive())
        {
            return;
        }

        nowTime = Time.realtimeSinceStartup - startTime;
        if (nowTime >= endTime)
        {
            active = false;
            return;
        }
    }
}
