using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGame.Arrows
{
    [Serializable]
    public class Arrow
    {
        public SpriteRenderer arrowSprite;

        public Direction direction;

        public Arrow(SpriteRenderer arrowSprite, Direction direction)
        {
            this.arrowSprite = arrowSprite;
            this.direction = direction;
        }
    }
}