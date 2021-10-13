using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;

    private List<FloatingText> floatingTexts = new List<FloatingText>();

    

    private void Update()
    {
        foreach(FloatingText txt in floatingTexts)
        {
            txt.UpdateFloatingText();
        }
    }



    public void Show(string msg,int fontSize, Color color
                        , Vector3 positon, Vector3 motion, float duration)
    {
        FloatingText floatingtxt = GetFloatingText();

        floatingtxt.txt.text = msg;
        floatingtxt.txt.fontSize = fontSize;
        floatingtxt.txt.color = color;
        //Tranfer world space to screen space so we can use it in the UI
        floatingtxt.go.transform.position =Camera.main.WorldToScreenPoint(positon);
        floatingtxt.motion = motion;
        floatingtxt.duration = duration;

        floatingtxt.Show();


    }


    private FloatingText GetFloatingText()
    {
        FloatingText txt = floatingTexts.Find(t => !t.active);

        if (txt == null)
        {
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<Text>();

            floatingTexts.Add(txt);

        }
        return txt;
    }
}
