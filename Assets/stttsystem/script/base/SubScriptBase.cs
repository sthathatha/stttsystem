using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 裏で自由に読めるScene
/// </summary>
public class SubScriptBase : MonoBehaviour
{
    #region メンバー

    /// <summary>スリープ時にActive=falseする親オブジェクト</summary>
    public GameObject objectParent = null;

    #endregion

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
        StartCoroutine(InitCoroutine(paramList));
    }

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
    /// シーン削除
    /// </summary>
    protected void DeleteScene()
    {
        ManagerSceneScript.GetInstance().DeleteSubScene(this);
    }

    /// <summary>
    /// ゲーム開始用にスリープ
    /// </summary>
    virtual public void Sleep()
    {
        objectParent?.SetActive(false);
    }
}
