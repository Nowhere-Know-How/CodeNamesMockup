using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeNames;
using UnityEngine.Events;

namespace CodeNames
{
    public class EventManager
    {
        public static OnGameStateChange onGameStateChange = new OnGameStateChange();
        public static OnGameStateChangeDone onGameStateChangeDone = new OnGameStateChangeDone();
        private EventManager(){}

        public static IEnumerator DelayInvoke(float delay, UnityEvent<GameState> e, GameState gs)
        {
            yield return new WaitForSeconds(delay);
            e.Invoke(gs);
        }
    }
}
