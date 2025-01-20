using PlasticGui.Configuration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �y�[�W���O�@�\�����X�g
/// </summary>
public class LineSelectPageList<T> : LineSelectList<T>
{
    #region �����o�[

    /// <summary>�\���擪</summary>
    public int headIndex { get; private set; }
    /// <summary>1�y�[�W�̃T�C�Y</summary>
    private int page_max;
    /// <summary>true:�y�[�W�܂邲�Ɛ؂�ւ��@false:1�P�ʂŃX�N���[��</summary>
    private bool display_paging;

    #endregion

    /// <summary>
    /// �R���X�g���N�^
    /// </summary>
    /// <param name="pageMax">1�y�[�W�̃T�C�Y</param>
    /// <param name="pagingMode">true:�y�[�W�܂邲�Ɛ؂�ւ��@false:1�P�ʂŃX�N���[��</param>
    public LineSelectPageList(int pageMax, bool pagingMode = false) : base()
    {
        display_paging = pagingMode;
        page_max = pageMax;
        headIndex = 0;
    }

    #region ����

    /// <summary>
    /// ���ɓ���
    /// </summary>
    override public void MoveNext()
    {
        base.MoveNext();
        if (list.Count == 0) return;

        // �\���ő吔������ꍇhead���삪����
        // ��ԉ���Index
        var last = GetDisplayLastIndex();

        if (display_paging)
        {
            // �ۂ��Ƃ̏ꍇ�͉����߂����玟��
            if (last < selectIndex)
                headIndex = selectIndex;
        }
        else
        {
            // �P���Ƃ̏ꍇ��ԉ��ɍs�����Ƃ�����i��
            if (last - 1 < selectIndex && list.Count - 1 > selectIndex)
            {
                headIndex = selectIndex - page_max + 2;
                if (headIndex < 0) headIndex = 0;
            }
        }
    }

    /// <summary>
    /// �O�ɓ���
    /// </summary>
    override public void MoveBefore()
    {
        base.MoveBefore();
        if (list.Count == 0) return;

        if (selectIndex == list.Count - 1)
        {
            // ��Ԃ�����ɔ�񂾏ꍇ
            if (display_paging)
            {
                // �y�[�W���O�̏ꍇ��Ԃ�����y�[�W�̐擪
                headIndex = (GetNowPage() - 1) * page_max;
            }
            else
            {
                // �P���̏ꍇ�Ō�̂P�y�[�W�Ԃ�
                headIndex = list.Count - page_max + 1;
                if (headIndex < 0) headIndex = 0;
            }
        }
        else
        {
            if (display_paging)
            {
                // �ۂ��Ƃ̏ꍇ�͏���߂�����O��
                if (headIndex > selectIndex)
                    headIndex = (GetNowPage() - 1) * page_max;
            }
            else
            {
                // �P���Ƃ̏ꍇ��ԏ�ɍs�����Ƃ�����߂�
                if (headIndex >= selectIndex)
                {
                    headIndex = selectIndex - 1;
                    if (headIndex < 0) headIndex = 0;
                }
            }
        }
    }

    /// <summary>
    /// ���y�[�W�ɓ���
    /// </summary>
    public void MoveNextPage()
    {
        if (list.Count == 0) return;

        if (display_paging)
        {
            // �y�[�W���O�̏ꍇ�ŐV�y�[�W��\��
            selectIndex += page_max;
            if (selectIndex >= list.Count) selectIndex = list.Count - 1;
            headIndex = (GetNowPage() - 1) * page_max;
        }
        else
        {
            // �P���̏ꍇ�Ō�̐擪�܂�
            var maxHead = list.Count - page_max + 1;
            if (maxHead < 0) maxHead = 0;
            // ���Ɍ��E�̏ꍇ�͍Ō�
            if (headIndex == maxHead)
            {
                selectIndex = list.Count - 1;
            }
            else
            {
                // �V�w�b�h
                var newHead = headIndex + page_max;
                if (newHead > maxHead) newHead = maxHead;
                // head�̐i�݂Ԃ񂾂��I�����i��
                selectIndex += newHead - headIndex;
                headIndex = newHead;
            }
        }
    }

    /// <summary>
    /// �O�y�[�W�ɓ���
    /// </summary>
    public void MoveBeforePage()
    {
        if (list.Count == 0) return;

        if (display_paging)
        {
            // �y�[�W���O�̏ꍇ�ŐV�y�[�W��\��
            selectIndex -= page_max;
            if (selectIndex < 0) selectIndex = 0;
            headIndex = (GetNowPage() - 1) * page_max;
        }
        else
        {
            // ���ɐ擪�̏ꍇ�͐擪
            if (headIndex == 0)
            {
                selectIndex = 0;
            }
            else
            {
                // �V�w�b�h
                var newHead = headIndex - page_max;
                if (newHead < 0) newHead = 0;
                // head�̖߂�Ԃ񂾂��I�����߂�
                selectIndex -= headIndex - newHead;
                headIndex = newHead;
            }
        }
    }

    #endregion

    #region �擾

    /// <summary>
    /// �\����ԉ��̃C���f�b�N�X
    /// </summary>
    /// <returns></returns>
    public int GetDisplayLastIndex()
    {
        // �w�b�h����v�Z
        var idx = headIndex + page_max - 1;
        // ��������ꍇ�Ō�̃A�C�e��
        if (idx >= list.Count) return list.Count - 1;

        return idx;
    }

    /// <summary>
    /// �ő�y�[�W��
    /// </summary>
    /// <returns></returns>
    public int GetMaxPage()
    {
        if (!display_paging || list.Count == 0) return 1;

        return (list.Count - 1) / page_max + 1;
    }

    /// <summary>
    /// ���ݑI�𒆂̃y�[�W
    /// </summary>
    /// <returns></returns>
    public int GetNowPage()
    {
        if (!display_paging || page_max <= 0 || list.Count == 0) return 1;

        return selectIndex / page_max + 1;
    }

    /// <summary>
    /// �y�[�W���O���A���y�[�W�̖��\�����肩
    /// </summary>
    /// <returns></returns>
    public bool NeedNextPageIcon()
    {
        return display_paging ? (GetNowPage() < GetMaxPage()) : false;
    }

    /// <summary>
    /// �y�[�W���O���A�O�y�[�W�̖��\�����肩
    /// </summary>
    /// <returns></returns>
    public bool NeedBeforePageIcon()
    {
        return display_paging ? (GetNowPage() > 1) : false;
    }

    /// <summary>
    /// �X�N���[���o�[Size�p
    /// </summary>
    /// <returns></returns>
    public float GetScrollBarSize()
    {
        if (display_paging) return 1f;

        var maxHead = list.Count - page_max + 1;
        if (maxHead <= 0) return 1f;

        return 1f / (maxHead + 1);
    }

    /// <summary>
    /// �X�N���[���o�[Value�p
    /// </summary>
    /// <returns></returns>
    public float GetScrollBarValue()
    {
        if (display_paging) return 0;

        var maxHead = list.Count - page_max + 1;
        if (maxHead <= 0) return 0f;

        return (float)headIndex / maxHead;
    }

    /// <summary>
    /// �y�[�W���O����Ȃ����A���X�N���[���̕\�����肩
    /// </summary>
    /// <returns></returns>
    public bool NeedNextScroll()
    {
        if (display_paging) return false;

        var maxHead = list.Count - page_max + 1;
        if (maxHead <= 0) return false;

        return headIndex < maxHead;
    }

    /// <summary>
    /// �y�[�W���O����Ȃ����A�O�X�N���[���̕\�����肩
    /// </summary>
    /// <returns></returns>
    public bool NeedBeforeScroll()
    {
        if (display_paging) return false;
        return headIndex > 0;
    }

    #endregion
}
