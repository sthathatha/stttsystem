using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region �萔
    /// <summary>�t�B�[���hBGM�^�C�v</summary>
    public enum MainBgmType : int
    {
        None = 0,
        Common1,
        Common2,
        Common3,
        Clip,
    }

    /// <summary>BGM�t�F�[�h����</summary>
    private const float BGM_FADE_TIME = 0.5f;
    #endregion

    #region �����o�[
    /// <summary>SE�Đ������p</summary>
    public GameObject seDummy = null;
    /// <summary>�t�B�[���hBGM�Đ��p</summary>
    public AudioSource mainBgmSource = null;
    /// <summary>�Q�[��BGM�Đ��p</summary>
    public AudioSource gameBgmSource = null;
    /// <summary>�{�C�X�Đ��p</summary>
    public AudioSource voiceSource = null;

    /// <summary>�ėp�I��SE</summary>
    public AudioClip commonSeSelect = null;
    /// <summary>�ėp�J�[�\���ړ�SE</summary>
    public AudioClip commonSeMove = null;
    /// <summary>�ėp�E�B���h�E�J��SE</summary>
    public AudioClip commonSeWindowOpen = null;
    /// <summary>�ėp�G���[���u�u�b</summary>
    public AudioClip commonSeError = null;
    /// <summary>�ėp�L�����Z����</summary>
    public AudioClip commonSeCancel = null;

    /// <summary>�ėpBGM</summary>
    public AudioClip commonBgm = null;
    #endregion

    #region �v���C�x�[�g�ϐ�
    /// <summary>�Đ����̃t�B�[���hBGM</summary>
    private MainBgmType playingMainBgm;
    #endregion

    #region ���菈��
    /// <summary>
    /// ������
    /// </summary>
    void Start()
    {
        playingMainBgm = MainBgmType.None;
    }
    #endregion

    #region SE�Ǘ�
    /// <summary>
    /// SE�{�����[���ݒ�
    /// </summary>
    /// <param name="v"></param>
    public void UpdateSeVolume()
    {
        seDummy.GetComponent<AudioSource>().volume = CalcSeVolume();
    }

    /// <summary>
    /// SE�Đ�
    /// </summary>
    /// <param name="se"></param>
    /// <param name="startTime">�Đ��J�n����</param>
    public void PlaySE(AudioClip se, float startTime = 0f)
    {
        var seObj = Instantiate(seDummy, transform, false);
        seObj.gameObject.SetActive(true);

        var seSource = seObj.GetComponent<AudioSource>();
        seSource.clip = se;
        seSource.time = startTime;
        seSource.Play();

        StartCoroutine(DestroyWaitCoroutine(seSource));
    }

    /// <summary>
    /// SE���[�v�Đ�
    /// </summary>
    /// <param name="se"></param>
    /// <param name="startTime">�Đ��J�n����</param>
    /// <returns>�Ăяo�����Ő��䂷��p�I�u�W�F�N�g</returns>
    public AudioSource PlaySELoop(AudioClip se, float startTime = 0f)
    {
        var seObj = Instantiate(seDummy, transform, false);
        seObj.gameObject.SetActive(true);

        var seSource = seObj.GetComponent<AudioSource>();
        seSource.clip = se;
        seSource.time = startTime;
        seSource.loop = true;
        seSource.Play();

        return seSource;
    }

    /// <summary>
    /// SE�Đ��I����҂��č폜
    /// </summary>
    /// <param name="se"></param>
    /// <returns></returns>
    private IEnumerator DestroyWaitCoroutine(AudioSource se)
    {
        yield return new WaitWhile(() => se.isPlaying);
        Destroy(se.gameObject);
    }

    /// <summary>
    /// ���[�vSE���~�߂�
    /// </summary>
    /// <param name="se"></param>
    /// <param name="time">�t�F�[�h����</param>
    public void StopLoopSE(AudioSource se, float time = -1f)
    {
        if (time <= 0f)
        {
            se.Stop();
            Destroy(se.gameObject);
            return;
        }

        StartCoroutine(StopLoopSECoroutine(se, time));
    }

    /// <summary>
    /// ���[�vSE�t�F�[�h�A�E�g�R���[�`��
    /// </summary>
    /// <param name="se"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator StopLoopSECoroutine(AudioSource se, float time)
    {
        var vol = new DeltaFloat();
        vol.Set(se.volume);
        vol.MoveTo(0f, time, DeltaFloat.MoveType.LINE);
        while (vol.IsActive())
        {
            yield return null;
            se.volume = vol.Get();
        }

        se.Stop();
        Destroy(se);
    }

    #endregion

    #region BGM�Ǘ�
    /// <summary>
    /// 
    /// </summary>
    public void UpdateBgmVolume()
    {
        var vol = CalcBgmVolume();
        mainBgmSource.GetComponent<AudioSource>().volume = vol;
        gameBgmSource.GetComponent<AudioSource>().volume = vol;
    }

    #region �t�B�[���hBGM
    /// <summary>
    /// �t�B�[���h�ǂݍ��ݎ���BGM�Đ�
    /// </summary>
    /// <param name="mainBgmType"></param>
    /// <param name="source"></param>
    /// <param name="loop"></param>
    public void PlayMainBgm(MainBgmType mainBgmType, AudioClip source = null, bool loop = true)
    {
        if (IsNeedChangeMainBgm(mainBgmType))
        {
            var clip = mainBgmType switch
            {
                MainBgmType.Common1 => commonBgm,
                _ => source,
            };
            if (clip != null)
            {
                mainBgmSource.volume = CalcBgmVolume();
                mainBgmSource.clip = clip;
                mainBgmSource.loop = loop;
                mainBgmSource.Play();
            }
        }

        playingMainBgm = mainBgmType;
    }

    /// <summary>
    /// �t�B�[���hBGM���t�F�[�h�A�E�g����
    /// </summary>
    /// <param name="isPause">true:�|�[�Y����̂݁@false:Stop</param>
    /// <returns></returns>
    public IEnumerator FadeOutMainBgm(bool isPause = false)
    {
        if (!mainBgmSource.isPlaying)
        {
            yield break;
        }

        var vol = new DeltaFloat();
        vol.Set(mainBgmSource.volume);
        vol.MoveTo(0, BGM_FADE_TIME, DeltaFloat.MoveType.LINE);
        while (vol.IsActive())
        {
            vol.Update(Time.deltaTime);
            mainBgmSource.volume = vol.Get();
            yield return null;
        }

        if (isPause)
        {
            mainBgmSource.Pause();
        }
        else
        {
            mainBgmSource.Stop();
            playingMainBgm = MainBgmType.None;
        }
    }

    /// <summary>
    /// �t�B�[���hBGM���܂����Ă���iPause�̏ꍇ�ł�false�ɂȂ�j
    /// </summary>
    /// <returns></returns>
    public bool IsMainBgmPlaying()
    {
        return mainBgmSource.isPlaying;
    }

    /// <summary>
    /// �|�[�Y�����t�B�[���hBGM���A
    /// </summary>
    /// <returns></returns>
    public IEnumerator ResumeMainBgm()
    {
        mainBgmSource.UnPause();
        var newVol = new DeltaFloat();
        newVol.Set(0);
        newVol.MoveTo(CalcBgmVolume(), BGM_FADE_TIME, DeltaFloat.MoveType.LINE);
        while (newVol.IsActive())
        {
            newVol.Update(Time.deltaTime);
            mainBgmSource.volume = newVol.Get();
            yield return null;
        }
    }

    /// <summary>
    /// BGM�ύX�̕K�v�����邩�`�F�b�N
    /// CommonBGM�̓}�b�v�؂�ւ��ŕς��Ȃ��ꍇ�ɔ��肷��
    /// </summary>
    /// <param name="mainBgmType">�^�C�v</param>
    /// <returns>true:�ύX����@false:�p��</returns>
    public bool IsNeedChangeMainBgm(MainBgmType mainBgmType)
    {
        if (mainBgmType == MainBgmType.Clip) return true;
        if (mainBgmType != playingMainBgm) return true;
        if (!mainBgmSource.isPlaying) return true;

        return false;
    }
    #endregion

    #region �Q�[��BGM
    /// <summary>
    /// �Q�[���J�n����BGM�J�n
    /// </summary>
    /// <param name="source"></param>
    public void StartGameBgm(AudioClip source = null)
    {
        if (!source) return;

        gameBgmSource.volume = CalcBgmVolume();
        gameBgmSource.clip = source;
        gameBgmSource.Play();
    }

    /// <summary>
    /// �Q�[��BGM���~�߂�
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeOutGameBgm()
    {
        if (!gameBgmSource.isPlaying) yield break;

        var vol = new DeltaFloat();
        vol.Set(gameBgmSource.volume);
        vol.MoveTo(0, BGM_FADE_TIME, DeltaFloat.MoveType.LINE);
        while (vol.IsActive())
        {
            vol.Update(Time.deltaTime);
            gameBgmSource.volume = vol.Get();
            yield return null;
        }
        gameBgmSource.Stop();
    }

    /// <summary>
    /// �Q�[��BGM���܂������Ă�
    /// </summary>
    /// <returns></returns>
    public bool IsGameBgmPlaying()
    {
        return gameBgmSource.isPlaying;
    }

    #endregion

    #endregion

    #region �{�C�X�Ǘ�
    /// <summary>
    /// �{�C�X�{�����[���ݒ�
    /// </summary>
    public void UpdateVoiceVolume()
    {
        voiceSource.volume = CalcVoiceVolume();
    }

    /// <summary>
    /// �{�C�X�Đ�
    /// </summary>
    /// <param name="voice"></param>
    public void PlayVoice(AudioClip voice)
    {
        StopVoice();
        if (voice)
        {
            voiceSource.clip = voice;
            voiceSource.Play();
        }
    }

    /// <summary>
    /// �Đ����̃{�C�X���~
    /// </summary>
    public void StopVoice()
    {
        if (voiceSource.isPlaying) { voiceSource.Stop(); }
    }
    #endregion

    #region �v���C�x�[�g
    /// <summary>
    /// BGMSource�̍Đ��{�����[��
    /// </summary>
    /// <returns></returns>
    private float CalcBgmVolume()
    {
        var optionVol = GlobalData.GetSaveData().system.bgmVolume;
        return 0.2f * optionVol / 10;
    }
    /// <summary>
    /// SESource�̍Đ��{�����[��
    /// </summary>
    /// <returns></returns>
    private float CalcSeVolume()
    {
        var optionVol = GlobalData.GetSaveData().system.seVolume;
        return 1f * optionVol / 10;
    }
    /// <summary>
    /// VoiceSource�̍Đ��{�����[��
    /// </summary>
    /// <returns></returns>
    private float CalcVoiceVolume()
    {
        var optionVol = GlobalData.GetSaveData().system.voiceVolume;
        return 1f * optionVol / 10;
    }
    #endregion
}
