using BaseSystems.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

<<<<<<< HEAD
namespace SevenSeas
{
    public class UIManager : MonoBehaviour
    {
=======
public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private GameObject treasureGameOverPanel;
    
    // Start is called before the first frame update
    void Start()
    {
            
    }
>>>>>>> origin/thanhdp

        public static UIManager Instance = null;

        [SerializeField]
        private Image[] healthImages;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                DestroyImmediate(gameObject);
        }

        void Start()
        {
            InitValues();
        }

        void InitValues()
        {
            int healthCount = GameManager.Instance.playerController.playerHealth;
            for (int i = 0; i < healthCount; i++)
            {
                healthImages[i].gameObject.SetActive(true);
            }
        }

        public void DecreaseHealth(int index)
        {
            healthImages[index].gameObject.SetActive(false);
        }
    }

<<<<<<< HEAD
=======
    public void ShowFindTreasureGameOver()
    {
        treasureGameOverPanel.SetActive(true);
    }
>>>>>>> origin/thanhdp
}
