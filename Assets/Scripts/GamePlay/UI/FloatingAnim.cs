using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class FloatingAnim : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private float verticalPadding = 5f;
    [SerializeField]
    private float duration = 1f;
    [SerializeField]
    private float scaleFactor = 1.5f;

    //References
    private Camera gameplayCam;
    private Canvas gameplayCanvas;


    //Target transform
    private Vector2 targetRectPosition;
    private Vector3 targetScale;
    private Color targetColor;

    //Start transform
    private Vector2 startRectPosition;
    private Vector3 startScale;
    private Color startColor;

    //Components
    private RectTransform rectTransform;
    

    private int scoreAmount;
    private float animSpeed;

    public void Setup(Camera gameplayCamera, Canvas gameplayCanvas)
    {
        gameplayCam = gameplayCamera;
        this.gameplayCanvas = gameplayCanvas;

        GetComponents();
        InitValues();
    }

    void GetComponents()
    {
        rectTransform = GetComponent<RectTransform>();
        gameplayCam = Camera.main;
    }
    void InitValues()
    {
        rectTransform.localPosition = Vector3.zero;
        animSpeed = 1f / duration;
        targetScale = rectTransform.localScale * scaleFactor;
        targetColor = scoreText.color;
        targetColor.a = 0;
    }

    Coroutine floatFadeCR;
    public void FloatAndFade(Vector3 worldPosition, int scoreAmount, System.Action completed = null)
    {
        //Reset values
        this.scoreAmount = scoreAmount;

        startRectPosition = CanvasHelper.WorldPointToCanvasPoint(gameplayCam, gameplayCanvas, worldPosition);
        startRectPosition.y += (verticalPadding - 10);
        targetRectPosition = startRectPosition;
        targetRectPosition.y += verticalPadding;

        startColor = scoreText.color;
        startColor.a = 1;
        targetColor = startColor;
        targetColor.a = 0;

        startScale = Vector3.one;
        targetScale = startScale * scaleFactor;


        if (floatFadeCR != null)
            StopCoroutine(floatFadeCR);

        floatFadeCR = StartCoroutine(CR_FloatAndFade(completed));

    }

    IEnumerator CR_FloatAndFade(System.Action completed = null)
    {
        scoreText.text = scoreAmount.ToString();

        float curVal = 0;

        while (curVal < 1)
        {
            curVal += Time.deltaTime * animSpeed;
            rectTransform.localPosition = Vector3.Lerp(startRectPosition, targetRectPosition, curVal);
            rectTransform.localScale = Vector3.Lerp(startScale, targetScale, curVal);
            scoreText.color = Color.Lerp(startColor, targetColor, curVal);
            yield return null;
        }

        if (completed != null)
            completed();
    }
}


