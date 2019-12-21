using BaseSystems.Observer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static SoundManager;
using Random = UnityEngine.Random;

namespace MainGame
{
    public class TreasureMap: MonoBehaviour
    {
        #region Private Properties
        [SerializeField] private Vector2Int MapSize = new Vector2Int(10, 10);
        [SerializeField] private Transform tileHolder;
        [SerializeField] private XSign xSignPrefab;
        [SerializeField] private GameObject treasurePrefab;
        [SerializeField] private GameObject skullPrefab;
        [SerializeField] private int skullCount = 10;
        [SerializeField] private Text instructionText;
        [SerializeField] private Sound clickSound;
        [SerializeField] private Sound treasureGameSound;
        [SerializeField] private Sound winGameSound;
        [SerializeField] private Sound loseGameSound;
        [SerializeField] private Sound failGameSound;

        private GameObject treasure;
        private Vector3 currentTreasurePosition;
        private List<XSign> XSignList;
        private List<GameObject> SkullList;
        private TreasureInstruction treasureInstruction;
        private bool isEndGame = false;

        private static List<Vector3> allPositions;
        #endregion Private Properties


        #region Mono Behaviour
        protected virtual void Awake()
        {
            Instance.PlayMusic(treasureGameSound);
            InitMap();

            SpawnTreasureRandom();
            SpawnSkull();
            XSignList = new List<XSign>();
            treasureInstruction = new TreasureInstruction();

            // Set text instruction is empty
            if (instructionText != null)
            {
                instructionText.text = string.Empty;
            }
        }

        protected virtual void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (isEndGame)
                {
                    SceneGameManager.Instance.LoadScene(CommonConstants.SceneName.CheckPointMapScene);
                    return;
                }
                Interact();
            }
        }
        #endregion Mono Behaviour

        #region Private Methods

        /// <summary>
        /// Handle interaction
        /// </summary>
        private void Interact()
        {   //Get position
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int col = Mathf.RoundToInt(mousePosition.x);
            int row = Mathf.RoundToInt(mousePosition.y);

            Vector3 posClick = new Vector3(col, row, 0);

            // Check valid of position click
            if (!IsClickPositionValidAt(posClick))
            {
                return;
            }
            Instance.PlaySound(clickSound);

            if (IsExistingTreasureAt(posClick))
            {
                LoseTreasureGame();
                instructionText.text = CommonConstants.Instruction.CLICK_TO_CONTINUE;
                return;
            }

            if (IsExistingSkullAt(posClick))
            {
                GameObject obj = SkullList.Where(x => x.transform.position == posClick).FirstOrDefault();
                if (obj != null)
                {
                    obj.SetActive(true);
                    LoseTreasureGame();

                    //display treasure
                    instructionText.text = CommonConstants.Instruction.CLICK_TO_CONTINUE;
                    return;
                }
            }

            if (!IsExistingXSignObjectAt(posClick) && !IsExistingTreasureAt(posClick))
            {
                int numberOfSkulls = GetNumberOfSkullsAroundAt(posClick);
                if (numberOfSkulls == 0)
                {
                    xSignPrefab.SetText(string.Empty);
                }
                else
                {
                    xSignPrefab.SetText(numberOfSkulls);
                }
                XSign xSign = Instantiate(xSignPrefab, new Vector3(col, row, 0), Quaternion.identity, tileHolder);

                XSignList.Add(xSign);
                instructionText.text = treasureInstruction.GetInstruction(currentTreasurePosition, posClick);
            }
        }

        private void LoseTreasureGame()
        {
            treasure.SetActive(true);
            isEndGame = true;

            Instance.StopMusic();
            Instance.PlaySound(loseGameSound);
            Instance.PlaySound(failGameSound);
        }

        /// <summary>
        /// Get number skull around position
        /// </summary>
        /// <param name="currentXSignPosition"></param>
        /// <returns></returns>
        private int GetNumberOfSkullsAroundAt(Vector3 currentXSignPosition)
        {
            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    var besidePos = new Vector3(currentXSignPosition.x + i, currentXSignPosition.y + j, 0);
                    if (SkullList.Any(x => x.transform.position == besidePos))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        #region Check condition
        /// <summary>
        /// is click position in map validation
        /// </summary>
        /// <param name="posClick"></param>
        /// <returns></returns>
        private bool IsClickPositionValidAt(Vector3 posClick)
        {
            return posClick.x < 5 && posClick.y < 5 && posClick.x > -5 && posClick.y > -5;
        }

        /// <summary>
        /// Is exist skull at position
        /// </summary>
        /// <param name="posClick"></param>
        /// <returns></returns>
        private bool IsExistingSkullAt(Vector3 posClick)
        {
            return SkullList.Any(x => x.transform.position == posClick);
        }

        /// <summary>
        /// Is exist treasure at position
        /// </summary>
        /// <param name="posClick"></param>
        /// <returns></returns>
        private bool IsExistingTreasureAt(Vector3 posClick)
        {
            return currentTreasurePosition == posClick;
        }

        /// <summary>
        /// Is exist XSign at position
        /// </summary>
        /// <param name="posClick"></param>
        /// <returns></returns>
        private bool IsExistingXSignObjectAt(Vector3 posClick)
        {
            return XSignList.Any(x => x.transform.position == posClick);
        }
        #endregion Check condition

        /// <summary>
        /// Init map, add vector to all tile of map
        /// </summary>
        private void InitMap()
        {
            allPositions = new List<Vector3>();

            for (int i = -4; i < MapSize.x / 2; i++)
            {
                for (int j = -4; j < MapSize.y / 2; j++)
                {
                    allPositions.Add(new Vector3(i, j, 0));
                }
            }
        }

        /// <summary>
        /// Spawn treasure random on map
        /// </summary>
        private void SpawnTreasureRandom()
        {
            currentTreasurePosition = RandomPosition();
            treasure = Instantiate(treasurePrefab, currentTreasurePosition, Quaternion.identity);
            treasure.SetActive(false);
        }

        /// <summary>
        /// Spawn skull in map
        /// </summary>
        private void SpawnSkull()
        {
            SkullList = new List<GameObject>();

            for (int i = 0; i < skullCount; i++)
            {
                Vector3 posRandom = RandomPosition();
                GameObject obj = Instantiate(skullPrefab, posRandom, Quaternion.identity);
                obj.SetActive(false);
                SkullList.Add(obj);
            }
        }

        /// <summary>
        /// RandomPosition returns a random position from our list gridPositions.
        /// </summary>
        /// <returns></returns>
        private Vector3 RandomPosition()
        {
            int randomIndex = Random.Range(0, allPositions.Count);

            Vector3 randomPosition = allPositions[randomIndex];

            allPositions.RemoveAt(randomIndex);

            return randomPosition;
        }
        #endregion Private Methods
    }
}