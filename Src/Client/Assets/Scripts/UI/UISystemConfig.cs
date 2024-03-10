using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISystemConfig : UIWindow
{
    public Toggle toggleMusic;
    public Toggle toggleSound;
    public Slider sliderMusic;
    public Slider sliderSound;
    public Image musicOff;
    public Image soundOff;
    private void Start()
    {
        //��ʼ��ֵ
        this.toggleMusic.isOn = Config.MusicOn;
        this.toggleSound.isOn = Config.SoundOn;
        this.sliderMusic.value = Config.MusicVolume;
        this.sliderSound.value = Config.SoundVolume;
    }
    //����رգ�
    public override void OnYesClick()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        PlayerPrefs.Save();
        base.OnYesClick();
    }
    //������ֵĿ��أ�
    public void MusicToggle(bool on)
    {
        musicOff.enabled = !on; //ͼƬ��ʾ���ǲ���ʾ
        Config.MusicOn = on;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);  //���ŵ��������
    }
    public void SoundToggle(bool on)
    {
        soundOff.enabled = !on; //ͼƬ��ʾ���ǲ���ʾ
        Config.SoundOn = on;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);  //���ŵ��������
    }
    public void SoundVolume(float vol)
    {
        Config.SoundVolume = (int)vol;
        PlaySound();
    }
    public void MusicVolume(float vol)
    {
        Config.MusicVolume = (int)vol;
        PlaySound();
    }
    float lastPlay = 0;
    private void PlaySound()
    {
        if (Time.realtimeSinceStartup - lastPlay > 0.1)
        {
            lastPlay = Time.realtimeSinceStartup;
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        }
    }
}