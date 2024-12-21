using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �J�����N���X
/// </summary>
public class MainCamera2D : MonoBehaviour
{
    #region �ϐ�

    /// <summary>�J�����ړ����x</summary>
    private const float CAM_SPEED = 1000f;
    /// <summary>���ݒ苗��</summary>
    private const float IMMEDIATE_DISTANCE = CAM_SPEED / 60f;

    /// <summary>���݈ʒu</summary>
    private Vector2 basePos;
    /// <summary>�ڕW�ʒu</summary>
    private Vector2 targetPos;

    /// <summary>�V�F�C�N�Ǘ�</summary>
    private Shaker shaker;

    #endregion

    #region ���

    /// <summary>
    /// ������
    /// </summary>
    void Start()
    {
        basePos = new Vector2(0, 0);
        targetPos = new Vector2(0, 0);
        shaker = new Shaker();
    }

    /// <summary>
    /// �t���[������
    /// </summary>
    void Update()
    {
        // �ڕW�ʒu������Ă���ꍇ
        var distance = targetPos - basePos;
        var length = distance.magnitude;
        if (length < IMMEDIATE_DISTANCE)
        {
            basePos = targetPos;
        }
        else
        {
            var deltaLen = CAM_SPEED * Time.deltaTime;
            if (deltaLen >= length)
                basePos = targetPos;
            else
                basePos += distance.normalized * deltaLen;
        }

        // �V�F�C�N����
        var shakeY = 0f;
        if (shaker.IsActive())
        {
            shaker.Update();
            shakeY = shaker.GetShakeValue();
        }

        // �ʒu�ݒ�
        transform.position = new Vector3(basePos.x, basePos.y + shakeY, -10);
    }

    #endregion

    #region �ʒu�Ǘ�

    /// <summary>
    /// �ʒu�ݒ�
    /// </summary>
    /// <param name="pos"></param>
    public void SetTargetPos(Vector2 pos)
    {
        targetPos = pos;

        var distance = targetPos - basePos;
        if (distance.magnitude < IMMEDIATE_DISTANCE)
        {
            Immediate();
        }
    }
    /// <summary>
    /// �ʒu�ݒ�
    /// </summary>
    /// <param name="_object"></param>
    public void SetTargetPos(GameObject _object)
    {
        SetTargetPos(_object.transform.position);
    }
    /// <summary>
    /// ���ݒ�
    /// </summary>
    public void Immediate()
    {
        basePos = targetPos;

        var shakeY = 0f;
        if (shaker.IsActive())
        {
            shakeY = shaker.GetShakeValue();
        }
        transform.position = new Vector3(basePos.x, basePos.y + shakeY, -10);
    }

    #endregion

    #region �V�F�C�N�Ǘ�

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
