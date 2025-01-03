using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Health and Posture Sliders")]
    public Slider HealthSlider;
    public Gradient HealthGradient;
    public Image HealthFill;
    public Slider PostureSlider;
    public Gradient PostureGradient;
    public Image PostureFill;
    public CharacterStats stats;



    private void Start() {
        SetMaxHealth();
        SetMaxPosture();
    }
    private void Update() {
        SetHealth();
        SetPosture();
    }

    public void SetMaxHealth(){
        HealthSlider.maxValue = stats.healthMAX;
        HealthSlider.value = stats.healthMAX;
        PostureFill.color = HealthGradient.Evaluate(1f);

    }
    public void SetHealth(){
        HealthSlider.value = stats.health;
        HealthFill.color = HealthGradient.Evaluate(HealthSlider.normalizedValue);
    }

    public void SetMaxPosture(){
        PostureSlider.maxValue = stats.PostureMAX;
        PostureSlider.value = stats.PostureMAX;
        PostureFill.color = PostureGradient.Evaluate(1f);
    }
    public void SetPosture(){
        PostureSlider.value = stats.Posture;
        PostureFill.color = PostureGradient.Evaluate(PostureSlider.normalizedValue);
    }
}
