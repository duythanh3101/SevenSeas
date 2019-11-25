using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureInstruction : MonoBehaviour
{
    public string GetInstruction(Vector3 currentTreasurePosition, Vector3 xSignPosition)
    {
        var distance = (xSignPosition - currentTreasurePosition).magnitude;
        if (distance > 7)
        {
            return CommonConstants.Instruction.WRONG_DIRECTION;
        }
        else if (distance > 5)
        {
            return CommonConstants.Instruction.IN_DIRECTION + CommonConstants.DirectionName.EAST_NAME;
        }
        else if (distance > 1)
        {
            return CommonConstants.Instruction.NEAR_YOU;
        }
        return string.Empty;
    }

    public string GetDirectionName(Vector3 currentTreasurePosition)
    {
        switch (currentTreasurePosition)
        {
            default:
                break;
        }
        return string.Empty;
    }
}
