using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUIController : MonoBehaviour
{
    public event System.Action OnRestartButtonClick = delegate { };

    [SerializeField]
    private Image gameOverImage;
    [SerializeField]
    private CanvasGroup canvasGroup;
    [SerializeField]
    private Button restartButton;

    private Coroutine showImageCR;

    void Awake()
    {
        restartButton.onClick.AddListener(() => OnRestartButtonClick());
    }
  
    void Start()
    {
        Hide();
    }

    public void ShowGameOverImage()
    {
        gameOverImage.gameObject.SetActive(true);
        if (showImageCR != null)
            StopCoroutine(showImageCR);
        showImageCR = StartCoroutine(CR_FadeInImage(gameOverImage));
    }

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
        canvasGroup.interactable = isShowing;
        canvasGroup.alpha = isShowing ? 1 : 0;
        canvasGroup.blocksRaycasts = isShowing;

    }

    IEnumerator CR_FadeInImage(Image image)
    {
        Color currentColor = image.color;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            currentColor.a = Mathf.Lerp(0, 1, t);
            image.color = currentColor;
            yield return null;
        }
    }


}
