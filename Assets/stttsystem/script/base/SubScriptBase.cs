using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 裏で自由に読めるScene
/// </summary>
public class SubScriptBase : MonoBehaviour
{
    /// <summary>
    /// 開始時
    /// </summary>
    /// <returns></returns>
    private void Start()
    {
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
}
