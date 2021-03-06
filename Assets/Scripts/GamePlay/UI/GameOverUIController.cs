﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIController : MonoBehaviour
{
   

    public float timeDisplay = 1;

    [SerializeField]
    private Image gameOverImage;
    [SerializeField]
    private CanvasGroup canvasGroup;

    
    public void Show()
    {
        Display(true);
        ShowGameOverImage();
    }
    public void Hide()
    {
        Display(false);
    }

    void Display(bool isShowing)
    {
        canvasGroup.alpha = isShowing ? 1 : 0;
        canvasGroup.blocksRaycasts = isShowing;
        canvasGroup.interactable = isShowing;
    }

    private Coroutine fadeImageCR;
    void ShowGameOverImage()
    {
        gameOverImage.gameObject.SetActive(true);

        if (fadeImageCR != null)
            StopCoroutine(fadeImageCR);
         fadeImageCR = StartCoroutine(CR_FadeInImage(gameOverImage));


    }

    IEnumerator CR_FadeInImage(Image img)
    {
        Color currentColr = img.color;
        currentColr.a = 0;
        float t = 0;
        while (t < timeDisplay)
        {
            t += Time.deltaTime;
            currentColr.a = Mathf.Lerp(0, 1, t);
            img.color = currentColr;
            yield return null;
        }
    }


}
