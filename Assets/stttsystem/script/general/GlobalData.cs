using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalData
{
    #region �萔

    /// <summary>���ڂ��Ƃ̋�؂�</summary>
    public const string SEP_GAMEDATA_ITEM = "&";
    /// <summary>�^�C�g���Ɠ��e�̋�؂�</summary>
    public const string SEP_GAMEDATA_TITLE = "=";

    #endregion

    #region �C���X�^���X�擾
    private static SaveData _saveData = null;
    /// <summary>
    /// �Z�[�u�f�[�^
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
    /// �ꎞ�f�[�^
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

    #region Dictionary����

    /// <summary>
    /// ������Ŏ擾
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
    /// int�Ŏ擾
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
    /// �Z�[�u�f�[�^
    /// </summary>
    public class SaveData
    {
        /// <summary>�Q�[�����̃f�[�^</summary>
        public Dictionary<string, string> gameData;

        /// <summary>�V�X�e���f�[�^</summary>
        public SystemData system;

        /// <summary>
        /// �V�X�e���f�[�^
        /// </summary>
        public struct SystemData
        {
            /// <summary>BGM</summary>
            public int bgmVolume;
            /// <summary>SE</summary>
            public int seVolume;
            /// <summary>�{�C�X</summary>
            public int voiceVolume;

            /// <summary>�V�X�e���p�ėp</summary>
            public Dictionary<string, string> general;
        }

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public SaveData()
        {
            gameData = new Dictionary<string, string>();

            system.bgmVolume = 3;
            system.seVolume = 3;
            system.voiceVolume = 3;

            system.general = new Dictionary<string, string>();
        }

        #region �Q�[���f�[�^

        /// <summary>
        /// �Q�[���f�[�^���Z�[�u
        /// </summary>
        public void SaveGameData()
        {
            var serial = ToSaveString(gameData);
            PlayerPrefs.SetString("gameData", serial);

            PlayerPrefs.Save();
        }

        /// <summary>
        /// �Q�[���f�[�^�����[�h
        /// </summary>
        public void LoadGameData()
        {
            gameData = DictionaryFromString(PlayerPrefs.GetString("gameData"));
        }

        /// <summary>
        /// �Q�[���f�[�^������
        /// </summary>
        public void InitGameData()
        {
            gameData.Clear();
        }

        /// <summary>
        /// �f�[�^�폜
        /// </summary>
        public void DeleteGameData()
        {
            PlayerPrefs.DeleteKey("gameData");
            PlayerPrefs.Save();
            InitGameData();
        }

        /// <summary>
        /// �Z�[�u�����邩�ǂ���
        /// </summary>
        /// <returns></returns>
        public bool IsEnableGameData()
        {
            return PlayerPrefs.HasKey("gameData");
        }

        /// <summary>
        /// �Q�[���f�[�^�Z�b�g
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetGameData(string key, string value)
        {
            gameData[key] = value;
        }

        /// <summary>
        /// �Q�[���f�[�^�Z�b�g������
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetGameData(string key, int value)
        {
            SetGameData(key, value.ToString());
        }

        /// <summary>
        /// �Q�[���f�[�^������擾
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetGameDataString(string key)
        {
            return GetDictionaryString(gameData, key);
        }

        /// <summary>
        /// �Q�[���f�[�^�𐮐��Ŏ擾
        /// </summary>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public int GetGameDataInt(string key, int def = 0)
        {
            return GetDictionaryInt(gameData, key, def);
        }

        #endregion

        #region �V�X�e���f�[�^

        /// <summary>
        /// �V�X�e���f�[�^���Z�[�u
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
        /// �V�X�e���f�[�^�����[�h
        /// </summary>
        public void LoadSystemData()
        {
            system.bgmVolume = PlayerPrefs.GetInt("optionBgmVolume", 3);
            system.seVolume = PlayerPrefs.GetInt("optionSeVolume", 3);
            system.voiceVolume = PlayerPrefs.GetInt("optionVoiceVolume", 3);

            system.general = DictionaryFromString(PlayerPrefs.GetString("systemGeneral"));
        }

        /// <summary>
        /// �V�X�e���f�[�^������擾
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSystemDataString(string key)
        {
            return GetDictionaryString(system.general, key);
        }

        /// <summary>
        /// �V�X�e���f�[�^�𐮐��Ŏ擾
        /// </summary>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public int GetSystemDataInt(string key, int def = 0)
        {
            return GetDictionaryInt(system.general, key, def);
        }

        #endregion

        #region Dictionary����

        /// <summary>
        /// �Z�[�u�p������ɕϊ�
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ToSaveString(Dictionary<string, string> data)
        {
            var strList = data.Select((pair, idx) => pair.Key + SEP_GAMEDATA_TITLE + pair.Value);

            return string.Join(SEP_GAMEDATA_ITEM, strList);
        }

        /// <summary>
        /// �������dictionary�ɕϊ�
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
    /// �ۑ����Ȃ��f�[�^
    /// </summary>
    public class TemporaryData
    {
        /// <summary>�ėp</summary>
        public Dictionary<string, string> general;

        public int dummy;

        public bool isLoadGame;

        /// <summary>
        /// �R���X�g���N�^
        /// </summary>
        public TemporaryData()
        {
            dummy = 0;
            isLoadGame = false;
            general = new Dictionary<string, string>();
        }

        #region Dictionary����

        /// <summary>
        /// �V�X�e���f�[�^������擾
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSystemDataString(string key)
        {
            return GetDictionaryString(general, key);
        }

        /// <summary>
        /// �V�X�e���f�[�^�𐮐��Ŏ擾
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
