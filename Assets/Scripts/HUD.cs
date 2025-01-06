using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health }    //¿­°ÅÇü
    public InfoType type;

    TextMeshProUGUI myText;
    Slider mySlider;

    void Awake()
    {
        myText = GetComponent<TextMeshProUGUI>();
        mySlider = GetComponent<Slider>();
    }

    void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length - 1)];
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format($"Lv.{GameManager.instance.level:F0}");
                break;
            case InfoType.Kill:
                myText.text = string.Format($"{GameManager.instance.kill:F0}");
                break;
            case InfoType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format($"{min:D2}:{sec:D2}");
                break;
            case InfoType.Health:
                float curHp = GameManager.instance.hp;
                float maxHp = GameManager.instance.maxHp;
                mySlider.value = curHp / maxHp;
                break;
        }
    }
}
