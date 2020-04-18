namespace CodeNames
{
    public enum GameState
    {
        INIT,
        INIT_DONE,
        PICK_TEAMS,
        PICK_TEAMS_DONE,
        WAIT_FOR_TEAMS_TO_MEET_EACH_OTHER,
        WAIT_FOR_TEAMS_TO_MEET_EACH_OTHER_DONE_BLUE_TO_START,
        WAIT_FOR_TEAMS_TO_MEET_EACH_OTHER_DONE_RED_TO_START,
        BLUE_TEAM_TURN_START,
        RED_TEAM_TURN_START,
        RESOLVE, 
        END,
        IDLE
    }
}
