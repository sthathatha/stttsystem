using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// �ėp�@�\
/// </summary>
public partial class Util
{
    /// <summary>
    /// �T�C���J�[�u��float�ɕϊ�
    /// </summary>
    /// <param name="_val">0�`1</param>
    /// <param name="_type">�������I��</param>
    /// <returns>0�`1</returns>
    public static float SinCurve(float _val, Constant.SinCurveType _type)
    {
        float theta;
        switch (_type)
        {
            case Constant.SinCurveType.Accel:
                theta = Mathf.PI * (_val / 2f - 0.5f);
                return Mathf.Sin(theta) + 1f;
            case Constant.SinCurveType.Decel:
                theta = Mathf.PI * (_val / 2f);
                return Mathf.Sin(theta);
            case Constant.SinCurveType.Both:
                theta = Mathf.PI * (_val - 0.5f);
                return (Mathf.Sin(theta) + 1f) / 2f;
        }

        return 0f;
    }

    /// <summary>
    /// ��Ԓl
    /// </summary>
    /// <param name="_rate"></param>
    /// <param name="_val1"></param>
    /// <param name="_val2"></param>
    /// <returns></returns>
    public static float CalcBetweenFloat(float _rate, float _val1, float _val2)
    {
        return _val1 + (_val2 - _val1) * _rate;
    }

    /// <summary>
    /// �����_������ max�������̉\������
    /// </summary>
    /// <param name="min">�Œ�l</param>
    /// <param name="max">�ő�l</param>
    /// <returns></returns>
    public static int RandomInt(int min, int max)
    {
        return Random.Range(min, max + 1);
    }

    /// <summary>
    /// �����_������
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float RandomFloat(float min, float max)
    {
        return Random.Range(min, max);
    }

    /// <summary>
    /// �����_�������@�T�C���J�[�u�Œ����t�߂��o�₷��
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float RandomFloatSin(float min, float max)
    {
        var rad = RandomFloat(-Mathf.PI / 2f, Mathf.PI * 1.5f);
        var rate = (Mathf.Sin(rad) + 1f) / 2f; // 0�`1

        return min + (max - min) * rate;
    }

    /// <summary>
    /// �d�����Ȃ������_�������i���т������_���j
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="count">�� ��������Ƒ���Ȃ��Ȃ�̂ŋ֎~</param>
    /// <returns></returns>
    public static List<int> RandomUniqueIntList(int min, int max, int count)
    {
        if (max - min + 1 < count) { throw new System.Exception("RandomUniqueIntList��count��������"); }

        var list = new List<int>();
        for (int i = min; i <= max; ++i)
        {
            list.Add(i);
        }

        var ret = new List<int>();
        for (int i = 0; i < count; ++i)
        {
            var rand = RandomInt(0, list.Count - 1);
            ret.Add(list[rand]);
            list.RemoveAt(rand);
        }

        return ret;
    }

    /// <summary>
    /// �m�����X�g��������̃C���f�b�N�X��I������
    /// </summary>
    /// <param name="rateList"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static List<int> RandomIndexList(List<int> rateList, int count)
    {
        var ret = new List<int>();
        if (count <= 0) return ret;
        if (count >= rateList.Count)
        {
            return RandomUniqueIntList(0, rateList.Count - 1, rateList.Count);
        }

        var rateMax = rateList.Sum() - 1;
        for (var i = 0; i < count; ++i)
        {
            var rand = RandomInt(0, rateMax);

            for (var idx = 0; idx < rateList.Count; ++idx)
            {
                if (ret.Contains(idx)) continue;

                rand -= rateList[idx];
                if (rand < 0)
                {
                    ret.Add(idx);
                    rateMax -= rateList[idx];
                    break;
                }
            }
        }

        return ret;
    }

    /// <summary>
    /// �����_������
    /// </summary>
    /// <param name="percent">�p�[�Z���g��true�ɂȂ�</param>
    /// <returns></returns>
    public static bool RandomCheck(int percent)
    {
        return RandomInt(0, 99) < percent;
    }

    /// <summary>
    /// �x�N�g����Direction�ɕϊ�
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public static Constant.Direction2D Get2DDirectionFromVec(Vector3 pos)
    {
        if (Mathf.Abs(pos.x) > Mathf.Abs(pos.y))
        {
            if (pos.x < 0)
                return Constant.Direction2D.Left;
            else
                return Constant.Direction2D.Right;
        }
        else
        {
            if (pos.y < 0)
                return Constant.Direction2D.Down;
            else
                return Constant.Direction2D.Up;
        }
    }

    /// <summary>
    /// Direction�̒P�ʃx�N�g��
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    public static Vector3 Get2DVector3FromDirection(Constant.Direction2D dir)
    {
        return dir switch
        {
            Constant.Direction2D.Right => new Vector3(1, 0),
            Constant.Direction2D.Up => new Vector3(0, 1),
            Constant.Direction2D.Down => new Vector3(0, -1),
            _ => new Vector3(-1, 0),
        };
    }

    /// <summary>
    /// Z����]�N�I�[�^�j�I���쐬�i�����v���j
    /// </summary>
    /// <param name="radian"></param>
    /// <returns></returns>
    public static Quaternion Get2DRotateQuaternion(float radian)
    {
        return Quaternion.Euler(0, 0, Mathf.Rad2Deg * radian);
    }

    /// <summary>
    /// ��]�p�̒P�ʃx�N�g��
    /// </summary>
    /// <param name="radian"></param>
    /// <returns></returns>
    public static Vector3 Get2DVector3IdentityFromRot(float radian)
    {
        return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    /// <summary>
    /// �x�N�g�������]�p���W�A�����Z�o
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static float Get2DRadianFromVector(Vector3 v)
    {
        v = v.normalized;
        return Mathf.Atan2(v.y, v.x);
    }

    /// <summary>
    /// 0�`2�΂̊Ԃɐ��K��
    /// </summary>
    /// <param name="rad"></param>
    /// <returns></returns>
    public static float GetNormalRadian(float rad)
    {
        if (rad >= 0f && rad < Mathf.PI * 2f)
        {
            return rad;
        }

        var cnt = Mathf.FloorToInt(rad / (Mathf.PI * 2f));
        return rad - cnt * Mathf.PI * 2f;
    }

    /// <summary>
    /// min��max�ɉ������߂��l
    /// </summary>
    /// <param name="val"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float GetClampF(float val, float min, float max)
    {
        if (val > max) return max;
        if (val < min) return min;
        return val;
    }
}
