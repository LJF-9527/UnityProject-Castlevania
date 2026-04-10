using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity=>GetComponentInParent<Entity>();
    private CharacterStats myStats=>GetComponentInParent<CharacterStats>();
    private RectTransform myTransform;
    private Slider slider;
    private void Start()
    {
        myTransform = GetComponent<RectTransform>();
        slider=GetComponentInChildren<Slider>();

        
        UpdateHealthUI();
    }
  
    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }
    private void OnEnable()
    {
        entity.onFlipped += FlipUI;
        myStats.onHealthChange += UpdateHealthUI;
    }
    private void OnDisable() 
    { 
        if(entity!=null)
        entity.onFlipped -= FlipUI; 
        if(myStats!=null)
        myStats.onHealthChange-= UpdateHealthUI;
    }
    private void FlipUI()=> myTransform.Rotate(0, 180, 0);
        
}
