using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MainGame
{
    public class TreasureMap: MonoBehaviour
    {
        [SerializeField]
        private Vector2Int MapSize = new Vector2Int(10, 10);

        [SerializeField]
        private Transform tileHolder;

        [SerializeField]
        private XSign xSignPrefab;

        [SerializeField]
        private GameObject treasurePrefab;

        [SerializeField]
        private GameObject skullPrefab;

        [SerializeField]
        private int skullCount = 10;

        private GameObject treasure;

        private Vector3 currentTreasurePosition;

        private List<XSign> XSignList;
        private List<GameObject> SkullList;

        private static List<Vector3> allPositions;

        protected virtual void Awake()
        {
            InitMap();

            SpawnTreasureRandom();
            SpawnSkull();
            XSignList = new List<XSign>();
        }

        protected virtual void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Interact();
            }
        }

        private void Interact()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int col = Mathf.RoundToInt(mousePosition.x);
            int row = Mathf.RoundToInt(mousePosition.y);

            Vector3 posClick = new Vector3(col, row, 0);
            if (IsExistingTreasure(posClick))
            {
                treasure.SetActive(true);
                Debug.Log("Trung roi");
                return;
            }

            if (IsExistingSkull(posClick))
            {
                GameObject obj = SkullList.Where(x => x.transform.position == posClick).FirstOrDefault();
                if (obj != null)
                {
                    obj.SetActive(true);
                    Debug.Log("HAHAHAH");
                    return;
                }
            }

            if (!IsExistingXSignObjectAt(posClick) && !IsExistingTreasure(posClick))
            {
                int numberOfSkulls = GetAroundNumberOfSkulls(posClick);
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
            }
        }

        private int GetAroundNumberOfSkulls(Vector3 currentXSignPosition)
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

        private bool IsExistingSkull(Vector3 posClick)
        {
            return SkullList.Any(x => x.transform.position == posClick);
        }

        private bool IsExistingTreasure(Vector3 posClick)
        {
            return currentTreasurePosition == posClick;
        }

        private bool IsExistingXSignObjectAt(Vector3 posClick)
        {
            return XSignList.Any(x => x.transform.position == posClick);
        }

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

        private void SpawnTreasureRandom()
        {
            currentTreasurePosition = RandomPosition();
            treasure = Instantiate(treasurePrefab, currentTreasurePosition, Quaternion.identity);
            treasure.SetActive(false);
        }

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
    }
}