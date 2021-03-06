﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace CodeNames
{
    public class EventManager
    {
        public static OnGameStateChange onGameStateChange = new OnGameStateChange();
        public static OnGameStateControllerChange onGameStateControllerChange = new OnGameStateControllerChange();
        public static OnGameStateApiDone onGameStateApiDone = new OnGameStateApiDone();

        public static OnCodeMasterSubmission onCodeMasterSubmission = new OnCodeMasterSubmission();
        public static OnCodeMasterSubmission onForwardedCodeMasterSubmission = new OnCodeMasterSubmission();
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
