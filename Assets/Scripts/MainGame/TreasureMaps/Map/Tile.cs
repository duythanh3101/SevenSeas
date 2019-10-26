using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MainGame
{
    public class Tile : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private Vector2Int coordinate;

        /// <summary>
        /// Tọa độ trong bàn cờ.
        /// </summary>
        public Vector2Int Coordinate
        {
            get { return coordinate; }
            set { coordinate = value; }
        }

        public XSign OnTileXSign { get; set; }
       
        public void OnPointerClick(PointerEventData eventData)
        {
            if (OnTileXSign != null)
            {
                Debug.Log(OnTileXSign.gameObject.transform.position + " clicked.");
            }
            else
            {
                Debug.Log("There is no xSign in this tile.");
            }
        }
    }
}
