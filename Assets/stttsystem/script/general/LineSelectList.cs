using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// メニュー型線形リスト
/// </summary>
public class LineSelectList<T>
{
    #region メンバー

    /// <summary>リスト本体</summary>
    protected List<T> list;

    /// <summary>選択中インデックス</summary>
    public int selectIndex { get; protected set; }

    #endregion

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public LineSelectList()
    {
        list = new List<T>();
        selectIndex = 0;
    }

    /// <summary>
    /// アイテム追加
    /// </summary>
    /// <param name="items"></param>
    public void AddItem(params T[] items)
    {
        list.AddRange(items);
    }

    #region 操作

    /// <summary>
    /// 次に動く
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
    /// 前に動く
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

    #region 取得

    /// <summary>
    /// アイテム数
    /// </summary>
    /// <returns></returns>
    public int GetAllCount() { return list.Count; }

    /// <summary>
    /// アイテム取得
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public T GetItem(int index)
    {
        if (index < 0 || index >= list.Count) return default;
        return list[index];
    }

    /// <summary>
    /// 選択中アイテム取得
    /// </summary>
    /// <returns></returns>
    public T GetSelectItem()
    {
        return GetItem(selectIndex);
    }

    #endregion
}
