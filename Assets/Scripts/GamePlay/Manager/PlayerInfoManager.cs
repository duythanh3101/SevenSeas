using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    
    public class PlayerInfoManager : MonoBehaviour
    {
        public static PlayerInfoManager Instance = null;

        private static readonly string PLAYER_SESSION_FILE_NAME = "player_session.json";
        private static readonly string END_PLAYER_SESSION_KEY = "END_PLAYER_SESSION";


        private static string playerSessionFilePath;

        public bool EndPlayerSession
        {
            get
            {
                return PlayerPrefs.GetInt(END_PLAYER_SESSION_KEY, 1) == 1;
            }
            private set
            {
                PlayerPrefs.SetInt(END_PLAYER_SESSION_KEY, value ? 1 : 0 );
                PlayerPrefs.Save();
            }
        }

        [System.Serializable]
        public class PlayerInfoSession
        {
            public int levelInCheckPoint;
            public int playerScore;
            public int piratesSunk;
            public int checkPoint;
            public int treasureFound;
            public int playerHealth;

           

            public void ResetData(int pHealth)
            {
                levelInCheckPoint = 0;
                playerScore = 0;
                piratesSunk = 0;
                checkPoint = 0;
                treasureFound = 0;
                playerHealth = pHealth;
            }

        }

        [Range(1,3)]
        [SerializeField]
        private int maxPlayerHealth = 3;

        [HideInInspector]
        public PlayerInfoSession playerInfoSession;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
                DestroyImmediate(gameObject);

            playerSessionFilePath = System.IO.Path.Combine(Application.dataPath,"Data", PLAYER_SESSION_FILE_NAME);
        }

        void Start()
        {
            playerInfoSession.ResetData(maxPlayerHealth);
        }

        public void UpdateScore(int amount)
        {
            playerInfoSession.playerScore += amount;
            UIManager.Instance.UpdateScore(playerInfoSession.playerScore);
        }

        public void UpdatePirateSunk()
        {
            playerInfoSession.piratesSunk++;
            
        }

        public void UpdateCheckPoint()
        {
            playerInfoSession.checkPoint++;
        }

        public void SavePlayerSession()
        {
            //Debug.Log("Saving player session to: " + playerSessionFilePath);

            EndPlayerSession = false;

            JsonFileHelper.SaveToFile(playerSessionFilePath, playerInfoSession);
            UnityEditor.AssetDatabase.Refresh();
        }
       
        public void ClearPlayerSession()
        {
            playerInfoSession.ResetData(maxPlayerHealth);
            EndPlayerSession = true;
        }

        public void LoadPlayerSession()
        {
            playerInfoSession = JsonFileHelper.LoadFromFile<PlayerInfoSession>(playerSessionFilePath) as PlayerInfoSession;

        }

        public void UpdatePlayerHealth(int pHealth)
        {
            playerInfoSession.playerHealth = pHealth;
        }
        
        public void UpdateLevelInCheckPoint()
        {
            playerInfoSession.levelInCheckPoint++;

            if (playerInfoSession.levelInCheckPoint > CommonConstants.MAX_LEVEL_PER_CHECKPOINT)
            {
                playerInfoSession.levelInCheckPoint = 1;
            }
        }
    }
}

