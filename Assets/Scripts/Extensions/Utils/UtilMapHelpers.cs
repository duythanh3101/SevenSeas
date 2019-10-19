using UnityEngine;

namespace Assets.Scripts.Extensions.Utils
{
    public class UtilMapHelpers
    {
        public static Vector2 CalculateCellSize(SpriteRenderer backgroundSprite, GameObject backgroundMap)
        {
            //Calculate the acttual size of the background
            var backgroundSize = new Vector2(
                (backgroundSprite.sprite.rect.size.x / backgroundSprite.sprite.pixelsPerUnit) * backgroundMap.transform.lossyScale.x,
                (backgroundSprite.sprite.rect.size.y / backgroundSprite.sprite.pixelsPerUnit) * backgroundMap.transform.lossyScale.y
                );

            //Calculate the size of the cell
            return new Vector2(
                backgroundSize.x / Mathf.Sqrt(CommonConstants.NUMBER_OF_CELLS),
                backgroundSize.y / Mathf.Sqrt(CommonConstants.NUMBER_OF_CELLS)
                );
        }

        public static int GetHorizontalSign(int col,int centerNumber)
        {
            return (col <= (centerNumber - 1) ? -1 : 1);
        }

        public static int GetVerticalSign(int row,int centerNumber)
        {
            return (row <= (centerNumber - 1) ? -1 : 1);
        }

        public static Vector2 GetDirectionVector(Direction direction)
        {
            switch (direction)
            {
                case Direction.South:
                    return CommonConstants.SOUTH_VECTOR;
                case Direction.East:
                    return CommonConstants.EAST_VECTOR;
                case Direction.West:
                    return CommonConstants.WEST_VECTOR;
                case Direction.North:
                    return CommonConstants.NORTH_VECTOR;
                case Direction.NorthEast:
                    return CommonConstants.NORTH_EAST_VECTOR;
                case Direction.NorthWest:
                    return CommonConstants.NORTH_WEST_VECTOR;
                case Direction.SouthEast:
                    return CommonConstants.SOUTH_EAST_VECTOR;
                case Direction.SouthWest:
                    return CommonConstants.SOUTH_WEST_VECTOR;
            }
            return Vector2.zero;
        }

        public static float GetDirectionAngle(Direction direction)
        {
            switch (direction)
            {
                case Direction.South:
                    return 270;
                case Direction.East:
                    return 0;
                case Direction.West:
                    return 180;
                case Direction.North:
                    return 90;
                case Direction.NorthEast:
                    return 45;
                case Direction.NorthWest:
                    return 135;
                case Direction.SouthEast:
                    return 315;
                case Direction.SouthWest:
                    return 225;
                default:
                    return 0;
            }
        }
    }
}
