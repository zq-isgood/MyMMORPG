using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITest : UIWindow
{
    public TextMeshProUGUI title;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetTitle(string title)
    {
        this.title.text = title;
    }

}
