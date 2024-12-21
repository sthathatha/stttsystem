using Codice.CM.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�����N���X
/// </summary>
public class MainCamera3D : MonoBehaviour
{
    #region �萔

    /// <summary>��������������deltaTime����������̂ŏ��</summary>
    private const float DELTATIME_LIMIT = 0.06f;

    /// <summary>�J������]���x</summary>
    private const float ROT_SPEED = 70f;

    /// <summary>�J�����������x</summary>
    private const float DIST_SPEED = 20f;

    /// <summary>
    /// ��������������deltaTime����������̂ŏ��
    /// </summary>
    private float MaxDelta
    {
        get
        {
            var delta = Time.deltaTime;
            return delta > DELTATIME_LIMIT ? DELTATIME_LIMIT : delta;
        }
    }

    #endregion

    #region �����o�[

    /// <summary>�J���������ő�</summary>
    public float dist_max { get; set; } = 10f;
    /// <summary>�J���������ŏ�</summary>
    public float dist_min { get; set; } = 3f;

    /// <summary>�J�����ړ���̌��E</summary>
    public float rot_up_limit { get; set; } = Mathf.PI * 0.3f;
    /// <summary>�J�����ړ����̌��E</summary>
    public float rot_down_limit { get; set; } = -Mathf.PI * 0.05f;

    #endregion

    #region �ϐ�

    /// <summary>���_</summary>
    private DeltaVector3 targetPos;

    /// <summary>�������E</summary>
    private DeltaFloat rotLR;
    /// <summary>�����㉺</summary>
    private DeltaFloat rotUD;
    /// <summary>����</summary>
    private DeltaFloat distance;

    /// <summary>�V�F�C�N�Ǘ�</summary>
    private Shaker shaker;

    #endregion

    #region ���

    /// <summary>
    /// ������
    /// </summary>
    void Start()
    {
        targetPos = new DeltaVector3();
        rotLR = new DeltaFloat();
        rotUD = new DeltaFloat();
        distance = new DeltaFloat();
        targetPos.Set(Vector3.zero);
        rotLR.Set(0f);
        rotUD.Set(0f);
        distance.Set(0f);

        shaker = new Shaker();
    }

    /// <summary>
    /// �t���[������
    /// </summary>
    void Update()
    {
        // �ړ����̂��̂�����΍X�V
        if (targetPos.IsActive() ||
            rotUD.IsActive() ||
            rotLR.IsActive() ||
            distance.IsActive() ||
            shaker.IsActive())
        {
            targetPos.Update(Time.deltaTime);
            rotUD.Update(Time.deltaTime);
            rotLR.Update(Time.deltaTime);
            distance.Update(Time.deltaTime);
            shaker.Update();

            UpdateCamera();
        }
    }

    #endregion

    #region �ʒu�Ǘ�

    /// <summary>
    /// ���݂̃p�����[�^�ňʒu�ƌ������v�Z
    /// </summary>
    private void UpdateCamera()
    {
        // ���E�p�x��0�`2�΂�
        if (!rotLR.IsActive())
            rotLR.Set(Util.GetNormalRadian(rotLR.Get()));

        // ��]���v�Z
        var quatUD = Quaternion.Euler(rotUD.Get(), 0, 0);
        var quatLR = Quaternion.Euler(0, rotLR.Get(), 0);
        var quat = quatLR * quatUD;
        transform.rotation = quat;

        // �ʒu���v�Z
        // �������̃x�N�g��
        var distVec = quat * new Vector3(0, 0, -distance.Get());
        transform.position = targetPos.Get() - distVec;
    }

    /// <summary>
    /// �ʒu�ݒ�
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="time"></param>
    public void SetTargetPos(Vector3 pos, float time = -1f)
    {
        if (time <= 0f)
        {
            targetPos.Set(pos);
            UpdateCamera();
        }
        else
            targetPos.MoveTo(pos, time, DeltaFloat.MoveType.BOTH);
    }
    /// <summary>
    /// �ʒu�ݒ�
    /// </summary>
    /// <param name="_object"></param>
    public void SetTargetPos(GameObject _object, float time = -1f)
    {
        SetTargetPos(_object.transform.position, time);
    }

    /// <summary>
    /// �J���������ύX
    /// </summary>
    /// <param name="rate"></param>
    public void SetCameraDist(float rate)
    {
        if (distance.IsActive()) return;

        var dist = distance.Get() + rate * MaxDelta * DIST_SPEED;
        distance.Set(Util.GetClampF(dist, dist_min, dist_max));

        UpdateCamera();
    }
    /// <summary>
    /// �J�������������ԂŌŒ�l�ɂ���
    /// </summary>
    /// <param name="dist"></param>
    /// <param name="time"></param>
    public void SetCameraDistTime(float dist, float time = -1f)
    {
        dist = Util.GetClampF(dist, dist_min, dist_max);

        if (time <= 0f)
        {
            distance.Set(dist);
            UpdateCamera();
        }
        else distance.MoveTo(dist, time, DeltaFloat.MoveType.BOTH);
    }

    /// <summary>
    /// ���E��]
    /// </summary>
    /// <param name="rate">1�ŉE�A-1�ō��A���l�ő��x</param>
    public void SetRotateLR(float rate)
    {
        if (rotLR.IsActive()) return;

        rotLR.Set(rotLR.Get() + rate * MaxDelta);
        UpdateCamera();
    }

    /// <summary>
    /// �㉺��]
    /// </summary>
    /// <param name="rate">1�ŏ�A-1�ŉ��A���l�ő��x</param>
    public void SetRotateUD(float rate)
    {
        if (rotUD.IsActive()) return;

        var newUD = rotUD.Get() + rate * MaxDelta * ROT_SPEED;

        rotUD.Set(Util.GetClampF(newUD, rot_down_limit, rot_up_limit));
        UpdateCamera();
    }
    /// <summary>
    /// ��]�����ԂŌŒ�l�ɂ���
    /// </summary>
    /// <param name="vec">����</param>
    /// <param name="time">����</param>
    public void SetRotateTime(Vector3 vec, float time = -1f)
    {
        // ���E�̌v�Z
        var noYVec = new Vector3(vec.x, 0f, vec.z).normalized;
        var lr = Mathf.Atan2(noYVec.z, noYVec.x);
        // ���E�͈ړ��������Z������I��
        if (lr - rotLR.Get() > Mathf.PI)
            lr -= Mathf.PI * 2f;
        else if (rotLR.Get() - lr > Mathf.PI)
            lr += Mathf.PI * 2f;

        // �㉺�̌v�Z
        var noVec = vec.normalized;
        var ud = Mathf.Atan2(Mathf.Sqrt(noVec.x * noVec.x + noVec.z * noVec.z), noVec.y);
        ud = Util.GetClampF(ud, rot_down_limit, rot_up_limit);

        if (time <= 0f)
        {
            rotLR.Set(lr);
            rotUD.Set(ud);
            UpdateCamera();
        }
        else
        {
            rotLR.MoveTo(lr, time, DeltaFloat.MoveType.BOTH);
            rotUD.MoveTo(ud, time, DeltaFloat.MoveType.BOTH);
        }
    }

    #endregion

    #region �V�F�C�N

    /// <summary>
    /// �V�F�C�N1��
    /// </summary>
    /// <param name="size"></param>
    /// <param name="time"></param>
    public void PlayShakeOne(Shaker.ShakeSize size, float time = 1f)
    {
        shaker.PlayOne(size, time);
    }
    /// <summary>
    /// �V�F�C�N�i�v
    /// </summary>
    /// <param name="size"></param>
    public void PlayShake(Shaker.ShakeSize size)
    {
        shaker.Play(size);
    }
    /// <summary>
    /// �V�F�C�N��~
    /// </summary>
    public void StopShake()
    {
        shaker.Stop();
    }

    #endregion
}
