﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    public enum GameMode
    {
        Easy,
        Medium,
        Hard
    }

    public class GameSessionInfoManager : MonoBehaviour
    {
        public static event System.Action OnLeaderboardDataLoaded = delegate { };

        public static GameSessionInfoManager Instance = null;

        private static readonly string PLAYER_SESSION_FILE_NAME = "player_session.json";
        private static readonly string BATTLE_SESSION_FILE_NAME = "battle_session.json";
        private static readonly string LEVEL_FILE_NAME = "levels.csv";

        private static readonly string LOAD_PREVIOUS_SESSION_KEY = "LOAD_PREVIOUS_SESSION";
     
        private static string playerSessionFilePath;
        private static string battleSessionFilePath;
        private static string levelSessionFilePath;

        public GameMode gameMode;

        public bool LoadPreviousSession
        {
            get
            {
                return PlayerPrefs.GetInt(LOAD_PREVIOUS_SESSION_KEY, 0) == 1;
            }
            private set
            {
                PlayerPrefs.SetInt(LOAD_PREVIOUS_SESSION_KEY, value ? 1 : 0);
                PlayerPrefs.Save();
            }
        }

       
        [System.Serializable]
        public class PlayerInfoSession
        {
            public int totalLevelCount;
            public int levelInCheckPoint;
            public int playerScore;
            public int piratesSunk;
            public int checkPoint;
            public int treasureFound;
            public int playerHealth;
           

            public void ResetData(int pHealth)
            {
                totalLevelCount = 0;
                levelInCheckPoint = 0;
                playerScore = 0;
                piratesSunk = 0;
                checkPoint = 1;
                treasureFound = 0;
                playerHealth = pHealth;
            }
        }

       

        [System.Serializable]
        public class BattleInfoSession
        {
            public string currentSceneName;

            public DataTransform playerTransform;

            public List<DataTransform> normalEnemyTransform;
            public List<DataTransform> advanceEnemyTransform;
            public List<DataTransform> firingEnemyTransform;

            public List<Vector3> islandPosition;
            public List<Vector3> skullPosition;

            public void ResetData()
            {
                normalEnemyTransform.Clear();
                advanceEnemyTransform.Clear();
                firingEnemyTransform.Clear();
                islandPosition.Clear();
                skullPosition.Clear();
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

       // [HideInInspector]
        public PlayerInfoSession playerInfoSession;
        public List<HighScoreModel> highscores = new List<HighScoreModel>();

        [Header("Battle Session Info")]
        private string sceneName;
        [HideInInspector]
        public BattleInfoSession battleInfoSession;


        private List<string> levelDatas = new List<string>();

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
            battleSessionFilePath = System.IO.Path.Combine(Application.dataPath, "Data", BATTLE_SESSION_FILE_NAME);
            levelSessionFilePath = System.IO.Path.Combine(Application.dataPath, "Data", LEVEL_FILE_NAME);

        }

        void Start()
        {
            playerInfoSession.ResetData(maxPlayerHealth);
            levelDatas = CSVFileHelper.LoadDataFromFile(levelSessionFilePath);
            LoadLeaderboard();
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
                    //Debug.Log( "Player Health: " + playerInfoSession.playerHealth);
                    UIManager.Instance.IncreaseHealth(playerInfoSession.playerHealth -1);
                }
                
            }
        }

        public void UpdatePirateSunk()
        {
            playerInfoSession.piratesSunk++;
            
        }

        void SavePlayerSession()
        {
            //Debug.Log("Saving player session to: " + playerSessionFilePath);
            JsonFileHelper.SaveToFile(playerSessionFilePath, playerInfoSession);

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif  
        }
       
         void ClearPlayerSession()
        {
            playerInfoSession.ResetData(maxPlayerHealth);
            
        }

         void LoadPlayerSession()
        {
            playerInfoSession = JsonFileHelper.LoadFromFile<PlayerInfoSession>(playerSessionFilePath) as PlayerInfoSession;

        }

        public void DecreseHealth()
        {

            playerInfoSession.playerHealth--;
        }
        
        public void UpdateLevelInCheckPoint()
        {
            playerInfoSession.totalLevelCount++;
            playerInfoSession.levelInCheckPoint++;

            if (playerInfoSession.levelInCheckPoint > CommonConstants.MAX_LEVEL_PER_CHECKPOINT)
            {
                playerInfoSession.levelInCheckPoint = 1;
                UpdateCheckPoint();
            }
        }

        public void UpdateCheckPoint()
        {
            playerInfoSession.checkPoint++;
            if (playerInfoSession.checkPoint > CommonConstants.MAX_CHECKPOINT)
            {
                //Finish game action goes here

            }

        }

        public void UpdateTreasureFound()
        {
            playerInfoSession.treasureFound++;
        }

        public void SetPlayerMaxHealth()
        {
            switch (gameMode)
            {
                case GameMode.Easy:
                    maxPlayerHealth = 3;
                    break;
                case GameMode.Medium:
                    maxPlayerHealth = 2;
                    break;
                case GameMode.Hard:
                    maxPlayerHealth = 2;
                    break;
            }

            playerInfoSession.playerHealth = maxPlayerHealth;
        }
        #endregion

        #region Battle Session Info Function
         void SaveBattleSession()
        {
            battleInfoSession.ResetData();

            //Save current battle scene name
            battleInfoSession.currentSceneName = SceneLoader.Instance.GetActiveSceneName();

            //Save player transform
            var player = GameObject.FindGameObjectWithTag("PlayerShip").GetComponent<PlayerController>();
            battleInfoSession.playerTransform.SetData(player.transform.position,player.isometricModel.transform.rotation,
                player.currentDirection);
            

            //Save enemies
            var enemies = GameObject.FindGameObjectsWithTag("EnemyShip");
            for (int i = 0; i < enemies.Length;i++)
            {
                EnemyController enemyController = enemies[i].GetComponent<EnemyController>();
                Vector3 pos = enemyController.transform.position;
                Direction boatDir = enemyController.currentDirection;
                Quaternion modelRotation = enemyController.isometricModel.transform.rotation;

                var dataTransform = new DataTransform();
                dataTransform.SetData(pos, modelRotation, boatDir);

                if (enemyController.GetType() == typeof(NormalEnemyController))
                {
                    battleInfoSession.normalEnemyTransform.Add(dataTransform);
                }
                else if (enemyController.GetType() == typeof(AdvanceEnemyController))
                {
                    battleInfoSession.advanceEnemyTransform.Add(dataTransform);
                }
                else if (enemyController.GetType() == typeof(FiringEnemyController))
                {
                    battleInfoSession.firingEnemyTransform.Add(dataTransform);
                }
            }
            
            //Save obstacles
            var obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            for (int i  = 0; i < obstacles.Length;i++)
            {
                if (obstacles[i].name.Contains("Island"))
                {
                    battleInfoSession.islandPosition.Add(obstacles[i].transform.position);
                }
                else if (obstacles[i].name.Contains("Skull"))
                {
                    battleInfoSession.skullPosition.Add(obstacles[i].transform.position);
                }
            }

            //Save to file
            JsonFileHelper.SaveToFile(battleSessionFilePath, battleInfoSession);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif  
        }
         void ClearBattleSession()
        {
            battleInfoSession.ResetData();
        }

        void LoadBattleSession()
        {
            battleInfoSession = JsonFileHelper.LoadFromFile<BattleInfoSession>(battleSessionFilePath) as BattleInfoSession;
        }

        #endregion

        public void ClearGameSession()
        {
            ClearPlayerSession();
            ClearBattleSession();
            LoadPreviousSession = false;
        }

        public  void SetLoadPrevSession(bool isTrue)
        {
            LoadPreviousSession = isTrue;
        }

        public void SaveGameSession()
        {
            SavePlayerSession();
            SaveBattleSession();
            LoadPreviousSession = true;
        }

        public void LoadGameSession()
        {
            LoadPlayerSession();
            LoadBattleSession();
            LoadPreviousSession = true;
        }

        public LevelInfo GetCurrentLevelInfo()
        {
            return LevelInfo.Parse(levelDatas[playerInfoSession.totalLevelCount + 1]);
        }

        #region Leaderboard fucntion
        public void LoadLeaderboard()
        {
            LeaderboardManager.Instance.DownloadHighScores((strData) =>
            {
                UpdateLeaderboard(strData);
                OnLeaderboardDataLoaded();
            });
        }

        public void UpdateLeaderboard(string strData)
        {
            
            highscores.Clear();
            string[] rows = strData.Split('\n');
            for (int i = 0; i < rows.Length; i++)
            {
                if (!string.IsNullOrEmpty(rows[i]))
                {
                    highscores.Add(HighScoreModel.FromPipeDreamlo(rows[i]));
                }
               
            }
        }
        #endregion
    }
}

