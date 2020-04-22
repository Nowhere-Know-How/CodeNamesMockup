using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamCardSubmission
{
    int cardIndex;
    CardColor teamColor;

    public int CardIndex
    {
        get { return cardIndex; }
    }
    public CardColor TeamColor
    {
        get { return teamColor; }
    }

    public TeamCardSubmission(int index, CardColor team)
    {
        cardIndex = index;
        teamColor = team;
    }

    public string ToString()
    {
        return "Team Color: " + teamColor.ToString() + ", CardIndex: " + cardIndex.ToString();
    }
}
