using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 汎用機能
/// </summary>
public partial class Util
{
    /// <summary>
    /// サインカーブのfloatに変換
    /// </summary>
    /// <param name="_val">0～1</param>
    /// <param name="_type">加減速選択</param>
    /// <returns>0～1</returns>
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
    /// 補間値
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
    /// ランダム整数 maxも発生の可能性あり
    /// </summary>
    /// <param name="min">最低値</param>
    /// <param name="max">最大値</param>
    /// <returns></returns>
    public static int RandomInt(int min, int max)
    {
        return Random.Range(min, max + 1);
    }

    /// <summary>
    /// ランダム小数
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float RandomFloat(float min, float max)
    {
        return Random.Range(min, max);
    }

    /// <summary>
    /// ランダム小数　サインカーブで中央付近が出やすい
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float RandomFloatSin(float min, float max)
    {
        var rad = RandomFloat(-Mathf.PI / 2f, Mathf.PI * 1.5f);
        var rate = (Mathf.Sin(rad) + 1f) / 2f; // 0～1

        return min + (max - min) * rate;
    }

    /// <summary>
    /// 重複しないランダム整数（並びもランダム）
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="count">個数 多すぎると足りなくなるので禁止</param>
    /// <returns></returns>
    public static List<int> RandomUniqueIntList(int min, int max, int count)
    {
        if (max - min + 1 < count) { throw new System.Exception("RandomUniqueIntListのcountが多すぎ"); }

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
    /// 確率リストから個数分のインデックスを選択する
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
    /// ランダム判定
    /// </summary>
    /// <param name="percent">パーセントでtrueになる</param>
    /// <returns></returns>
    public static bool RandomCheck(int percent)
    {
        return RandomInt(0, 99) < percent;
    }

    /// <summary>
    /// ベクトルをDirectionに変換
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
    /// Directionの単位ベクトル
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
    /// Z軸回転クオータニオン作成（反時計回り）
    /// </summary>
    /// <param name="radian"></param>
    /// <returns></returns>
    public static Quaternion Get2DRotateQuaternion(float radian)
    {
        return Quaternion.Euler(0, 0, Mathf.Rad2Deg * radian);
    }

    /// <summary>
    /// 回転角の単位ベクトル
    /// </summary>
    /// <param name="radian"></param>
    /// <returns></returns>
    public static Vector3 Get2DVector3IdentityFromRot(float radian)
    {
        return new Vector3(Mathf.Cos(radian), Mathf.Sin(radian));
    }

    /// <summary>
    /// ベクトルから回転角ラジアンを算出
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static float Get2DRadianFromVector(Vector3 v)
    {
        v = v.normalized;
        return Mathf.Atan2(v.y, v.x);
    }

    /// <summary>
    /// 0～2πの間に正規化
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
    /// minとmaxに押し込めた値
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
