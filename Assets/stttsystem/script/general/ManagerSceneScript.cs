using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 基本マネージャーScene
/// </summary>
public class ManagerSceneScript : MonoBehaviour
{
    /// <summary>デバッグ用</summary>
    public static bool isDebugLoad = false;

    #region 定数
    /// <summary>フェード時間</summary>
    private const float FADE_TIME = 0.5f;

    /// <summary>
    /// シーン状態
    /// </summary>
    public enum State : int
    {
        Loading = 0,
        Main,
        Game,
    }

    /// <summary>シーン状態</summary>
    public State SceneState { get; private set; }
    #endregion

    #region インスタンス
    /// <summary>インスタンス</summary>
    private static ManagerSceneScript _instance = null;
    /// <summary>インスタンス</summary>
    /// <returns></returns>
    public static ManagerSceneScript GetInstance() { return _instance; }

    #endregion

    #region メンバー

    #region UI

    /// <summary>フェーダ</summary>
    public CanvasGroup fader = null;

    #endregion

    /// <summary>システム外部</summary>
    public stttsystem system = null;

    /// <summary>基本シーン</summary>
    public void SetMainScript(MainScriptBase script) { mainScript = script; }
    private MainScriptBase mainScript = null;

    /// <summary>ゲームシーン</summary>
    public void SetGameScript(GameScriptBase script) { gameScript = script; }
    private GameScriptBase gameScript = null;

    /// <summary>サブシーン</summary>
    private List<SubScriptBase> subScriptList = null;
    /// <summary>サブシーン読み込み待ちパラメータ</summary>
    private List<SubSceneParam> subSceneParamList = null;

    /// <summary>サウンド管理</summary>
    public SoundManager soundManager = null;

    /// <summary>カメラ</summary>
    public GameObject mainCam = null;

    /// <summary>時間速度</summary>
    private float time_speed = 1f;

    #endregion

    #region 変数

    /// <summary>プレイヤー初期化位置</summary>
    protected int initId = 0;

    /// <summary>有効なdeltaTime</summary>
    public float validDeltaTime = 0f;

    #endregion

    #region 基底

    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        _instance = this;
        subScriptList = new List<SubScriptBase>();
        subSceneParamList = new List<SubSceneParam>();

        // メモリ減った時リソース削除
        Application.lowMemory += () =>
        {
            Resources.UnloadUnusedAssets();
        };

        // カメラ2Dか3D
        var cam = mainCam.GetComponent<Camera>();
        if (system.camera_is_2d)
        {
            mainCam.GetComponent<MainCamera2D>().enabled = true;
            mainCam.GetComponent<MainCamera3D>().enabled = false;
            cam.orthographic = true;
        }
        else
        {
            mainCam.GetComponent<MainCamera2D>().enabled = false;
            mainCam.GetComponent<MainCamera3D>().enabled = true;
            cam.orthographic = false;
            cam.fieldOfView = 45f;
        }

        GlobalData.GetSaveData().LoadSystemData();
        soundManager.UpdateBgmVolume();
        soundManager.UpdateSeVolume();
        soundManager.UpdateVoiceVolume();

        fader.gameObject.SetActive(true);
        fader.alpha = 1f;

        if (!isDebugLoad)
        {
            //初期シーンロード
            SceneManager.LoadSceneAsync(system.init_scene, LoadSceneMode.Additive);
        }
        else
        {
            //デバッグ時はセーブデータをロード
            GlobalData.GetSaveData().LoadGameData();
        }

        SceneState = State.Loading;
        yield return new WaitWhile(() => mainScript == null);

        // ゲーム起動時最初の処理
        InitMainScene();

        yield return mainScript.BeforeInitFadeIn();
        yield return mainScript.BeforeFadeIn();
        yield return FadeIn();
        yield return mainScript.AfterFadeIn(true);
        SceneState = State.Main;
    }

    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        // 時間更新
        if (time_speed <= 0f)
        {
            validDeltaTime = 0f;
        }
        else
        {
            var dt = Time.deltaTime;
            // 処理落ちで遅すぎた後
            if (dt > 0.3f) dt = 0.03f;
            // スピード考慮
            validDeltaTime = dt * time_speed;
        }

    }

    #endregion

    #region グローバルUI取得

    /// <summary>
    /// 2Dカメラ
    /// </summary>
    /// <returns></returns>
    public MainCamera2D GetCamera2D() { return mainCam.GetComponent<MainCamera2D>(); }

    /// <summary>
    /// 3Dカメラ
    /// </summary>
    /// <returns></returns>
    public MainCamera3D GetCamera3D() { return mainCam.GetComponent<MainCamera3D>(); }

    ///// <summary>ダイアログウィンドウ</summary>
    ///// <returns></returns>
    //public DialogWindow GetDialogWindow()
    //{
    //    return dialogWindow.GetComponent<DialogWindow>();
    //}

    ///// <summary>オプションウィンドウ</summary>
    ///// <returns></returns>
    //public OptionWindow GetOptionWindow()
    //{
    //    return optionWindow.GetComponent<OptionWindow>();
    //}

    #endregion

    #region フェード管理
    /// <summary>
    /// フェードアウト
    /// </summary>
    /// <param name="time">フェード時間　未指定でデフォルト</param>
    /// <param name="color">色指定　未指定で黒</param>
    /// <returns></returns>
    public IEnumerator FadeOut(float time = -1f, Color? color = null)
    {
        var col = color ?? Color.black;
        fader.GetComponentInChildren<Image>().color = col;

        var fadeTime = time > 0f ? time : FADE_TIME;
        fader.gameObject.SetActive(true);
        var alpha = new DeltaFloat();
        alpha.Set(0);
        alpha.MoveTo(1f, fadeTime, DeltaFloat.MoveType.LINE);
        while (alpha.IsActive())
        {
            alpha.Update(Time.deltaTime);
            fader.alpha = alpha.Get();
            yield return null;
        }
    }

    /// <summary>
    /// フェードイン
    /// </summary>
    /// <param name="time">フェード時間　未指定でデフォルト</param>
    /// <returns></returns>
    public IEnumerator FadeIn(float time = -1f)
    {
        var fadeTime = time > 0f ? time : FADE_TIME;
        var alpha = new DeltaFloat();
        alpha.Set(1f);
        alpha.MoveTo(0f, fadeTime, DeltaFloat.MoveType.LINE);

        // Startで表示初期化しているが、直後フェードイン始まると一瞬見えるので1フレ待つ
        fader.alpha = alpha.Get();
        yield return null;

        while (alpha.IsActive())
        {
            alpha.Update(Time.deltaTime);
            fader.alpha = alpha.Get();
            yield return null;
        }
        fader.alpha = 0f;
        fader.gameObject.SetActive(false);
    }

    /// <summary>
    /// 瞬時に暗くする
    /// </summary>
    /// <param name="color">色指定　未指定で黒</param>
    public void FadeOutNoWait(Color? color = null)
    {
        var col = color ?? Color.black;
        fader.GetComponentInChildren<Image>().color = col;

        fader.alpha = 1f;
        fader.gameObject.SetActive(true);
    }

    /// <summary>
    /// 瞬時に明るくする
    /// </summary>
    public void FadeInNoWait()
    {
        fader.alpha = 0f;
        fader.gameObject.SetActive(false);
    }
    #endregion

    #region シーン管理
    /// <summary>
    /// メインシーン切り替え
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="id">プレイヤー位置</param>
    /// <param name="subSceneClear">サブシーン全部消す</param>
    public void LoadMainScene(string sceneName, int id, bool subSceneClear = true)
    {
        StartCoroutine(LoadMainSceneCoroutine(sceneName, id, subSceneClear));
    }

    /// <summary>
    /// 初期化位置取得
    /// </summary>
    /// <returns></returns>
    public int GetInitId() { return initId; }

    /// <summary>
    /// メインシーン切り替えコルーチン
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="id"></param>
    /// <param name="subSceneClear">サブシーン全部消す</param>
    /// <returns></returns>
    private IEnumerator LoadMainSceneCoroutine(string sceneName, int id, bool subSceneClear = true)
    {
        // フェードアウト
        SceneState = State.Loading;
        yield return FadeOut();
        initId = id;

        // サブシーン消す
        if (subSceneClear)
        {
            DeleteSubSceneAll();
            yield return new WaitWhile(() => IsLoadingSubScene());
        }

        // 旧シーンを保持してロード開始
        var oldScript = mainScript;
        if (oldScript != null)
        {
            // オブジェクトが重なると誤作動するので停止だけする
            oldScript.Sleep();
        }

        mainScript = null;
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => mainScript != null);

        // BGMデータ取得
        var bgmData = mainScript.GetBgm();
        // 変更の必要がある場合フェードアウトを待つ
        if (soundManager.IsNeedChangeMainBgm(bgmData.Item1))
        {
            yield return soundManager.FadeOutMainBgm(false);
        }

        // 旧シーンがある場合アンロード
        if (oldScript != null)
        {
            SceneManager.UnloadSceneAsync(oldScript.gameObject.scene);
        }

        // 初期処理
        InitMainScene(id);
        // フェードイン前の処理
        yield return mainScript.BeforeInitFadeIn();
        yield return mainScript.BeforeFadeIn();
        // フェードイン
        yield return FadeIn();
        // フェードイン後の処理
        yield return mainScript.AfterFadeIn(true);

        SceneState = State.Main;
    }

    /// <summary>
    /// メインシーン読み込み後、フェードアウト直前にやるべきこと
    /// </summary>
    private void InitMainScene(int id = -1)
    {
        // BGM開始
        var bgmData = mainScript.GetBgm();
        soundManager.PlayMainBgm(bgmData.Item1, bgmData.Item2);
    }

    #endregion

    #region サブシーン管理

    private static object _subSceneLock = new();
    /// <summary>
    /// ロック実行
    /// </summary>
    /// <param name="act"></param>
    private void SubSceneLockAct(Action act)
    {
        lock (_subSceneLock)
        {
            act.Invoke();
        }
    }

    /// <summary>
    /// サブシーンのリスト取得
    /// </summary>
    /// <returns></returns>
    public List<SubScriptBase> GetSubSceneList() { return subScriptList; }

    /// <summary>ロード中サブシーンのパラメータリスト取得</summary>
    /// <returns></returns>
    public List<SubSceneParam> GetSubSceneLoadingList() { return subSceneParamList; }

    /// <summary>
    /// サブシーンロード
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="prms">パラメータint群</param>
    public void LoadSubScene(string sceneName, params int[] prms)
    {
        SubSceneLockAct(() =>
        {
            subSceneParamList.Add(new SubSceneParam(sceneName, prms));
        });
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    /// <summary>
    /// サブシーンロード終了時に呼び出す
    /// </summary>
    /// <param name="scr"></param>
    public void LoadedSubScene(SubScriptBase scr)
    {
        SubSceneLockAct(() =>
        {
            for (var i = 0; i < subSceneParamList.Count; i++)
            {
                // 同名シーンを複数呼ぶとタイミングによりパラメータが変わる可能性があるが
                // 同名なら入れ替わっても問題ないだろう
                if (scr.gameObject.scene.name != subSceneParamList[i].sceneName) continue;

                // パラメータ渡して初期化
                scr.InitParam(subSceneParamList[i].prmList);

                subSceneParamList.RemoveAt(i);
                break;
            }

            subScriptList.Add(scr);
        });
    }

    /// <summary>
    /// サブシーン読み込みパラメータクラス
    /// </summary>
    public class SubSceneParam
    {
        public string sceneName;
        public List<int> prmList;

        public SubSceneParam(string name, params int[] prms)
        {
            sceneName = name;
            prmList = new List<int>();
            prmList.AddRange(prms);
        }
    }

    /// <summary>
    /// サブシーンロード中
    /// </summary>
    /// <returns></returns>
    public bool IsLoadingSubScene() { return subSceneParamList.Any(); }

    /// <summary>
    /// サブシーン削除
    /// </summary>
    /// <param name="subscr"></param>
    public void DeleteSubScene(SubScriptBase subscr)
    {
        SubSceneLockAct(() =>
        {
            subScriptList.Remove(subscr);
            SceneManager.UnloadSceneAsync(subscr.gameObject.scene);
        });
    }

    /// <summary>
    /// サブシーン全削除
    /// </summary>
    public void DeleteSubSceneAll()
    {
        StartCoroutine(DeleteSubSceneAllCoroutine());
    }

    /// <summary>
    /// サブシーン全削除コルーチン
    /// </summary>
    private IEnumerator DeleteSubSceneAllCoroutine()
    {
        if (IsLoadingSubScene())
        {
            // ロード中のがあったら今あるのを一旦スリープして待つ
            foreach (var subscr in subScriptList)
            {
                subscr.Sleep();
            }

            yield return new WaitWhile(() => subSceneParamList.Any());
        }

        // 全部消し
        SubSceneLockAct(() =>
        {
            foreach (var subscr in subScriptList)
            {
                SceneManager.UnloadSceneAsync(subscr.gameObject.scene);
            }
            subScriptList.Clear();
        });
    }

    #endregion

    #region 時間管理

    /// <summary>
    /// 時間の速度を設定
    /// </summary>
    /// <param name="speed"></param>
    public void SetTimeSpeed(float speed)
    {
        time_speed = speed;
    }

    #endregion
}
