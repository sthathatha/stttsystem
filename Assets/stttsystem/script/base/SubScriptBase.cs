using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ���Ŏ��R�ɓǂ߂�Scene
/// </summary>
public class SubScriptBase : MonoBehaviour
{
    #region �����o�[

    /// <summary>�X���[�v����Active=false����e�I�u�W�F�N�g</summary>
    public GameObject objectParent = null;

    #endregion

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
        StartCoroutine(InitCoroutine(paramList));
    }

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
    /// �V�[���폜
    /// </summary>
    protected void DeleteScene()
    {
        ManagerSceneScript.GetInstance().DeleteSubScene(this);
    }

    /// <summary>
    /// �Q�[���J�n�p�ɃX���[�v
    /// </summary>
    virtual public void Sleep()
    {
        objectParent?.SetActive(false);
    }
}
