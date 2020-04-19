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
        public static OnGameStateControllerChange onGameStateControllerChange = new OnGameStateControllerChange();
        public static OnGameStateManagerDone onGameStateManagerDone = new OnGameStateManagerDone();

        public static OnCodeMasterSubmission onCodeMasterSubmission = new OnCodeMasterSubmission();
        public static OnTeamSubmission onTeamSubmission = new OnTeamSubmission();
        public static OnTeamSubmission onForwardedTeamSubmission = new OnTeamSubmission();

        private EventManager(){}

        public static IEnumerator DelayInvoke(float delay, UnityEvent<GameState> e, GameState gs)
        {
            yield return new WaitForSeconds(delay);
            e.Invoke(gs);
        }
    }
}
