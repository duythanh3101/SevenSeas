using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This will be using object pooling
public class FloatingScorePointUIController : MonoBehaviour
{

    [SerializeField]
    private GameObject scorePointPrefab;
    [SerializeField]
    private Camera gamplayCam;
    [SerializeField]
    private Canvas gameplayCanvas;

    Queue<GameObject> scorePointQueue = new Queue<GameObject>();
    RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        InitFloatingScorePoints();
    }

    public void SpawnFloatingScorePoint(Vector3 worldPosition, int scoreAmount)
    {
        GameObject scorePointIns = scorePointQueue.Dequeue();

        scorePointIns.SetActive(true);
        scorePointIns.GetComponent<FloatingAnim>().FloatAndFade(worldPosition, scoreAmount, () =>
        {
            scorePointIns.SetActive(false);
            scorePointQueue.Enqueue(scorePointIns);
        });
    }

    void InitFloatingScorePoints()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject scorePointIns =  Instantiate(scorePointPrefab, Vector3.zero, Quaternion.identity, transform);
            scorePointIns.GetComponent<FloatingAnim>().Setup(gamplayCam,gameplayCanvas);
            scorePointIns.SetActive(false);
            scorePointQueue.Enqueue(scorePointIns);
        }

    }
}
