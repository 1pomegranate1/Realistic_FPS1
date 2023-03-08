using UnityEngine;
using System.Collections;
using System.IO;

public class GameVersonFindScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Font myFont;
    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();
        Rect rect = new Rect(w / 1300, 0, w, h);
        //Debug.Log("rect"+ rect);
        style.alignment = TextAnchor.LowerLeft;
        style.font = myFont;
        style.fontSize = h * 2 / 80;
        style.normal.textColor = new Color(0.1f, 0.1f, 0.1f, 0.6f);
        string text = string.Format($"{Application.version} Verson  Made By 석현  {Application.platform}");

        GUI.Label(rect, text, style);
    }
}
