using System.Text;
using UnityEngine;

public static class GameConstants
{
    #region AUDIO

    public const string AUDIO_MUSIC_BATTLE_INTRO = "audio_music_battle_intro";
    public const string AUDIO_MUSIC_BATTLE_LOOP = "audio_music_battle_loop";
    
    public const string AUDIO_MUSIC_OVERWORLD_THEME_INTRO = "audio_music_overworld_intro";
    public const string AUDIO_MUSIC_OVERWORLD_THEME_LOOP = "audio_music_overworld_loop";

    public const string AUDIO_MUSIC_ODDREALM_THEME_INTRO = "audio_music_main_theme_intro";
    public const string AUDIO_MUSIC_ODDREALM_THEME_LOOP = "audio_music_main_theme_loop";

    public const string AUDIO_SFX_TEXT_WIDGET = "audio_sfx_snes_echo_01";
    public const string AUDIO_SFX_STINGER_GAME_OVER = "audio_sfx_game_over";
    #endregion

    #region DISCOVERIES

    public const string DISCOVERIES_DEFAULT_CAVES_DATA_ID = "discovery_caves_treasure_easy";

    #endregion DISCOVERIES

    #region SEARCH REQUESTS

    public const string SEARCH_REQ_ATTACK_TASKS = "request_item";

    #endregion SEARCH REQUESTS

    #region FACTION

    public const string FACTION_PLAYER_ID = "faction_player";
    public const string FACTION_NEUTRAL_ID = "faction_npc_neutral";
    public const string FACTION_HOSTILE_ID = "faction_npc_hostile";

    #endregion FACTION

    #region LOGIC

    public const string CONDITION_DISCARD_INFO_AVAILABLE_ID = "discard_info_available";
    public const string CONDITION_ITEMS_OWNED_ID = "items_owned";
    public const string CONDITION_OVERWORLD_LOADOUT_ID = "overworld_loadout";
    public const string CONDITION_ENTITIES_KILLED_ID = "entities_killed";
    public const string CONDITION_ENTITY_EXISTS_ID = "entity_exists";
    public const string CONDITION_GLOBAL_INT_VARIABLE_ID = "global_int_variable";

    public const string ACTION_DESTROY_ITEMS_ID = "destroy_items";
    public const string ACTION_SPAWN_ENTITY_ID = "spawn_entities";
    public const string ACTION_GENERATE_MODEL_PREFABS_ID = "gen_model_prefabs";
    public const string ACTION_PLAY_SOUND_TYPE_ID = "play_sound";
    public const string ACTION_CHANGE_SKYLIGHT_ID = "change_skylight";
    public const string ACTION_SET_ENTITY_TAG_ID = "set_entity_tag";
    public const string ACTION_SPAWN_ITEM_ID = "spawn_items";
    public const string ACTION_COLLECT_ID = "collect";
    public const string ACTION_CHANGE_FACTION_ID = "change_faction";
    public const string ACTION_LEAVE_MAP_ID = "leave_map";
    public const string ACTION_START_DIALOGUE_ID = "start_dialogue";
    public const string ACTION_START_MERCHANT_ID = "start_merchant";
    public const string ACTION_MOVE_CAMERA_ID = "move_camera";
    public const string ACTION_ADD_SCENARIO_TAG_ID = "add_scenario_tag";

    // TAGS.
    public const string INIT_ENTITIES = "init_entities";
    public const string COLLECTED_ENTITIES = "collected_entities";
    public const string COLLECTED_ITEMS = "collected_items";
    public const string COLLECTED_BLOCKS = "collected_blocks";
    public const string SETTLERS = "settlers";
    public const string SPAWNED_BLOCKS = "spawned_blocks";
    public const string SPAWNED_BLOCKS_NOT_OBSTRUCTION = "spawned_blocks_not_obstruction";
    public const string SPAWNED_ENTITIES = "spawned_entities";
    public const string SPAWNED_ITEMS = "spawned_items";
    public const string RANDOM_SURFACE_POINT = "random_surface_point";
    public const string RANDOM_MAP_EDGE_GROUND = "random_map_edge_ground";
    public const string RANDOM_MAP_EDGE_AIR = "random_map_edge_air";
    public const string RANDOM_MAP_EDGE_WATER = "random_map_edge_water";
    public const string RANDOM_MAP_EDGE_GROUND_OR_WATER = "random_map_edge_ground_or_water";
    public const string RANDOM_MAP_EDGE_UNDER_WATER = "random_map_edge_underwater";
    public const string LAST_ENTITY_SPAWNED = "last_entity_spawned";
    public const string RANDOM_INIT_POINT = "random_init_block_point";
    public const string RANDOM_NEW_DISCOVERED_BLOCK = "random_new_discovered_block";
    public const string RANDOM_MAP_EDGE_UNLIT = "random_map_edge_unlit";

    #endregion LOGIC

    #region QUEST TRIGGERS

    public const string QUEST_TRIGGER_ADULT_ID = "quest_trigger_adult";

    #endregion QUEST TRIGGERS

    #region QUESTS

    public const string QUEST_CHOOSE_PROFESSION_ID = "quest_choose_profession";

    #endregion QUESTS

    #region ENTITY INDICATORS

    public const string ENTITY_INDICATOR_PREFAB_PATH = "Gameplay/pf_entity_indicator_icon";

    public const string ENTITY_INDICATOR_HOSTILE_THREAT = "indicator_hostile";
    public const string ENTITY_INDICATOR_IMPORTANT = "indicator_important";
    public const string ENTITY_INDICATOR_THREAT = "indicator_threat";
    public const string ENTITY_INDICATOR_DEAD = "indicator_killed";
    public const string ENTITY_INDICATOR_FLEE = "indicator_flee";
    public const string ENTITY_INDICATOR_LEVEL_UP = "indicator_level_up";
    public const string ENTITY_INDICATOR_RECOVER = "indicator_recover";
    public const string ENTITY_INDICATOR_LOVE = "indicator_love";
    public const string ENTITY_INDICATOR_TUTORIAL = "indicator_tutorial";
    public const string ENTITY_INDICATOR_DIALOGUE = "indicator_dialogue";

    #endregion ENTITY INDICATORS

    #region ERRORS

    public const string ERROR_MISSING_BLUEPRINT = "BP Unknown";
    public const string ERROR_MISSING_ITEM = "Item Unknown";
    public const string ERROR_MISSING_PLANT = "Plant Unknown";
    public const string ERROR_MISSING_ENTITY = "Entity Unknown";
    public const string ERROR_MISSING_BLOCK = "Block Unknown";
    public const string ERROR_MISSING_TOOLTIP = "Tooltip Unknown";
    public const string ERROR_MISSING_SKILL = "Skill Unknown";
    public const string ERROR_MISSING_ZONE = "Zone Unknown";
    public const string ERROR_MISSING_ROOM = "Room Unknown";
    public const string ERROR_MISSING_UTILITY = "Utility Unknown";

    #endregion ERRORS

    #region BEHAVIOURS

    public const string BEHAVIOUR_MILITARY_WORKER_ID = "behaviour_military_worker";
    public const string BEHAVIOUR_WORKSTATION_WORKER_ID = "behaviour_workstation_worker";
    public const string BEHAVIOUR_BLOCK_WORKER_ID = "behaviour_block_worker";

    public const string BEHAVIOUR_STEALTHED_ENTITY_SEARCH_ID = "behaviour_stealthed_entity_search";
    public const string BEHAVIOUR_DESTROY_ITEMS_ID = "behaviour_destroy_items";
    public const string BEHAVIOUR_EXTINGUISH_FIRE_ID = "behaviour_extinguish_fire";
    public const string BEHAVIOUR_HUNT_ID = "behaviour_hunt";
    public const string BEHAVIOUR_COMPANION_ID = "behaviour_companion";
    public const string BEHAVIOUR_COMPANION_SEARCH_ID = "behaviour_companion_search";
    public const string BEHAVIOUR_CREATURE_BASE_ID = "behaviour_creature_base";
    public const string BEHAVIOUR_ELDER_ID = "behaviour_elder";
    public const string BEHAVIOUR_ANCIENT = "behaviour_ancient";
    public const string BEHAVIOUR_ARDYN = "behaviour_ardyn";
    public const string BEHAVIOUR_VOID_SICKNESS = "behaviour_void_sickness";
    public const string BEHAVIOUR_VOID_WOKEN = "behaviour_void_woken";
    public const string BEHAVIOUR_VOID_IMP = "behaviour_void_imp";
    public const string BEHAVIOUR_ENERGY_BEING = "behaviour_energy_being";
    public const string BEHAVIOUR_BIRD_ID = "behaviour_bird";
    public const string BEHAVIOUR_CHILD_ID = "behaviour_child";
    public const string BEHAVIOUR_DAILY_ITEM_SPAWN_ID = "behaviour_daily_item_spawn";
    public const string BEHAVIOUR_SHIMMER_ID = "behaviour_shimmer";
    public const string BEHAVIOUR_KOOSH_KASHEEN_ID = "behaviour_koosh_kasheen";
    public const string BEHAVIOUR_STEALTH_ID = "behaviour_stealth";
    public const string BEHAVIOUR_TREE_SPAWN_ID = "behaviour_tree_spawn";
    public const string BEHAVIOUR_WANT_FURNITURE_ID = "behaviour_wants_furniture";
    public const string BEHAVIOUR_WANTS_ROOM_ID = "behaviour_wants_room";
    public const string BEHAVIOUR_WANTS_ROOF_ID = "behaviour_wants_roof";
    public const string BEHAVIOUR_WANTS_SLEEP_ON_BED_ID = "behaviour_wants_sleep_on_bed";
    public const string BEHAVIOUR_SICK_ID = "behaviour_sick";
    public const string BEHAVIOUR_HUNGRY_ID = "behaviour_hungry";
    public const string BEHAVIOUR_STARVING_ID = "behaviour_starving";
    public const string BEHAVIOUR_THIRSTY_ID = "behaviour_thirsty";
    public const string BEHAVIOUR_FREEZING_ID = "behaviour_freezing";
    public const string BEHAVIOUR_WARMING_UP_ID = "behaviour_warming_up";
    public const string BEHAVIOUR_DEAD_ID = "behaviour_dead";
    public const string BEHAVIOUR_UNHAPPY_ID = "behaviour_unhappy";
    public const string BEHAVIOUR_WOUNDED_BREAK_ID = "behaviour_wounded_break";
    public const string BEHAVIOUR_WOUNDED_BLEEDING_ID = "behaviour_wounded_bleeding";
    public const string BEHAVIOUR_WOUNDED_DROWNING_ID = "behaviour_wounded_drowning";
    public const string BEHAVIOUR_TIRED_ID = "behaviour_tired";
    public const string BEHAVIOUR_WELL_FED_ID = "behaviour_well_fed";
    public const string BEHAVIOUR_SKILL_LOG_ID = "behaviour_skill_log";
    public const string BEHAVIOUR_SKILL_CRAFT_WOOD_ID = "behaviour_skill_craft_wood";
    public const string BEHAVIOUR_SKILL_CRAFT_STONE_ID = "behaviour_skill_craft_stone";
    public const string BEHAVIOUR_SKILL_CRAFT_METAL_ID = "behaviour_skill_craft_metal";
    public const string BEHAVIOUR_SKILL_COOK_ID = "behaviour_skill_cook";
    public const string BEHAVIOUR_SKILL_FISH_ID = "behaviour_skill_fish";
    public const string BEHAVIOUR_SKILL_DECONSTRUCT_ID = "behaviour_skill_deconstruct";
    public const string BEHAVIOUR_SKILL_HARVEST_ID = "behaviour_skill_harvest";
    public const string BEHAVIOUR_SKILL_CARRY_ID = "behaviour_skill_carry";
    public const string BEHAVIOUR_SKILL_LOOT_ID = "behaviour_skill_loot";
    public const string BEHAVIOUR_SKILL_CUT_ID = "behaviour_skill_cut";
    public const string BEHAVIOUR_SKILL_PLANT_ID = "behaviour_skill_plant";
    public const string BEHAVIOUR_SKILL_FORESTRY_ID = "behaviour_skill_forestry";
    public const string BEHAVIOUR_SKILL_FIGHT_MELEE_ID = "behaviour_skill_fight_melee";
    public const string BEHAVIOUR_SKILL_FIGHT_RANGE_ID = "behaviour_skill_fight_range";
    public const string BEHAVIOUR_SKILL_FIGHT_MAGIC_ID = "behaviour_skill_fight_magic";
    public const string BEHAVIOUR_SKILL_CRAFT_VOID_ID = "behaviour_skill_craft_void";
    public const string BEHAVIOUR_SKILL_CRAFT_CLOTH_ID = "behaviour_skill_craft_cloth";
    public const string BEHAVIOUR_SKILL_CRAFT_LEATHER_ID = "behaviour_skill_craft_leather";
    public const string BEHAVIOUR_SKILL_EVOCATION_ID = "behaviour_skill_evocation";
    public const string BEHAVIOUR_SKILL_TAME_ANIMAL_ID = "behaviour_skill_tame_animal";
    public const string BEHAVIOUR_SKILL_SURVIVAL_ID = "behaviour_skill_survival";
    public const string BEHAVIOUR_SKILL_MINE_ID = "behaviour_skill_mine";
    public const string BEHAVIOUR_SKILL_HEAL_ID = "behaviour_skill_heal";
    public const string BEHAVIOUR_ROOM_OWNER_ID = "behaviour_room_owner";
    public const string BEHAVIOUR_TAVERN_OCCUPANT_ID = "behaviour_tavern_occupant";
    public const string BEHAVIOUR_PRISON_CELL_OCCUPANT_ID = "behaviour_prison_cell_occupant";
    public const string BEHAVIOUR_ANIMAL_ENCLOSURE_OCCUPANT_ID = "behaviour_animal_enclosure_occupant";
    public const string BEHAVIOUR_AUTO_ROOM_JOIN_ID = "behaviour_auto_room_join";
    public const string BEHAVIOUR_TAMEABLE_ID = "behaviour_tameable";
    public const string BEHAVIOUR_TAMED_ID = "behaviour_tamed";
    public const string BEHAVIOUR_EQUIP_ITEMS_ID = "behaviour_equip_items";
    public const string BEHAVIOUR_HOSTILE_ID = "behaviour_hostile";
    public const string BEHAVIOUR_COMBAT_ID = "behaviour_combat";
    public const string BEHAVIOUR_TERRIFIED_ID = "behaviour_terrified";
    public const string BEHAVIOUR_ROAMING_ANIMAL_ID = "behaviour_roaming_animal"; // Randomly leave map, and inform other migrating animals of same race to do so too.
    public const string BEHAVIOUR_KIDNAPPER_ID = "behaviour_kidnapper";
    public const string BEHAVIOUR_SABOTEUR_ID = "behaviour_saboteur";
    public const string BEHAVIOUR_AUTO_EQUIP_ID = "behaviour_auto_equip";
    public const string BEHAVIOUR_BODY_TEMPERATURE_ID = "behaviour_body_temperature";
    public const string BEHAVIOUR_HUNGER_DECAY_ID = "behaviour_hunger_decay";
    public const string BEHAVIOUR_HAPPINESS_DECAY_ID = "behaviour_happiness_decay";
    public const string BEHAVIOUR_THIRST_DECAY_ID = "behaviour_thirst_decay";
    public const string BEHAVIOUR_FOOD_SEARCH_ID = "behaviour_food_search";
    public const string BEHAVIOUR_WATER_SEARCH_ID = "behaviour_water_search";
    public const string BEHAVIOUR_HEAT_SEARCH_ID = "behaviour_heat_search";
    public const string BEHAVIOUR_FOOD_SEARCH_IN_ROOM_ID = "behaviour_food_search_in_room";
    public const string BEHAVIOUR_WATER_SEARCH_IN_ROOM_ID = "behaviour_water_search_in_room";
    public const string BEHAVIOUR_STOCK_ROOM_ID = "behaviour_stock_room";
    public const string BEHAVIOUR_RELAX_SEARCH_ID = "behaviour_relax_search";
    public const string BEHAVIOUR_RELAXED_ID = "behaviour_relaxed";

    // BEHAVIOUR DESCRIPTIONS
    public const string BEHAVIOUR_TAMEABLE_DEFAULT_DESCRIPTION = "Can be tamed.";

    // COMPANION / COMPANION SEARCH BEHAVIOUR
    public const int BEHAVIOUR_COMPANION_ATTRACTION_INC_MIN = 0;
    public const int BEHAVIOUR_COMPANION_ATTRACTION_INC_MAX = 15;
    public const int BEHAVIOUR_COMPANION_ATTRACTION_THRESHOLD_MIN = 50;
    public const int BEHAVIOUR_COMPANION_ATTRACTION_THRESHOLD_MAX = 100;
    public const int BEHAVIOUR_CHANCE_TO_BECOME_ATTRACTED_TO = 25;
    public const int BEHAVIOUR_CHANCE_FOR_CHILD_PER_DAY = 100;
    public const int BEHAVIOUR_CHANCE_FOR_CHILD_PER_DAY_FAIL_MOD = 1;
    public const int BEHAVIOUR_MAX_CHILDREN_FROM_ENTITY = 5;
    public const string BEHAVIOUR_COMPANION_DECEASED_DESCRIPTION = "Companion has died.";

    // HUMAN BEHAVIOUR
    public const int BEHAVIOUR_HUMAN_HOURLY_CHANCE_TO_COMPANION_SEARCH = 50; // 5%
    public const int BEHAVIOUR_HUMAN_MIN_HOURS_TO_BECOME_TIRED = 24;
    public const int BEHAVIOUR_HUMAN_CHANCE_TO_GET_TIRED = 100; // x / 1000
    public const int BEHAVIOUR_HUMAN_MIN_HOURS_TO_BECOME_HUNGRY = 48;
    public const int BEHAVIOUR_HUMAN_CHANCE_TO_GET_HUNGRY = 100; // x / 1000

    // HUNGRY BEHAVIOUR
    public const int BEHAVIOUR_ALIVE_MIN_HOURS_TO_BECOME_STARVED = 96;
    public const int BEHAVIOUR_ALIVE_CHANCE_TO_GET_STARVED = 100; // x / 1000

    // STARVED BEHAVIOUR
    public const int BEHAVIOUR_ALIVE_MIN_HOURS_TO_DIE = 24;
    public const int BEHAVIOUR_ALIVE_CHANCE_TO_DIE = 100; // x / 1000
    
    // WOUNDED BLEEDING BEHAVIOUR
    public const int BEHAVIOUR_WOUNDED_BLEEDING_CHANCE_TO_DIE_BY_HOUR = 20; // +2% chance to die
    public const int BEHAVIOUR_WOUNDED_BLEEDING_BASE_KILL_CHANCE = 200; // +20% chance to die
    public const int BEHAVIOUR_WOUNDED_BLEEDING_BASE_RECOVER_CHANCE = 750; // +75% chance to recover

    // WOUNDED BREAK BEHAVIOUR
    public const int BEHAVIOUR_WOUNDED_BREAK_CHANCE_TO_DIE_BY_HOUR = 5; // +0.5% chance to die
    public const int BEHAVIOUR_WOUNDED_BREAK_BASE_KILL_CHANCE = 250; // +10% chance to die

    // TIRED BEHAVIOUR
    public const int BEHAVIOUR_TIRED_MIN_HOUR_TO_REST = 2;
    public const int BEHAVIOUR_TIRED_MAX_HOUR_TO_REST = 3;
    public const int BEHAVIOUR_TIRED_CHANCE_TO_READY_FOR_SLEEP = 200; // x / 1000

    #endregion BEHAVIOURS

    #region COMBAT

    // SLASHING
    public const int COMBAT_SLASHING_WEAPON_CHANCE_TO_WOUND_BY_STRENGTH = 10; // +10% chance to wound per weapon strength
    //public const int COMBAT_SLASHING_WEAPON_CHANCE_TO_WOUND_BASE = 450; // +45% chance to wound per weapon strength

    // BLUNT
    public const int COMBAT_BLUNT_WEAPON_CHANCE_TO_WOUND_BY_STRENGTH = 5; // +10% chance to wound per weapon strength
    //public const int COMBAT_BLUNT_WEAPON_CHANCE_TO_WOUND_BASE = 100; // +10% chance to wound per weapon strength
    
    // PIERCING
    public const int COMBAT_PIERCING_WEAPON_CHANCE_TO_WOUND_BY_STRENGTH = 10; // +10% chance to wound per weapon strength
    //public const int COMBAT_PIERCING_WEAPON_CHANCE_TO_WOUND_BASE = 600; // +60% chance to wound per weapon strength

    #endregion

    #region NOTIFICATIONS

    public const string NOTIF_MISSING_ROOM_DIET_ITEMS = "notif_entity_missing_room_diet_items";
    public const string NOTIF_ENTITY_DIED = "notif_entity_died";
    public const string NOTIF_ENTITY_HUNGRY = "notif_entity_hungry";
    public const string NOTIF_ENTITY_STARVING = "notif_entity_starving";
    public const string NOTIF_NEW_BLUEPRINT = "notif_new_blueprint";
    public const string NOTIF_NEW_UNION = "notif_new_union";
    public const string NOTIF_NEW_CHILD = "notif_new_child";
    public const string NOTIF_ENTITY_ADULT = "notif_entity_adult";
    public const string NOTIF_ENTITY_KIDNAPPED = "notif_entity_kidnapped";
    public const string NOTIF_WORK = "notif_entity_work";
    public const string NOTIF_COMBAT = "notif_entity_combat";
    public const string NOTIF_FISHING = "notif_entity_fishing";
    public const string NOTIF_BLUEPRINT_NEW = "notif_new_blueprint";
    public const string NOTIF_FISH_NEW = "notif_new_fish";
    public const string NOTIF_LEVEL_UP = "notif_level_up";
    public const string NOTIF_NEW_ELDER = "notif_new_elder";
    public const string NOTIF_ENTITY_CAPTURED = "notif_entity_captured";
    public const string NOTIF_ENTITY_CAPTURE_FAILED = "notif_entity_capture_failed";
    public const string NOTIF_ENTITY_CAPTURE_SUCCESS = "notif_entity_capture_success";
    public const string NOTIF_ENTITY_INTEGRATE_SUCCESS = "notif_entity_integrate_success";
    public const string NOTIF_ENTITY_LEAVING = "notif_entity_leaving";
    public const string NOTIF_ENTITY_SIGHTED = "notif_entity_sighted";
    public const string NOTIF_ENTITY_STUCK = "notif_entity_stuck";
    public const string NOTIF_ENTITY_NO_PROFESSION = "notif_entity_no_profession";
    public const string NOTIF_TRACKED_STAT_LOW = "notif_tracked_stat_low";
    public const string NOTIF_LOW_ON_BEVERAGES = "notif_low_beverages";
    public const string NOTIF_ENTITY_CANNOT_REACH_JOB = "notif_entity_cannot_reach_job";
    public const string NOTIF_ENTITY_CANNOT_REACH_JOB_ITEM = "notif_entity_cannot_reach_job_item";
    public const string NOTIF_THREAT_INCOMING = "notif_threat_incoming";
    public const string NOTIF_BANDITS_ARRIVED = "notif_bandits_arrived";
    public const string NOTIF_OCCUPANT_ADDED_TO_ROOM = "notif_occupant_added_to_room";
    public const string NOTIF_OCCUPANT_REMOVED_FROM_ROOM = "notif_occupant_removed_from_room";


    #endregion NOTIFICATIONS

    #region BLOCKS

    public const string BLOCK_DIRT = "block_dirt";
    public const string BLOCK_WATER = "block_water";
    public const string BLOCK_KALITE = "block_kalite";
    public const string BLOCK_RIDGESTONE = "block_ridgestone";
    public const string BLOCK_WOOD_ASH = "block_wood_ash";
    public const string BLOCK_LEAF_ASH = "block_leaf_ash";
    public const string BLOCK_WALL_ASH = "block_wall_ash";
    public const string BLOCK_CULTIVATED_DIRT = "block_cultivated_dirt";
    public const string BLOCK_BEDROCK = "block_bedrock";
    public const string BLOCK_COPPER = "block_copper";
    public const string BLOCK_IRON = "block_iron";
    public const string BLOCK_TITANIUM = "block_titanium";
    public const string BLOCK_ITEM = "block_item";
    public const string BLOCK_PLANT = "block_plant";
    public const string BLOCK_NONE = "block_none";
    public const string BLOCK_UNKNOWN = "???";
    public const string BLOCK_OPEN_AIR = "Open air";
    #endregion BLOCKS

    #region GAMEPLAY

    public const int MAX_QUEUED_WORKSTATION_BLUEPRINTS = 5;
    public const int GAMEPLAY_ATTRIBUTES_MAX = 100;
    public const float GAMEPLAY_FOLLOW_OFFSET_THRESH = 16f; //48.0f;
    public const float GAMEPLAY_TOOLTIP_RANGE = 6.0f; //22
    public const int GAMEPLAY_MAX_EVASION = 999;
    public const int GAMEPLAY_MAX_TOUGHNESS = 999;

    #endregion GAMEPLAY

    #region ANIM

    

    #endregion ANIM

    #region TASKS

    public const string TASK_RESEARCH = "research";
    public const string TASK_RELAX = "relax";
    public const string TASK_ANIMATE = "anim";
    public const string TASK_ATTACK = "att";
    public const string TASK_DROP = "drop";
    public const string TASK_EXTINGUISH_FIRE = "extinguishFire";
    public const string TASK_FOLLOW = "foll";
    public const string TASK_MOVE = "move";
    public const string TASK_FEED = "feed";
    public const string TASK_PICK_UP = "pickup";
    public const string TASK_DESTROY_BLOCK_ITEM = "dstryBlkItem";
    public const string TASK_PLAY_BLOCK_FX = "blkfx";
    public const string TASK_SET_ANIM_FOCUS = "anmfocus";
    public const string TASK_SET_BLOCK_TYPE = "stblktype";
    public const string TASK_TAME = "tame";
    public const string TASK_BUILD = "build";
    public const string TASK_PLANT = "plant";
    public const string TASK_FORESTRY = "forestry";
    public const string TASK_UNRESERVE_TREE = "unrsrvetree";
    public const string TASK_RESERVE_TREE = "rsrvetree";
    public const string TASK_EQUIP = "equip";
    public const string TASK_END = "end";
    public const string TASK_TRANSFER_JOB = "transferjob";
    public const string TASK_AWARD_XP = "awardxp";
    public const string TASK_TIRE_ENTITY = "tireentity";
    public const string TASK_SLEEP = "sleep";
    public const string TASK_TRAIN = "train";
    public const string TASK_RESERVE_BLOCK = "resblock";
    public const string TASK_UNRESERVE_BLOCK = "unresblock";
    public const string TASK_EAT = "eat";
    public const string TASK_DRINK = "drink";
    public const string TASK_LOG = "log";
    public const string TASK_HARVEST = "harvest";
    public const string TASK_CUT = "cut";
    public const string TASK_LOOT = "loot";
    public const string TASK_FREE = "free";
    public const string TASK_CAPTURE = "capture";
    public const string TASK_PLAY_AUDIO = "plyaudio";
    public const string TASK_MATE = "mate";
    public const string TASK_SPAWN_ITEM = "spwnitem";
    public const string TASK_WORK_STATION_START = "workStnStart";
    public const string TASK_WORK_STATION_COMPLETE = "workStnCmplt";
    public const string TASK_WORK_STATION_CLEAR = "workStnClr";
    public const string TASK_USE_TOOL = "useTool";
    public const string TASK_SET_INDEX = "setIndex";
    public const string TASK_ADD_SKILL_XP = "addSkillXP";
    public const string TASK_PLAY_AUDIO_VARIANT = "playAudioVar";
    public const string TASK_BLOCK_INTERACT = "blkInteract";
    public const string TASK_ERROR = "err";
    public const string TASK_ANIM_ATTACK = "attanim";
    public const string TASK_BLOCK_ATTACK = "blkAttack";
    public const string TASK_BLOCK_PROGRESS = "blkProgress";
    public const string TASK_DO_WORK = "doWork";
    public const string TASK_WORK_DELAY = "workDelay";
    public const string TASK_JUMP_TO = "jumpTo";
    public const string TASK_ROOM_JOB_COMPLETE = "roomJobComplete";
    public const string TASK_SPAWN_ITEMS = "spawnItems";
    public const string TASK_BLOCK_INTERACT_DONE = "interactDone";
    public const string TASK_DO_FISHING = "fishing";
    public const string TASK_DO_WATER_COLLECT = "wtrCollect";
    public const string TASK_SPAWN_BLOCK_DROP_ITEMS = "spwnBlkDropItems";
    public const string TASK_SPAWN_BLOCK_PRODUCTION_ITEMS = "spwnBlkProductionItems";
    public const string TASK_DO_ROOM_JOB = "roomJob";
    public const string TASK_DECONSTRUCT_BLOCK = "deconstructBlk";
    public const string TASK_BUTCHER = "butcher";
    public const string TASK_MINE = "mine";


    #endregion TASKS

    #region PROFESSIONS

    public const string PROFESSION_FORESTER = "profession_forester";
    public const string PROFESSION_FISHERMAN = "profession_fisherman";
    public const string PROFESSION_WARRIOR = "profession_warrior";
    public const string PROFESSION_ARCHER = "profession_archer";
    public const string PROFESSION_SHEPHERD = "profession_shepherd";
    public const string PROFESSION_CARPENTER = "profession_carpenter";
    public const string PROFESSION_STONE_MASON = "profession_stone_mason";
    public const string PROFESSION_BLACKSMITH = "profession_blacksmith";
    public const string PROFESSION_MINER = "profession_miner";
    public const string PROFESSION_COOK = "profession_cook";
    public const string PROFESSION_HERBALIST = "profession_herbalist";
    public const string PROFESSION_TOR = "profession_tor";
    public const string PROFESSION_FARMER = "profession_farmer";
    //public const string PROFESSION_CHILD = "profession_child";
    public const string PROFESSION_NONE = "profession_none";
    public const string PROFESSION_ALL = "profession_all";
    public const string PROFESSION_ANIMAL = "profession_animal";
    public const string PROFESSION_RANDOM = "profession_random";
    public const string PROFESSION_RANDOM_NO_CHILDREN = "profession_random_no_children";
    public const string PROFESSION_RANDOM_FIGHTERS = "profession_random_fighters";
    public const string PROFESSION_RANDOM_MAGIC = "profession_random_magic";

    #endregion PROFESSIONS

    #region SKILLS

    public const string SKILL_LOG = "skill_log";
    public const string SKILL_CRAFT_WOOD = "skill_craft_wood";
    public const string SKILL_CRAFT_STONE = "skill_craft_stone";
    public const string SKILL_CRAFT_METAL = "skill_craft_metal";
    public const string SKILL_COOK = "skill_cook";
    public const string SKILL_FISH = "skill_fish";
    public const string SKILL_DECONSTRUCT = "skill_deconstruct";
    public const string SKILL_HARVEST = "skill_harvest";
    public const string SKILL_CARRY = "skill_carry";
    public const string SKILL_CUT = "skill_cut";
    public const string SKILL_CULTIVATE = "skill_cultivate";
    public const string SKILL_PLANT = "skill_plant";
    public const string SKILL_FIGHT_MELEE = "skill_fight_melee";
    public const string SKILL_FIGHT_RANGE = "skill_fight_range";
    public const string SKILL_TOR = "skill_tor";
    public const string SKILL_TAME_ANIMAL = "skill_tame_animal";
    public const string SKILL_FEED_ANIMAL = "skill_feed_animal";
    public const string SKILL_MINE = "skill_mine";
    public const string SKILL_HEAL = "skill_heal";
    public const string SKILL_ALL = "skill_all";
    public const string SKILL_NONE = "skill_none";

    //public const string SKILL_FRIENDLY_LOG = "Logging";
    //public const string SKILL_FRIENDLY_CRAFT_WOOD = "Carpentry";
    //public const string SKILL_FRIENDLY_CRAFT_STONE = "Stonework";
    //public const string SKILL_FRIENDLY_CRAFT_METAL = "Blacksmithing";
    //public const string SKILL_FRIENDLY_COOK = "Cooking";
    //public const string SKILL_FRIENDLY_FISH = "Fishing";
    //public const string SKILL_FRIENDLY_DECONSTRUCT = "Deconstructing";
    //public const string SKILL_FRIENDLY_HARVEST = "Harvesting";
    //public const string SKILL_FRIENDLY_CARRY = "Carrying";
    //public const string SKILL_FRIENDLY_CUT = "Cutting";
    //public const string SKILL_FRIENDLY_CULTIVATE = "Cultivating";
    //public const string SKILL_FRIENDLY_PLANT = "Planting";
    //public const string SKILL_FRIENDLY_FIGHT_MELEE = "Melee Fighting";
    //public const string SKILL_FRIENDLY_FIGHT_RANGE = "Ranged Fighting";
    //public const string SKILL_FRIENDLY_TOR = "Tor";
    //public const string SKILL_FRIENDLY_TAME_ANIMAL = "Taming Animals";
    //public const string SKILL_FRIENDLY_FEED_ANIMAL = "Feeding Animals";
    //public const string SKILL_FRIENDLY_MINE = "Mining";
    //public const string SKILL_FRIENDLY_HEAL = "Healing";
    //public const string SKILL_FRIENDLY_ALL = "All";
    //public const string SKILL_FRIENDLY_NONE = "None";

    //public const int SKILL_NAT_BONUS_MIN = 0;
    //public const int SKILL_NAT_BONUS_MAX = 1;

    //public const DiceTypes SKILL_FAVORED_BONUS_ROLL = DiceTypes.ONE_D_SIX;
    public const DiceTypes SKILL_NATURAL_BONUS_ROLL = DiceTypes.ONE_D_THREE;

    public const int SKILL_FAVORED_BONUS = 3;
    public const int SKILL_MAX_LEVEL = 45;
    public const int SKILL_MAX_BONUS = 5;
    public const int SKILL_MAX_TOOL_STR = 50;
    public const int SKILL_ERROR_TOUGHNESS = 999;
    public const int SKILL_NO_DATA_TOUGHNESS = 2;

    public const int SKILL_BASE_TARGET_STR = 16;
    public const float SKILL_TARGET_STR_GROWTH = 2.4875f;
    public const int SKILL_XP_REQ_BASE = 250;
    public const float SKILL_XP_REQ_GROWTH = 3f;
    public const int SKILL_XP_PER_ITERATION = 1;
    public const float SKILL_PROFICIENCY_GROWTH = 1.5f;

    public const float SKILL_XP_PER_BLOCK_STRENGTH_POW = 1.2f;
    public const int SKILL_XP_FIGHT_BASE = 95;

    #endregion SKILLS

    #region LEVELS

    public const int MAX_LEVEL = 99;

    #endregion

    #region POPPERS

    public const string POPPER_NEW_TOOLBAR_ITEM = "new_toolbar_item";

    #endregion

    #region UI

    //public const string UI_TOOLBAR_MENU_OPTION_PRIORITIZE_SETTLER = "prioritize_settler";
    //public const string UI_TOOLBAR_MENU_OPTION_SELECT_BORDER = "select_border";
    //public const string UI_TOOLBAR_MENU_OPTION_SELECT_IN_AIR = "select_in_air";
    //public const string UI_TOOLBAR_MENU_OPTION_SAME_LAYER_AIR = "select_midair";

    public const string UI_RACE_SELECT_REALM_NAME_NOT_LONG_ENOUGH = "The realm name needs to be at least 3 characters long.";
    public const string UI_RACE_SELECT_SEED_NOT_LONG_ENOUGH = "The world seed needs to be at least 1 number.";
    public const string UI_RACE_SELECT_LOCKED = "<sprite=1> Coming soon.";

    public const string UI_SKILLS_FAVORED_BONUSES = "Favored Bonuses:";

    public const string UI_ENTITY_OVERLAY_NO_BEHAVIOURS = "No active behaviours";
    public const string UI_SETTLERS_OVERLAY_NO_SETTLERS = "No settlers";
    public const string UI_SETTLERS_OVERLAY_NO_NOTIFS = "No notifications";
    public const string UI_PERMISSIONS_OVERLAY_NO_ENTITIES = "No entities";
    public const string UI_BLUEPRINTS_OVERLAY_NO_BLUEPRINTS = "No blueprints";
    public const string UI_BLUEPRINTS_OVERLAY_NO_WORKSTATIONS = "No workstations";
    public const string UI_ITEMS_FILTER_OVERLAY_NO_ITEMS = "No items";
    public const string UI_ENTITY_OVERLAY_NO_PERKS = "No perks";
    public const string UI_GENERIC_NO_ITEMS = "No items";


    public const string UI_ITEM_FILTER_TITLE = "Items";
    public const string UI_SETTLER_FILTER_TITLE = "Settlers";
    public const string UI_ANIMAL_FILTER_TITLE = "Animals";

    public const string UI_MISC_NO_PROFESSIONS_AVAILABLE = "No professions available";
    public const string UI_MISC_EMPTY = "Empty";
    public const string UI_MISC_ENABLED = "Enabled";
    public const string UI_MISC_DISABLED = "Disabled";

    public const string UI_CONFIRMATION_POPUP_QUIT_GAME = "Are you sure?";
    public const string UI_CONFIRMATION_POPUP_ARE_YOU_SURE = "Are you sure?";
    public const string UI_CHOOSE_PROF_POPUP_TITLE = "choose a profession";
    public const string UI_CHOOSE_PROF_POPUP_MESSAGE = "Choosing a new profession will give a bonus to favored skills and reset the rest.";
    
    public const int UI_TOOLTIP_CURSOR_OFFSET_X = 18;
    public const int UI_TOOLTIP_CURSOR_OFFSET_Y_HIGH = -18;
    public const int UI_TOOLTIP_CURSOR_OFFSET_Y_LOW = 48;

    public const string UI_BLUEPRINTS_PANEL_ID = "blueprints";
    public const string UI_PERMISSIONS_PANEL_ID = "permissions";
    public const string UI_SETTLERS_PANEL_ID = "setters";
    public const string UI_ENTITY_OVERLAY_ID = "entity";

    public const string UI_ICON_MISSING = "sp_missing_icon";
    public const string UI_ICON_TOR_SMALL = "sp_smalltor_icon";
    public const string UI_ICON_BUILDING_SMALL = "sp_smallbuilding_icon";
    public const string UI_ICON_CARRY_SMALL = "sp_smallcarry_icon";
    public const string UI_ICON_CULTIVATE_SMALL = "sp_smallcultivate_icon";
    public const string UI_ICON_CUTTING_SMALL = "sp_smallcutting_icon";
    public const string UI_ICON_DECONSTRUCT_SMALL = "sp_smalldeconstruct_icon";
    public const string UI_ICON_FARMING_SMALL = "sp_smallfarming_icon";
    public const string UI_ICON_FIGHT_SMALL = "sp_smallfight_icon";
    public const string UI_ICON_LOGGING_SMALL = "sp_smalllogging_icon";
    public const string UI_ICON_MINING_SMALL = "sp_smallmining_icon";
    public const string UI_ICON_PLANTING_SMALL = "sp_smallplanting_icon";
    public const string UI_ICON_SKILLS_SMALL = "sp_smallskills_icon";

    public const string UI_ROLE_NAME_TOR = "Tor";
    public const string UI_ROLE_NAME_BUILDER = "Builder";
    public const string UI_ROLE_NAME_FARMER = "Farmer";
    public const string UI_ROLE_NAME_RANGER = "Ranger";
    public const string UI_ROLE_NAME_LOGGER = "Logger";
    public const string UI_ROLE_NAME_MINER = "Miner";

    public const string UI_OVERWORLD_REALM_TITLE = "Realm: {0}";

    public static string[] UI_ORDINAL_INDICATORS = new string[] {
        "st",
        "nd",
        "rd",
        "th",
        "th",
        "th",
        "th",
        "th",
        "th",
        "th",//10th
        "th",
        "th",
        "th",
        "th",
        "th",
        "th",
        "th",
        "th",
        "th",
        "th",//20th
        "st",
        "nd",
        "rd",
        "th",
        "th",
        "th",
        "th",
        "th",
        "th",
        "th",//30th
        "st",
        "nd",
        "rd",
        "th",
        "th",
        "th",
        "th",
        "th",
        "th",
        "th",//40th
        "st",
        "nd",
        "rd",
        "th",
        "th",
        "th",
        "th",
        "th",
        "th",
        "th",//50th
        "st",
        "nd",
        "rd",
        "th",
        "th",
        "th",
        "th",
        "th",
        "th",
        "th",//60th
    };

    #endregion UI

    #region FX

    public const string FX_ENTITY_DEATH = "fx_entity_death";
    public const string FX_ENTITY_UNION = "fx_entity_union";
    public const string FX_ENTITY_TAMED = "fx_entity_union";
    public const string FX_ENTITY_BLOOD_SPLOTCH_FORMAT = "fx_entity_blood_splotch_red{0}";
    public const string FX_CURSOR_CLICK_DIRT_FORMAT = "fx_cursor_click_dirt{0}";
    public const string FX_CURSOR_MOVE_ORDER_SUCCESS = "fx_cursor_move_order_success";
    public const string FX_CURSOR_ATTACK_MOVE_ORDER_SUCCESS = "fx_cursor_attack_move_order_success";
    public const string FX_CURSOR_TAME_MOVE_ORDER_SUCCESS = "fx_cursor_tame_move_order_success";
    public const string FX_CURSOR_MOVE_ORDER_FAIL = "fx_cursor_move_order_fail";

    #endregion FX

    #region DATA

    public const string DATA_ENTITY_HUMAN = "entity_human";

    #endregion DATA

    #region ENTITIES


    public const float ENTITY_HUNGER_THRESHOLD_NORM = 0.5f;
    public const float ENTITY_STARVING_THRESHOLD_NORM = 0.25f;
    public const float ENTITY_THIRSTY_THRESHOLD_NORM = 0.4f;
    //public const int ENTITY_STARVING_THRESHOLD = 100;

    public const int ENTITY_HUNGER_DECAY_MIN = 1;
    public const int ENTITY_HUNGER_DECAY_MAX = 2;

    public const string ENTITY_APPEARANCES_PATH = "Appearances/";

    public const int ENTITY_MAX_APPEARANCE_TYPES = 4;
    public const int ENTITY_MAX_APPEARANCE_VARIANTS = 1;

    public const string ENTITY_GENDER_MALE = "male";
    public const string ENTITY_GENDER_FEMALE = "female";
    public const string ENTITY_GENDER_RANDOM = "random";
    public const string ENTITY_GENDER_PRIORITY = "priority";

    public const string ENTITY_HUMAN = "race_human";
    public const string ENTITY_ARDYN = "race_ardyn";
    public const string ENTITY_CHICKEN = "race_chicken";
    public const string ENTITY_NEQUHTLI = "race_nequhtli";

    #endregion

    #region HELPERS

    public static string AddOrdinalIndicatorToNumber(uint i)
    {
        if (i == 0 || i - 1 >= UI_ORDINAL_INDICATORS.Length)
        {
#if DEV_TESTING
            Debug.LogError("No ordinal indicator found for: " + i.ToString());
#endif
            return i.ToString();
        }

        return i + UI_ORDINAL_INDICATORS[i - 1];
    }

    public static string GetOrdinalIndicator(int i)
    {
        if (i <= 0 || i - 1 >= UI_ORDINAL_INDICATORS.Length)
        {
#if DEV_TESTING
            Debug.LogError("No ordinal indicator found for: " + i.ToString());
#endif
            return "th";
        }

        return UI_ORDINAL_INDICATORS[i - 1];
    }

    #endregion HELPERS
}