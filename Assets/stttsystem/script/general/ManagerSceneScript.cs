using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

/// <summary>
/// ��{�}�l�[�W���[Scene
/// </summary>
public class ManagerSceneScript : MonoBehaviour
{
    /// <summary>�f�o�b�O�p</summary>
    public static bool isDebugLoad = false;

    #region �萔
    /// <summary>�t�F�[�h����</summary>
    private const float FADE_TIME = 0.5f;

    /// <summary>
    /// �V�[�����
    /// </summary>
    public enum State : int
    {
        Loading = 0,
        Main,
        Game,
    }

    /// <summary>�V�[�����</summary>
    public State SceneState { get; private set; }
    #endregion

    #region �C���X�^���X
    /// <summary>�C���X�^���X</summary>
    private static ManagerSceneScript _instance = null;
    /// <summary>�C���X�^���X</summary>
    /// <returns></returns>
    public static ManagerSceneScript GetInstance() { return _instance; }

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public ManagerSceneScript()
    {
        _instance = this;
    }
    #endregion

    #region �����o�[

    #region UI

    /// <summary>�t�F�[�_</summary>
    public CanvasGroup fader = null;

    #endregion

    /// <summary>�V�X�e���O��</summary>
    public stttsystem system = null;

    /// <summary>��{�V�[��</summary>
    public void SetMainScript(MainScriptBase script) { mainScript = script; }
    private MainScriptBase mainScript = null;

    /// <summary>�Q�[���V�[��</summary>
    public void SetGameScript(GameScriptBase script) { gameScript = script; }
    private GameScriptBase gameScript = null;

    /// <summary>�T�E���h�Ǘ�</summary>
    public SoundManager soundManager = null;

    /// <summary>�J����</summary>
    public MainCamera mainCam = null;

    #endregion

    #region �ϐ�

    /// <summary>�v���C���[�������ʒu</summary>
    protected int initId = 0;

    #endregion

    #region ������
    /// <summary>
    /// ������
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        GlobalData.GetSaveData().LoadSystemData();
        soundManager.UpdateBgmVolume();
        soundManager.UpdateSeVolume();
        soundManager.UpdateVoiceVolume();

        fader.gameObject.SetActive(true);
        fader.alpha = 1f;

        if (!isDebugLoad)
        {
            //�����V�[�����[�h
            SceneManager.LoadSceneAsync(system.init_scene, LoadSceneMode.Additive);
        }
        else
        {
            //�f�o�b�O���̓Z�[�u�f�[�^�����[�h
            GlobalData.GetSaveData().LoadGameData();
        }

        SceneState = State.Loading;
        yield return new WaitWhile(() => mainScript == null);

        // �Q�[���N�����ŏ��̏���
        InitMainScene();

        yield return mainScript.BeforeInitFadeIn();
        yield return mainScript.BeforeFadeIn();
        yield return FadeIn();
        yield return mainScript.AfterFadeIn(true);
        SceneState = State.Main;
    }
    #endregion

    #region �O���[�o��UI�擾

    ///// <summary>�_�C�A���O�E�B���h�E</summary>
    ///// <returns></returns>
    //public DialogWindow GetDialogWindow()
    //{
    //    return dialogWindow.GetComponent<DialogWindow>();
    //}

    ///// <summary>�I�v�V�����E�B���h�E</summary>
    ///// <returns></returns>
    //public OptionWindow GetOptionWindow()
    //{
    //    return optionWindow.GetComponent<OptionWindow>();
    //}

    #endregion

    #region �t�F�[�h�Ǘ�
    /// <summary>
    /// �t�F�[�h�A�E�g
    /// </summary>
    /// <param name="time">�t�F�[�h���ԁ@���w��Ńf�t�H���g</param>
    /// <param name="color">�F�w��@���w��ō�</param>
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
    /// �t�F�[�h�C��
    /// </summary>
    /// <param name="time">�t�F�[�h���ԁ@���w��Ńf�t�H���g</param>
    /// <returns></returns>
    public IEnumerator FadeIn(float time = -1f)
    {
        var fadeTime = time > 0f ? time : FADE_TIME;
        var alpha = new DeltaFloat();
        alpha.Set(1f);
        alpha.MoveTo(0f, fadeTime, DeltaFloat.MoveType.LINE);

        // Start�ŕ\�����������Ă��邪�A����t�F�[�h�C���n�܂�ƈ�u������̂�1�t���҂�
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
    /// �u���ɈÂ�����
    /// </summary>
    /// <param name="color">�F�w��@���w��ō�</param>
    public void FadeOutNoWait(Color? color = null)
    {
        var col = color ?? Color.black;
        fader.GetComponentInChildren<Image>().color = col;

        fader.alpha = 1f;
        fader.gameObject.SetActive(true);
    }

    /// <summary>
    /// �u���ɖ��邭����
    /// </summary>
    public void FadeInNoWait()
    {
        fader.alpha = 0f;
        fader.gameObject.SetActive(false);
    }
    #endregion

    #region �V�[���Ǘ�
    /// <summary>
    /// ���C���V�[���؂�ւ�
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="id">�v���C���[�ʒu</param>
    public void LoadMainScene(string sceneName, int id)
    {
        StartCoroutine(LoadMainSceneCoroutine(sceneName, id));
    }

    /// <summary>
    /// �������ʒu�擾
    /// </summary>
    /// <returns></returns>
    public int GetInitId() { return initId; }

    /// <summary>
    /// ���C���V�[���؂�ւ��R���[�`��
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    private IEnumerator LoadMainSceneCoroutine(string sceneName, int id)
    {
        // �t�F�[�h�A�E�g
        SceneState = State.Loading;
        yield return FadeOut();
        initId = id;

        // ���V�[����ێ����ă��[�h�J�n
        var oldScript = mainScript;
        if (oldScript != null)
        {
            // �I�u�W�F�N�g���d�Ȃ�ƌ�쓮����̂Œ�~��������
            oldScript.Sleep();
        }

        mainScript = null;
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => mainScript != null);

        // BGM�f�[�^�擾
        var bgmData = mainScript.GetBgm();
        // �ύX�̕K�v������ꍇ�t�F�[�h�A�E�g��҂�
        if (soundManager.IsNeedChangeMainBgm(bgmData.Item1))
        {
            yield return soundManager.FadeOutMainBgm(false);
        }

        // ���V�[��������ꍇ�A�����[�h
        if (oldScript != null)
        {
            SceneManager.UnloadSceneAsync(oldScript.gameObject.scene);
        }

        // ��������
        InitMainScene(id);
        // �t�F�[�h�C���O�̏���
        yield return mainScript.BeforeInitFadeIn();
        yield return mainScript.BeforeFadeIn();
        // �t�F�[�h�C��
        yield return FadeIn();
        // �t�F�[�h�C����̏���
        yield return mainScript.AfterFadeIn(true);

        SceneState = State.Main;
    }

    /// <summary>
    /// ���C���V�[���ǂݍ��݌�A�t�F�[�h�A�E�g���O�ɂ��ׂ�����
    /// </summary>
    private void InitMainScene(int id = -1)
    {
        // BGM�J�n
        var bgmData = mainScript.GetBgm();
        soundManager.PlayMainBgm(bgmData.Item1, bgmData.Item2);
    }

    #endregion
}