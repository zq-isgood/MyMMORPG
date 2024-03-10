using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabButton : MonoBehaviour
{
    public Sprite activeImage;
    private Sprite normalImage;
    public TabView tabView;
    public int tabIndex = 0;
    public bool selected = false;
    private Image tabImage;
    void Start()
    {
        tabImage = this.GetComponent<Image>();
        normalImage = tabImage.sprite;
        this.GetComponent<Button>().onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    public void Select(bool select)
    {
        tabImage.overrideSprite = select ? activeImage : normalImage;
    }
    void OnClick()
    {
        this.tabView.SelectTab(this.tabIndex);
    }
}
