using UnityEngine;

/// <summary>
/// ���ԕω�����l
/// </summary>
public class DeltaFloat
{
    /// <summary>
    /// �ω��^�C�v
    /// </summary>
    public enum MoveType : int
    {
        /// <summary>����</summary>
        LINE = 0,
        /// <summary>����</summary>
        ACCEL,
        /// <summary>����</summary>
        DECEL,
        /// <summary>������</summary>
        BOTH,
    }

    private MoveType moveType;
    private float startTime;
    private float endTime;

    private float beforeValue;
    private float afterValue;
    private bool active;

    private float offset;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public DeltaFloat()
    {
        startTime = 0;
        endTime = 0;
        offset = 0;

        active = false;
    }

    /// <summary>
    /// �I�t�Z�b�g�ǉ�
    /// </summary>
    /// <param name="ofs"></param>
    public void AddOffset(float ofs) { offset += ofs; }

    /// <summary>
    /// �ړ���
    /// </summary>
    /// <returns></returns>
    public bool IsActive() { return active; }

    /// <summary>
    /// ���ݒl
    /// </summary>
    /// <returns></returns>
    public float Get()
    {
        if (IsActive() == false)
        {
            return afterValue;
        }

        var nowTime = Time.realtimeSinceStartup - startTime + offset;
        if (nowTime > endTime)
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
    /// �����ύX
    /// </summary>
    /// <param name="val"></param>
    public void Set(float val)
    {
        beforeValue = val;
        afterValue = val;
        endTime = 0;
        active = false;
    }

    /// <summary>
    /// �w�莞�ԂŒl��ς���
    /// </summary>
    /// <param name="_val"></param>
    /// <param name="_time"></param>
    /// <param name="_moveType"></param>
    public void MoveTo(float _val, float _time, MoveType _moveType)
    {
        beforeValue = Get();

        startTime = Time.realtimeSinceStartup;

        afterValue = _val;
        endTime = _time;

        active = true;
        moveType = _moveType;
    }

    /// <summary>
    /// �X�V
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update(float deltaTime)
    {
        if (!IsActive())
        {
            return;
        }

        var nowTime = Time.realtimeSinceStartup - startTime + offset;
        if (nowTime > endTime)
        {
            active = false;
            return;
        }
    }
}
