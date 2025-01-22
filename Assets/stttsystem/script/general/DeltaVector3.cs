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
