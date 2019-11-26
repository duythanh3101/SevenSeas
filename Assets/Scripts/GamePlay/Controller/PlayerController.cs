using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGame;
using Assets.Scripts.Extensions.Utils;

namespace SevenSeas
{
    public class PlayerController : BoatController
    {

        void Awake()
        {
            ArrowController.OnArrowClicked += ArrowController_OnArrowClicked;
        }

      
        void OnDestroy()
        {
            ArrowController.OnArrowClicked -= ArrowController_OnArrowClicked;
        }

        void ArrowController_OnArrowClicked(Direction dir)
        {
            MoveAndRotate(dir);
        }

      

       
        
        
    }
}


