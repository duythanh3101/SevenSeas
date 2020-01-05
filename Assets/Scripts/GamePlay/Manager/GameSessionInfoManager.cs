using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    
    public class GameSessionInfoManager : MonoBehaviour
    {
        public static GameSessionInfoManager Instance = null;

        private static readonly string PLAYER_SESSION_FILE_NAME = "player_session.json";
        private static readonly string GAME_SESSION_KEY = "GAME_SESSION";
      
        private static string playerSessionFilePath;

        public bool EndGameSession
        {
            get
            {
                return PlayerPrefs.GetInt(GAME_SESSION_KEY, 1) == 1;
            }
            private set
            {
                PlayerPrefs.SetInt(GAME_SESSION_KEY, value ? 1 : 0 );
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

        [Header("Player Session Info")]
        [HideInInspector]
        public int currentAmountScore;

        [Range(1,3)]
        [SerializeField]
        private int maxPlayerHealth = 3;

        [Tooltip("Player earn one life when getting the specific amount of score")]
        public int bonusLifeScoreAmount = 15;

        [HideInInspector]
        public PlayerInfoSession playerInfoSession;

        [Header("Battle Session Info")]
        private string sceneName;

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

        #region Player Session Info Function
        public void UpdateScore(int amount)
        {
            playerInfoSession.playerScore += amount;
            currentAmountScore += amount;

            //Check for update bonus ife
            CheckUpdateBonusLife();

            UIManager.Instance.UpdateScore(playerInfoSession.playerScore);
        }

        void CheckUpdateBonusLife()
        {
            if (currentAmountScore >= bonusLifeScoreAmount)
            {
                if (playerInfoSession.playerHealth < maxPlayerHealth)
                {
                    //Bonus one life
                    currentAmountScore = 0;
                    playerInfoSession.playerHealth++;

                    //Update UI
                    SoundManager.Instance.PlayBonusSound();
                    UIManager.Instance.IncreaseHealth(playerInfoSession.playerHealth - 1);
                }
                
            }
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

            EndGameSession = false;

            JsonFileHelper.SaveToFile(playerSessionFilePath, playerInfoSession);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif  
        }
       
        public void ClearPlayerSession()
        {
            playerInfoSession.ResetData(maxPlayerHealth);
            EndGameSession = true;
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
        #endregion

        #region Battle Session Info Function

        #endregion
    }
}

