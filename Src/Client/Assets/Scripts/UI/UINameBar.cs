using Entities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINameBar : MonoBehaviour
{
    public TextMeshProUGUI avatarName;
    public Character character;
    public Image iconch;
    public Image iconmo;
    void Start()
    {
        if(this.character != null)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfo();
       
        //this.transform.LookAt(Camera.main.transform, Vector3.up);
    }
    void UpdateInfo()
    {
        if (this.character != null)
        {
            string name = this.character.Name + "Lv." + this.character.Info.Level;
            if (name != this.avatarName.text)
            {
                this.avatarName.text = name;
            }
            if (character.Id == 0)
            {
                this.iconch.gameObject.SetActive(false);
            }
            else
            {
                this.iconmo.gameObject.SetActive(false);
            }
        }
    }
}
