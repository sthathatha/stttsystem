using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���Ŏ��R�ɓǂ߂�Scene
/// </summary>
public class SubScriptBase : MonoBehaviour
{
    #region �����o�[

    /// <summary>�X���[�v����Active=false����e�I�u�W�F�N�g</summary>
    public GameObject objectParent = null;

    /// <summary>�������I���</summary>
    private bool scriptInitEnd = false;

    #endregion

    #region ��������

    /// <summary>
    /// �J�n��
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {
        yield return InitStart();

        // �}�l�[�W���[�ɐݒ肵�ď������p�����[�^���Ă�ł��炤
        ManagerSceneScript.GetInstance().LoadedSubScene(this);
    }

    /// <summary>
    /// �p�����[�^������
    /// </summary>
    /// <param name="paramList"></param>
    public void InitParam(List<int> paramList)
    {
        StartCoroutine(InitCoroutineBase(paramList));
    }

    /// <summary>
    /// �������R���[�`��
    /// </summary>
    /// <param name="paramList"></param>
    /// <returns></returns>
    private IEnumerator InitCoroutineBase(List<int> paramList)
    {
        yield return InitCoroutine(paramList);
        scriptInitEnd = true;
    }

    /// <summary>
    /// �X�V
    /// </summary>
    private void Update()
    {
        // ���������I����Ă���Ă�
        if (scriptInitEnd) { UpdateSub(); }
    }

    #endregion

    #region �O����h���悩��Ăяo������

    /// <summary>
    /// �V�[���폜
    /// </summary>
    protected void DeleteScene()
    {
        ManagerSceneScript.GetInstance().DeleteSubScene(this);
    }

    #endregion

    #region �h���p

    /// <summary>
    /// ����������ԍŏ��iStart���j�̏�������
    /// </summary>
    /// <returns></returns>
    virtual protected IEnumerator InitStart()
    {
        yield break;
    }

    /// <summary>
    /// �h���p�������R���[�`��
    /// </summary>
    /// <param name="paramList"></param>
    /// <returns></returns>
    virtual protected IEnumerator InitCoroutine(List<int> paramList)
    {
        yield break;
    }

    /// <summary>
    /// �h���X�V�p
    /// </summary>
    virtual protected void UpdateSub() { }

    /// <summary>
    /// �Q�[���J�n�p�ɃX���[�v
    /// </summary>
    virtual public void Sleep()
    {
        objectParent?.SetActive(false);
    }

    #endregion
}
