using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FadeScene_UI : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetTrigger("FadeIn");


    }

    public void FadeOut() { anim.SetTrigger("FadeOut"); }
    public void FadeIn() => anim.SetTrigger("FadeIn");
}
