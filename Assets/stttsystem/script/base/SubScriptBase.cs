using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 裏で自由に読めるScene
/// </summary>
public class SubScriptBase : MonoBehaviour
{
    #region メンバー

    /// <summary>スリープ時にActive=falseする親オブジェクト</summary>
    public GameObject objectParent = null;

    /// <summary>初期化終わり</summary>
    private bool scriptInitEnd = false;

    #endregion

    #region 内部処理

    /// <summary>
    /// 開始時
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {
        yield return InitStart();

        // マネージャーに設定して初期化パラメータを呼んでもらう
        ManagerSceneScript.GetInstance().LoadedSubScene(this);
    }

    /// <summary>
    /// パラメータ初期化
    /// </summary>
    /// <param name="paramList"></param>
    public void InitParam(List<int> paramList)
    {
        StartCoroutine(InitCoroutineBase(paramList));
    }

    /// <summary>
    /// 初期化コルーチン
    /// </summary>
    /// <param name="paramList"></param>
    /// <returns></returns>
    private IEnumerator InitCoroutineBase(List<int> paramList)
    {
        yield return InitCoroutine(paramList);
        scriptInitEnd = true;
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        // 初期化が終わってから呼ぶ
        if (scriptInitEnd) { UpdateSub(); }
    }

    #endregion

    #region 外部や派生先から呼び出す処理

    /// <summary>
    /// シーン削除
    /// </summary>
    protected void DeleteScene()
    {
        ManagerSceneScript.GetInstance().DeleteSubScene(this);
    }

    #endregion

    #region 派生用

    /// <summary>
    /// 生成した一番最初（Start時）の初期処理
    /// </summary>
    /// <returns></returns>
    virtual protected IEnumerator InitStart()
    {
        yield break;
    }

    /// <summary>
    /// 派生用初期化コルーチン
    /// </summary>
    /// <param name="paramList"></param>
    /// <returns></returns>
    virtual protected IEnumerator InitCoroutine(List<int> paramList)
    {
        yield break;
    }

    /// <summary>
    /// 派生更新用
    /// </summary>
    virtual protected void UpdateSub() { }

    /// <summary>
    /// ゲーム開始用にスリープ
    /// </summary>
    virtual public void Sleep()
    {
        objectParent?.SetActive(false);
    }

    #endregion
}
