using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalData
{
    #region 定数

    /// <summary>項目ごとの区切り</summary>
    public const string SEP_GAMEDATA_ITEM = "&";
    /// <summary>タイトルと内容の区切り</summary>
    public const string SEP_GAMEDATA_TITLE = "=";

    #endregion

    #region インスタンス取得
    private static SaveData _saveData = null;
    /// <summary>
    /// セーブデータ
    /// </summary>
    /// <returns></returns>
    public static SaveData GetSaveData()
    {
        if (_saveData == null)
        {
            _saveData = new SaveData();
        }
        return _saveData;
    }

    private static TemporaryData _temporaryData = null;
    /// <summary>
    /// 一時データ
    /// </summary>
    /// <returns></returns>
    public static TemporaryData GetTemporaryData()
    {
        if (_temporaryData == null)
        {
            _temporaryData = new TemporaryData();
        }
        return _temporaryData;
    }
    #endregion

    #region Dictionary操作

    /// <summary>
    /// 文字列で取得
    /// </summary>
    /// <param name="data"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    private static string GetDictionaryString(Dictionary<string, string> data, string key)
    {
        if (data.ContainsKey(key)) return data[key];
        return "";
    }

    /// <summary>
    /// intで取得
    /// </summary>
    /// <param name="data"></param>
    /// <param name="key"></param>
    /// <param name="def"></param>
    /// <returns></returns>
    private static int GetDictionaryInt(Dictionary<string, string> data, string key, int def = 0)
    {
        if (int.TryParse(GetDictionaryString(data, key), out int val)) return val;
        return def;
    }

    #endregion

    /// <summary>
    /// セーブデータ
    /// </summary>
    public class SaveData
    {
        /// <summary>ゲーム内のデータ</summary>
        public Dictionary<string, string> gameData;

        /// <summary>システムデータ</summary>
        public SystemData system;

        /// <summary>
        /// システムデータ
        /// </summary>
        public struct SystemData
        {
            /// <summary>BGM</summary>
            public int bgmVolume;
            /// <summary>SE</summary>
            public int seVolume;
            /// <summary>ボイス</summary>
            public int voiceVolume;

            /// <summary>システム用汎用</summary>
            public Dictionary<string, string> general;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SaveData()
        {
            gameData = new Dictionary<string, string>();

            system.bgmVolume = 3;
            system.seVolume = 3;
            system.voiceVolume = 3;

            system.general = new Dictionary<string, string>();
        }

        #region ゲームデータ

        /// <summary>
        /// ゲームデータをセーブ
        /// </summary>
        public void SaveGameData()
        {
            var serial = ToSaveString(gameData);
            PlayerPrefs.SetString("gameData", serial);

            PlayerPrefs.Save();
        }

        /// <summary>
        /// ゲームデータをロード
        /// </summary>
        public void LoadGameData()
        {
            gameData = DictionaryFromString(PlayerPrefs.GetString("gameData"));
        }

        /// <summary>
        /// ゲームデータ初期化
        /// </summary>
        public void InitGameData()
        {
            gameData.Clear();
        }

        /// <summary>
        /// データ削除
        /// </summary>
        public void DeleteGameData()
        {
            PlayerPrefs.DeleteKey("gameData");
            PlayerPrefs.Save();
            InitGameData();
        }

        /// <summary>
        /// セーブがあるかどうか
        /// </summary>
        /// <returns></returns>
        public bool IsEnableGameData()
        {
            return PlayerPrefs.HasKey("gameData");
        }

        /// <summary>
        /// ゲームデータセット
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetGameData(string key, string value)
        {
            gameData[key] = value;
        }

        /// <summary>
        /// ゲームデータセット整数版
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetGameData(string key, int value)
        {
            SetGameData(key, value.ToString());
        }

        /// <summary>
        /// ゲームデータ文字列取得
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetGameDataString(string key)
        {
            return GetDictionaryString(gameData, key);
        }

        /// <summary>
        /// ゲームデータを整数で取得
        /// </summary>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public int GetGameDataInt(string key, int def = 0)
        {
            return GetDictionaryInt(gameData, key, def);
        }

        #endregion

        #region システムデータ

        /// <summary>
        /// システムデータをセーブ
        /// </summary>
        public void SaveSystemData()
        {
            PlayerPrefs.SetInt("optionBgmVolume", system.bgmVolume);
            PlayerPrefs.SetInt("optionSeVolume", system.seVolume);
            PlayerPrefs.SetInt("optionVoiceVolume", system.voiceVolume);
            var serial = ToSaveString(system.general);
            PlayerPrefs.SetString("systemGeneral", serial);

            PlayerPrefs.Save();
        }

        /// <summary>
        /// システムデータをロード
        /// </summary>
        public void LoadSystemData()
        {
            system.bgmVolume = PlayerPrefs.GetInt("optionBgmVolume", 3);
            system.seVolume = PlayerPrefs.GetInt("optionSeVolume", 3);
            system.voiceVolume = PlayerPrefs.GetInt("optionVoiceVolume", 3);

            system.general = DictionaryFromString(PlayerPrefs.GetString("systemGeneral"));
        }

        /// <summary>
        /// システムデータ文字列取得
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSystemDataString(string key)
        {
            return GetDictionaryString(system.general, key);
        }

        /// <summary>
        /// システムデータを整数で取得
        /// </summary>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public int GetSystemDataInt(string key, int def = 0)
        {
            return GetDictionaryInt(system.general, key, def);
        }

        #endregion

        #region Dictionary操作

        /// <summary>
        /// セーブ用文字列に変換
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ToSaveString(Dictionary<string, string> data)
        {
            var strList = data.Select((pair, idx) => pair.Key + SEP_GAMEDATA_TITLE + pair.Value);

            return string.Join(SEP_GAMEDATA_ITEM, strList);
        }

        /// <summary>
        /// 文字列をdictionaryに変換
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Dictionary<string, string> DictionaryFromString(string data)
        {
            var ret = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(data)) return ret;

            foreach (var str in data.Split(SEP_GAMEDATA_ITEM))
            {
                var pair = str.Split(SEP_GAMEDATA_TITLE);
                ret[pair[0]] = pair[1];
            }

            return ret;
        }

        #endregion
    }

    /// <summary>
    /// 保存しないデータ
    /// </summary>
    public class TemporaryData
    {
        /// <summary>汎用</summary>
        public Dictionary<string, string> general;

        public int dummy;

        public bool isLoadGame;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TemporaryData()
        {
            dummy = 0;
            isLoadGame = false;
            general = new Dictionary<string, string>();
        }

        #region Dictionary操作

        /// <summary>
        /// システムデータ文字列取得
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSystemDataString(string key)
        {
            return GetDictionaryString(general, key);
        }

        /// <summary>
        /// システムデータを整数で取得
        /// </summary>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public int GetSystemDataInt(string key, int def = 0)
        {
            return GetDictionaryInt(general, key, def);
        }

        #endregion
    }
}
