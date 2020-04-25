using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EventManagerClient
{
    public static OnAssignedPlayerOwnership onAssignedPlayerOwnership = new OnAssignedPlayerOwnership();

    private EventManagerClient() { }

    public static IEnumerator DelayInvoke(float delay, UnityEvent e)
    {
        yield return new WaitForSeconds(delay);
        e.Invoke();
    }
}
