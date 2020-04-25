using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeNames;

namespace CodeNames
{
    public class TeamCardSubmission
    {
        CardChoice cardIndex;
        CardColor teamColor;

        public CardChoice CardIndex
        {
            get { return cardIndex; }
        }
        public CardColor TeamColor
        {
            get { return teamColor; }
        }

        public TeamCardSubmission(CardChoice index, CardColor team)
        {
            cardIndex = index;
            teamColor = team;
        }

        public override string ToString()
        {
            return "Team Color: " + teamColor.ToString() + ", CardIndex: " + cardIndex.ToString();
        }
    }
}
