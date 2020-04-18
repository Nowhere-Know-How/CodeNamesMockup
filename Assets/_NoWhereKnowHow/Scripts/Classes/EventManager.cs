using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeNames;

namespace CodeNames
{
    public class EventManager
    {
        private EventManager(){}
        public static OnGameStateChange onGameStateChange = new OnGameStateChange();

        // public static EventManager Instance
        // {
        //     get
        //     {
        //         if (EventManager == null)
        //         {
        //             EventManager.instance = new EventManager();
        //         }
        //         return EventManager.instance;
        //     }
        // }

    }
}
