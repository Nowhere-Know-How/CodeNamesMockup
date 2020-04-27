using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class EventManagerClient
{
    public static OnAssignedPlayerOwnership onAssignedPlayerOwnership = new OnAssignedPlayerOwnership();

    //public static List<OnCardsChanged> onCardsChangedList = Enumerable.Repeat(new OnCardsChanged(), 25).ToList();
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

    private EventManagerClient() { }

    public static IEnumerator DelayInvoke(float delay, UnityEvent e)
    {
        yield return new WaitForSeconds(delay);
        e.Invoke();
    }
}
