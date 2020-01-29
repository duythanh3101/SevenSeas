using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SevenSeas
{
    public class LeaderboardManager : MonoBehaviour
    {
        public static LeaderboardManager Instance = null;

        [SerializeField]
        private int numberOfHighScores = 5;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                DestroyImmediate(gameObject);
        }

        public void UploadScore(HighScoreModel highScore, System.Action successCallback = null, System.Action failedCallback = null)
        {
            StartCoroutine(CR_UploadScore(highScore,successCallback, failedCallback)); 
        }


        IEnumerator CR_UploadScore(HighScoreModel highScore, System.Action successCallback = null, System.Action failedCallback = null)
        {
            string uri =string.Format("{0}/{1}/{2}/{3}/{4}", StringConstant.DREAMLO_WEB_URL, StringConstant.DREAMLO_PRIVATE_CODE, "add", highScore.username, highScore.score.ToString());
           
            //Debug.Log(string.Format("Uploading: {0}:{1}...",highScore.username,highScore.score));
            using (UnityWebRequest webRequest = new UnityWebRequest(uri))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    failedCallback();
                }
                else
                {
                    successCallback();
                }

            }
        }

        public void DownloadHighScores(System.Action<string> callback)
        {
            StartCoroutine(CR_DownloadHighScores(numberOfHighScores, callback));
        }

        IEnumerator CR_DownloadHighScores(int n, System.Action<string> callback)
        {
            string uri = string.Format("{0}/{1}/{2}/{3}", StringConstant.DREAMLO_WEB_URL, StringConstant.DREAMLO_PUBLIC_CODE, "pipe",n.ToString());
            using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
            {
                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    Debug.LogError(webRequest.error);
                }
                else
                {
                    if (callback != null)
                        callback(webRequest.downloadHandler.text);
                }

            }
        }
    }
}

