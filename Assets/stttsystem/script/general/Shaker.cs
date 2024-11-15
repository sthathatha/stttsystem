using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �V�F�C�N�Ǘ��N���X
/// </summary>
public class Shaker
{
    /// <summary>
    /// �T�C�Y�w��
    /// </summary>
    public enum ShakeSize : int
    {
        /// <summary>�ア</summary>
        Weak = 0,
        /// <summary>����</summary>
        Middle,
        /// <summary>����</summary>
        Strong,
    }

    /// <summary>
    /// ���
    /// </summary>
    private enum ShakeState : int
    {
        /// <summary>�h��ĂȂ�</summary>
        Idle = 0,
        /// <summary>�h��Ă���</summary>
        Active,
        /// <summary>�~�܂����</summary>
        Fadeout,
    }

    /// <summary>�p���x</summary>
    private const float SHAKE_SPEED = Mathf.PI * 12f;

    /// <summary>�p�x</summary>
    private float shakeRot;
    /// <summary>�U��</summary>
    private DeltaFloat shakeWidth;
    /// <summary>�~�܂�܂Ŏ���</summary>
    private float shakeTime;

    /// <summary>���ݏ��</summary>
    private ShakeState state;

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public Shaker()
    {
        shakeTime = -1f;
        shakeRot = 0f;
        shakeWidth = new DeltaFloat();
        shakeWidth.Set(0);
    }

    /// <summary>
    /// �P��V�F�C�N
    /// </summary>
    /// <param name="size"></param>
    /// <param name="time"></param>
    public void PlayOne(ShakeSize size, float time)
    {
        Play(size);
        shakeTime = time;
    }

    /// <summary>
    /// �i�v�V�F�C�N
    /// </summary>
    /// <param name="size"></param>
    public void Play(ShakeSize size)
    {
        if (state == ShakeState.Idle)
        {
            shakeRot = 0f;
        }

        state = ShakeState.Active;
        shakeTime = -1f;
        shakeWidth.Set(CalcShakeWidth(size));
    }

    /// <summary>
    /// �~�߂�
    /// </summary>
    public void Stop()
    {
        if (state != ShakeState.Active) return;

        state = ShakeState.Fadeout;
        shakeTime = -1f;
        shakeWidth.MoveTo(0f, 0.5f, DeltaFloat.MoveType.LINE);
    }

    /// <summary>
    /// �t���[������
    /// </summary>
    public void Update()
    {
        // 
        shakeRot += Time.deltaTime * SHAKE_SPEED;
        if (shakeRot > Mathf.PI * 2f) shakeRot -= Mathf.PI * 2f;

        if (state == ShakeState.Active)
        {
            if (shakeTime >= 0f)
            {
                shakeTime -= Time.deltaTime;
                if (shakeTime < 0f)
                {
                    Stop();
                }
            }
        }
        else if (state == ShakeState.Fadeout)
        {
            shakeWidth.Update(Time.deltaTime);
            if (!shakeWidth.IsActive())
            {
                state = ShakeState.Idle;
            }
        }
    }

    /// <summary>
    /// �V�F�C�N��
    /// </summary>
    /// <returns></returns>
    public bool IsActive()
    {
        return state != ShakeState.Idle;
    }

    /// <summary>
    /// �V�F�C�N�l�擾
    /// </summary>
    /// <returns></returns>
    public float GetShakeValue()
    {
        return Mathf.Sin(shakeRot) * shakeWidth.Get();
    }

    /// <summary>
    /// �T�C�Y���畝����
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    private float CalcShakeWidth(ShakeSize size)
    {
        return size switch
        {
            ShakeSize.Weak => 10f,
            ShakeSize.Middle => 20f,
            _ => 30f
        };
    }
}
