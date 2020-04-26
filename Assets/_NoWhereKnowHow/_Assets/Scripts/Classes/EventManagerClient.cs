using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManagerClient
{
    public static OnAssignedPlayerOwnership onAssignedPlayerOwnership = new OnAssignedPlayerOwnership();

    public static List<OnCardsChanged> onCardsChangedList = new List<OnCardsChanged> { 
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged(),
        new OnCardsChanged()
    };

    //public static OnCardsChanged onCardsChanged = new OnCardsChanged();

    private EventManagerClient() { }

    public static IEnumerator DelayInvoke(float delay, UnityEvent e)
    {
        yield return new WaitForSeconds(delay);
        e.Invoke();
    }
}
