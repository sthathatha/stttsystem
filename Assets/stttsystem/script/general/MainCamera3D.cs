using Codice.CM.Triggers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラクラス
/// </summary>
public class MainCamera3D : MonoBehaviour
{
    #region 定数

    /// <summary>処理落ちした時deltaTimeが爆発するので上限</summary>
    private const float DELTATIME_LIMIT = 0.06f;

    /// <summary>カメラ回転速度</summary>
    private const float ROT_SPEED = 70f;

    /// <summary>カメラ距離速度</summary>
    private const float DIST_SPEED = 20f;

    /// <summary>
    /// 処理落ちした時deltaTimeが爆発するので上限
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

    #region メンバー

    /// <summary>カメラ距離最大</summary>
    public float dist_max { get; set; } = 10f;
    /// <summary>カメラ距離最小</summary>
    public float dist_min { get; set; } = 3f;

    /// <summary>カメラ移動上の限界</summary>
    public float rot_up_limit { get; set; } = Mathf.PI * 0.3f;
    /// <summary>カメラ移動下の限界</summary>
    public float rot_down_limit { get; set; } = -Mathf.PI * 0.05f;

    #endregion

    #region 変数

    /// <summary>視点</summary>
    private DeltaVector3 targetPos;

    /// <summary>向き左右</summary>
    private DeltaFloat rotLR;
    /// <summary>向き上下</summary>
    private DeltaFloat rotUD;
    /// <summary>距離</summary>
    private DeltaFloat distance;

    /// <summary>シェイク管理</summary>
    private Shaker shaker;

    #endregion

    #region 基底

    /// <summary>
    /// 初期化
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
    /// フレーム処理
    /// </summary>
    void Update()
    {
        // 移動中のものがあれば更新
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

    #region 位置管理

    /// <summary>
    /// 現在のパラメータで位置と向きを計算
    /// </summary>
    private void UpdateCamera()
    {
        // 左右角度は0～2πに
        if (!rotLR.IsActive())
            rotLR.Set(Util.GetNormalRadian(rotLR.Get()));

        // 回転を計算
        var quatUD = Quaternion.Euler(rotUD.Get(), 0, 0);
        var quatLR = Quaternion.Euler(0, rotLR.Get(), 0);
        var quat = quatLR * quatUD;
        transform.rotation = quat;

        // 位置を計算
        // 距離分のベクトル
        var distVec = quat * new Vector3(0, 0, -distance.Get());
        transform.position = targetPos.Get() - distVec;
    }

    /// <summary>
    /// 位置設定
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
    /// 位置設定
    /// </summary>
    /// <param name="_object"></param>
    public void SetTargetPos(GameObject _object, float time = -1f)
    {
        SetTargetPos(_object.transform.position, time);
    }

    /// <summary>
    /// カメラ距離変更
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
    /// カメラ距離を時間で固定値にする
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
    /// 左右回転
    /// </summary>
    /// <param name="rate">1で右、-1で左、数値で速度</param>
    public void SetRotateLR(float rate)
    {
        if (rotLR.IsActive()) return;

        rotLR.Set(rotLR.Get() + rate * MaxDelta);
        UpdateCamera();
    }

    /// <summary>
    /// 上下回転
    /// </summary>
    /// <param name="rate">1で上、-1で下、数値で速度</param>
    public void SetRotateUD(float rate)
    {
        if (rotUD.IsActive()) return;

        var newUD = rotUD.Get() + rate * MaxDelta * ROT_SPEED;

        rotUD.Set(Util.GetClampF(newUD, rot_down_limit, rot_up_limit));
        UpdateCamera();
    }
    /// <summary>
    /// 回転を時間で固定値にする
    /// </summary>
    /// <param name="vec">向き</param>
    /// <param name="time">時間</param>
    public void SetRotateTime(Vector3 vec, float time = -1f)
    {
        // 左右の計算
        var noYVec = new Vector3(vec.x, 0f, vec.z).normalized;
        var lr = Mathf.Atan2(noYVec.z, noYVec.x);
        // 左右は移動距離が短い方を選択
        if (lr - rotLR.Get() > Mathf.PI)
            lr -= Mathf.PI * 2f;
        else if (rotLR.Get() - lr > Mathf.PI)
            lr += Mathf.PI * 2f;

        // 上下の計算
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

    #region シェイク

    /// <summary>
    /// シェイク1回
    /// </summary>
    /// <param name="size"></param>
    /// <param name="time"></param>
    public void PlayShakeOne(Shaker.ShakeSize size, float time = 1f)
    {
        shaker.PlayOne(size, time);
    }
    /// <summary>
    /// シェイク永久
    /// </summary>
    /// <param name="size"></param>
    public void PlayShake(Shaker.ShakeSize size)
    {
        shaker.Play(size);
    }
    /// <summary>
    /// シェイク停止
    /// </summary>
    public void StopShake()
    {
        shaker.Stop();
    }

    #endregion
}
