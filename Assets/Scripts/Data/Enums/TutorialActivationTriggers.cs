﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialActivationTriggers
{
    NONE,
    ELAPSED_GAME_MINUTE,
    ELAPSED_PLAY_TIME,
    WINDOW_OPENED,
    ENTITY_ATTRIBUTE_CHANGE,
    ENTITY_SELECTED,
    ROOM_DESIGNATED,
    ITEM_SPAWNED,
    AUTO_JOB_CREATED,
    BLUEPRINT_CHANGE
}