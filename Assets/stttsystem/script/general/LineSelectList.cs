using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���j���[�^���`���X�g
/// </summary>
public class LineSelectList<T>
{
    #region �����o�[

    /// <summary>���X�g�{��</summary>
    protected List<T> list;

    /// <summary>�I�𒆃C���f�b�N�X</summary>
    public int selectIndex { get; protected set; }

    #endregion

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    public LineSelectList()
    {
        list = new List<T>();
        selectIndex = 0;
    }

    /// <summary>
    /// �A�C�e���ǉ�
    /// </summary>
    /// <param name="items"></param>
    public void AddItem(params T[] items)
    {
        list.AddRange(items);
    }

    #region ����

    /// <summary>
    /// ���ɓ���
    /// </summary>
    virtual public void MoveNext()
    {
        if (list.Count == 0) return;

        selectIndex++;
        if (selectIndex >= list.Count)
        {
            selectIndex = 0;
        }
    }

    /// <summary>
    /// �O�ɓ���
    /// </summary>
    virtual public void MoveBefore()
    {
        if (list.Count == 0) return;

        selectIndex--;
        if (selectIndex < 0)
        {
            selectIndex = list.Count - 1;
        }
    }

    #endregion

    #region �擾

    /// <summary>
    /// �A�C�e����
    /// </summary>
    /// <returns></returns>
    public int GetAllCount() { return list.Count; }

    /// <summary>
    /// �A�C�e���擾
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T GetItem(int index)
    {
        if (index < 0 || index >= list.Count) return default;
        return list[index];
    }

    /// <summary>
    /// �I�𒆃A�C�e���擾
    /// </summary>
    /// <returns></returns>
    public T GetSelectItem()
    {
        return GetItem(selectIndex);
    }

    #endregion
}
