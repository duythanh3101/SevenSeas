using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureInstruction : MonoBehaviour
{
    public string GetInstruction(Vector3 currentTreasurePosition, Vector3 xSignPosition)
    {
        var distance = (xSignPosition - currentTreasurePosition).magnitude;
        if (distance >= 3)
        {
            return CommonConstants.Instruction.IN_DIRECTION + GetDirectionName(currentTreasurePosition, xSignPosition);
        }
        else if (distance >= 1)
        {
            return CommonConstants.Instruction.NEAR_YOU;
        }
        return string.Empty;
    }

    public string GetDirectionName(Vector3 currentTreasurePosition, Vector3 xSignPosition)
    {
        float x  =currentTreasurePosition.x;
        float y = currentTreasurePosition.y;
        float xSignPos = xSignPosition.x;
        float ySignPos = xSignPosition.y;

        string directionName = string.Empty;

        if (x > xSignPos && y > ySignPos)
        {
            directionName = CommonConstants.DirectionName.NORTH_EAST_NAME;
        }
        else if (x > xSignPos && y < ySignPos)
        {
            directionName = CommonConstants.DirectionName.SOUTH_EAST_NAME;
        }
        else if (x < xSignPos && y > ySignPos)
        {
            directionName = CommonConstants.DirectionName.NORTH_WEST_NAME;
        }
        else if (x < xSignPos && y < ySignPos)
        {
            directionName = CommonConstants.DirectionName.SOUTH_WEST_NAME;
        }
        else if (x == xSignPos && y > ySignPos)
        {
            directionName = CommonConstants.DirectionName.NORTH_NAME;
        }
        else if (x == xSignPos && y < ySignPos)
        {
            directionName = CommonConstants.DirectionName.SOUTH_WEST_NAME;
        }
        else if (x > xSignPos && y == ySignPos)
        {
            directionName = CommonConstants.DirectionName.EAST_NAME;
        }
        else if (x < xSignPos && y == ySignPos)
        {
            directionName = CommonConstants.DirectionName.WEST_NAME;
        }

        return directionName;
    }
}
