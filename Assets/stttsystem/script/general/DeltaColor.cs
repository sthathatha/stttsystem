using UnityEngine;

/// <summary>
/// DeltaFloat  Color�o�[�W����
/// </summary>
public class DeltaColor
{
    private DeltaFloat _delta = new DeltaFloat();
    private Color _target;
    private Color _before;

    /// <summary>
    /// �ړ���
    /// </summary>
    /// <returns></returns>
    public bool IsActive() { return _delta.IsActive(); }

    /// <summary>
    /// ���ݒl
    /// </summary>
    /// <returns></returns>
    public Color Get()
    {
        var now = _delta.Get();
        var r = Util.CalcBetweenFloat(now, _before.r, _target.r);
        var g = Util.CalcBetweenFloat(now, _before.g, _target.g);
        var b = Util.CalcBetweenFloat(now, _before.b, _target.b);
        var a = Util.CalcBetweenFloat(now, _before.a, _target.a);
        return new Color(r, g, b, a);
    }

    /// <summary>
    /// �����ύX
    /// </summary>
    /// <param name="val"></param>
    public void Set(Color val)
    {
        _before = val;
        _target = val;
        _delta.Set(0);
    }

    /// <summary>
    /// �w�莞�ԂŒl��ς���
    /// </summary>
    /// <param name="_val"></param>
    /// <param name="_time"></param>
    /// <param name="_moveType"></param>
    public void MoveTo(Color _val, float _time, DeltaFloat.MoveType _moveType)
    {
        _before = Get();
        _target = _val;

        _delta.Set(0);
        _delta.MoveTo(1, _time, _moveType);
    }

    /// <summary>
    /// �n�����l�ōX�V�i����ł��邪float�̌덷���o��j
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Update(float deltaTime)
    {
        _delta.Update(deltaTime);
    }

    /// <summary>
    /// ���A���^�C���ł̍X�V�i���m�����~�܂�Ȃ��j
    /// </summary>
    public void Update()
    {
        _delta.Update();
    }
}
