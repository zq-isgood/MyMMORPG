using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;


public class UILogin : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public Button buttonLogin;
    void Start()
    {
        UserService.Instance.OnLogin = this.OnLogin;
    }
    void OnLogin(Result result, string msg)
    {
        if(result == Result.Success){
            Debug.Log(msg);
            SceneManager.Instance.LoadScene("CharSelect"); //º”‘ÿΩ«…´—°‘Òµƒ≥°æ∞
            SoundManager.Instance.PlayMusic(SoundDefine.Music_Select);
        }
        else
        {
            MessageBox.Show(msg, "¥ÌŒÛ", MessageBoxType.Error);
        }

    }
    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(this.username.text))
        {
            MessageBox.Show("«Î ‰»Î’À∫≈");
            return;
        }
        if (string.IsNullOrEmpty(this.password.text))
        {
            MessageBox.Show("«Î ‰»Î√‹¬Î");
            return;
        }
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        UserService.Instance.SendLogin(this.username.text, this.password.text);
    }
    void Update()
    {
        
    }
}
