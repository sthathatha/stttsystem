using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScriptBase : MonoBehaviour
{
    #region �����o�[

    /// <summary>�X���[�v����Active=false����e�I�u�W�F�N�g</summary>
    public GameObject objectParent = null;

    /// <summary>BGM</summary>
    public AudioClip bgmClip = null;

    #endregion

    #region �ϐ�

    #endregion

    #region ���

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public MainScriptBase()
    {
    }

    /// <summary>
    /// �J�n��
    /// </summary>
    /// <returns></returns>
    virtual protected IEnumerator Start()
    {
        // ���ڋN�����Ƀ}�l�[�W���N��
        if (!ManagerSceneScript.GetInstance())
        {
            ManagerSceneScript.isDebugLoad = true;
            GlobalData.GetSaveData().LoadGameData();
            SceneManager.LoadScene("_ManagerScene", LoadSceneMode.Additive);
            yield return null;
        }

        ManagerSceneScript.GetInstance().SetMainScript(this);
    }

    #endregion

    #region �p�u���b�N���\�b�h

    /// <summary>
    /// �Q�[���J�n�p�ɃX���[�v
    /// </summary>
    virtual public void Sleep()
    {
        objectParent?.SetActive(false);
    }

    /// <summary>
    /// �Q�[���I�����ɍĊJ
    /// </summary>
    virtual public void AwakeFromGame()
    {
        objectParent?.SetActive(true);

        if (!ManagerSceneScript.GetInstance()) return;
    }

    /// <summary>
    /// ���[�h��ŏ��̂P��
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator BeforeInitFadeIn()
    {
        yield break;
    }

    /// <summary>
    /// �t�F�[�h�C�����O�ɂ�邱��
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator BeforeFadeIn()
    {
        yield break;
    }

    /// <summary>
    /// �t�F�[�h�C���I��������邱��
    /// </summary>
    /// <returns></returns>
    virtual public IEnumerator AfterFadeIn(bool init)
    {
        yield break;
    }

    /// <summary>
    /// BGM�ݒ�@���ꏈ���̏ꍇ�̓I�[�o�[���C�h
    /// </summary>
    /// <returns></returns>
    virtual public Tuple<SoundManager.MainBgmType, AudioClip> GetBgm()
    {
        return new Tuple<SoundManager.MainBgmType, AudioClip>(SoundManager.MainBgmType.Clip, bgmClip);
    }

    #endregion
}
