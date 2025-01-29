using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    public Slider healthBar;
    public RectTransform postureBar;
    public Image postureBarImage;
    public Boss boss;
    public float posturePercentage;
    public const float POSTURE_BAR_WIDTH = 588f;
    private void Awake(){

    }

    private void Start() {
        SetHealth(boss.Health);
        SetPosture(boss.Posture/200);
    }

    private void Update() {
        SetHealth(boss.Health);
        SetPosture(Mathf.Abs((boss.Posture/200f) - 1));
    }

    public void SetHealth(float health){
        healthBar.value = health;
    }
    public void SetPosture(float posture){
        postureBar.sizeDelta = new Vector2(posture * POSTURE_BAR_WIDTH, postureBar.sizeDelta.y);
        Color postureColor = Color.Lerp(Color.yellow, Color.red, posture);
        postureBarImage.color = postureColor;
    }
}
