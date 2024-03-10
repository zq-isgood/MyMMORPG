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
        //初始赋值
        this.toggleMusic.isOn = Config.MusicOn;
        this.toggleSound.isOn = Config.SoundOn;
        this.sliderMusic.value = Config.MusicVolume;
        this.sliderSound.value = Config.SoundVolume;
    }
    //点击关闭：
    public override void OnYesClick()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        PlayerPrefs.Save();
        base.OnYesClick();
    }
    //点击音乐的开关：
    public void MusicToggle(bool on)
    {
        musicOff.enabled = !on; //图片显示还是不显示
        Config.MusicOn = on;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);  //播放点击的声音
    }
    public void SoundToggle(bool on)
    {
        soundOff.enabled = !on; //图片显示还是不显示
        Config.SoundOn = on;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);  //播放点击的声音
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