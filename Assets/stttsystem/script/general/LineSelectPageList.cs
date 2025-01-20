using PlasticGui.Configuration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ページング機能つきリスト
/// </summary>
public class LineSelectPageList<T> : LineSelectList<T>
{
    #region メンバー

    /// <summary>表示先頭</summary>
    public int headIndex { get; private set; }
    /// <summary>1ページのサイズ</summary>
    private int page_max;
    /// <summary>true:ページまるごと切り替え　false:1個単位でスクロール</summary>
    private bool display_paging;

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="pageMax">1ページのサイズ</param>
    /// <param name="pagingMode">true:ページまるごと切り替え　false:1個単位でスクロール</param>
    public LineSelectPageList(int pageMax, bool pagingMode = false) : base()
    {
        display_paging = pagingMode;
        page_max = pageMax;
        headIndex = 0;
    }

    #region 操作

    /// <summary>
    /// 次に動く
    /// </summary>
    override public void MoveNext()
    {
        base.MoveNext();
        if (list.Count == 0) return;

        // 表示最大数がある場合head操作がある
        // 一番下のIndex
        var last = GetDisplayLastIndex();

        if (display_paging)
        {
            // 丸ごとの場合は下を過ぎたら次へ
            if (last < selectIndex)
                headIndex = selectIndex;
        }
        else
        {
            // １個ごとの場合一番下に行こうとしたら進む
            if (last - 1 < selectIndex && list.Count - 1 > selectIndex)
            {
                headIndex = selectIndex - page_max + 2;
                if (headIndex < 0) headIndex = 0;
            }
        }
    }

    /// <summary>
    /// 前に動く
    /// </summary>
    override public void MoveBefore()
    {
        base.MoveBefore();
        if (list.Count == 0) return;

        if (selectIndex == list.Count - 1)
        {
            // 一番うしろに飛んだ場合
            if (display_paging)
            {
                // ページングの場合一番うしろページの先頭
                headIndex = (GetNowPage() - 1) * page_max;
            }
            else
            {
                // １個ずつの場合最後の１ページぶん
                headIndex = list.Count - page_max + 1;
                if (headIndex < 0) headIndex = 0;
            }
        }
        else
        {
            if (display_paging)
            {
                // 丸ごとの場合は上を過ぎたら前へ
                if (headIndex > selectIndex)
                    headIndex = (GetNowPage() - 1) * page_max;
            }
            else
            {
                // １個ごとの場合一番上に行こうとしたら戻る
                if (headIndex >= selectIndex)
                {
                    headIndex = selectIndex - 1;
                    if (headIndex < 0) headIndex = 0;
                }
            }
        }
    }

    /// <summary>
    /// 次ページに動く
    /// </summary>
    public void MoveNextPage()
    {
        if (list.Count == 0) return;

        if (display_paging)
        {
            // ページングの場合最新ページを表示
            selectIndex += page_max;
            if (selectIndex >= list.Count) selectIndex = list.Count - 1;
            headIndex = (GetNowPage() - 1) * page_max;
        }
        else
        {
            // １個ずつの場合最後の先頭まで
            var maxHead = list.Count - page_max + 1;
            if (maxHead < 0) maxHead = 0;
            // 既に限界の場合は最後
            if (headIndex == maxHead)
            {
                selectIndex = list.Count - 1;
            }
            else
            {
                // 新ヘッド
                var newHead = headIndex + page_max;
                if (newHead > maxHead) newHead = maxHead;
                // headの進みぶんだけ選択も進む
                selectIndex += newHead - headIndex;
                headIndex = newHead;
            }
        }
    }

    /// <summary>
    /// 前ページに動く
    /// </summary>
    public void MoveBeforePage()
    {
        if (list.Count == 0) return;

        if (display_paging)
        {
            // ページングの場合最新ページを表示
            selectIndex -= page_max;
            if (selectIndex < 0) selectIndex = 0;
            headIndex = (GetNowPage() - 1) * page_max;
        }
        else
        {
            // 既に先頭の場合は先頭
            if (headIndex == 0)
            {
                selectIndex = 0;
            }
            else
            {
                // 新ヘッド
                var newHead = headIndex - page_max;
                if (newHead < 0) newHead = 0;
                // headの戻りぶんだけ選択も戻る
                selectIndex -= headIndex - newHead;
                headIndex = newHead;
            }
        }
    }

    #endregion

    #region 取得

    /// <summary>
    /// 表示一番下のインデックス
    /// </summary>
    /// <returns></returns>
    public int GetDisplayLastIndex()
    {
        // ヘッドから計算
        var idx = headIndex + page_max - 1;
        // 下すぎる場合最後のアイテム
        if (idx >= list.Count) return list.Count - 1;

        return idx;
    }

    /// <summary>
    /// 最大ページ数
    /// </summary>
    /// <returns></returns>
    public int GetMaxPage()
    {
        if (!display_paging || list.Count == 0) return 1;

        return (list.Count - 1) / page_max + 1;
    }

    /// <summary>
    /// 現在選択中のページ
    /// </summary>
    /// <returns></returns>
    public int GetNowPage()
    {
        if (!display_paging || page_max <= 0 || list.Count == 0) return 1;

        return selectIndex / page_max + 1;
    }

    /// <summary>
    /// ページング時、次ページの矢印表示ありか
    /// </summary>
    /// <returns></returns>
    public bool NeedNextPageIcon()
    {
        return display_paging ? (GetNowPage() < GetMaxPage()) : false;
    }

    /// <summary>
    /// ページング時、前ページの矢印表示ありか
    /// </summary>
    /// <returns></returns>
    public bool NeedBeforePageIcon()
    {
        return display_paging ? (GetNowPage() > 1) : false;
    }

    /// <summary>
    /// スクロールバーSize用
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
    /// スクロールバーValue用
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
    /// ページングじゃない時、次スクロールの表示ありか
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
    /// ページングじゃない時、前スクロールの表示ありか
    /// </summary>
    /// <returns></returns>
    public bool NeedBeforeScroll()
    {
        if (display_paging) return false;
        return headIndex > 0;
    }

    #endregion
}
