using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SevenSeas
{
    

    public class PlayerInfoManager : MonoBehaviour
    {
        public static PlayerInfoManager Instance = null;

        [System.Serializable]
        public class PlayerInfoSession
        {
            public int playerScore;
            public int piratesSunk;
            public int checkPoint;
            public int treasureFound;
            public int playerHealth;

            public void ClearData()
            {

            }

            public void SetData(int pHealth)
            {
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
        }

        void Start()
        {
            playerInfoSession.SetData(maxPlayerHealth);
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

        }
       

        public void ClearPlayerSession()
        {
            playerInfoSession.ClearData();
        }

        public void UpdatePlayerHealth(int pHealth)
        {
            playerInfoSession.playerHealth = pHealth;
        }
    }
}

