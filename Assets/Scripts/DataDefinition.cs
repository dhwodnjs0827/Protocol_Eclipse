using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataDefinition
{
    // 데이터 구조체
    public struct PlayerStatusData
    {
        public int level;
        public int maxShield;
        public int maxHP;
        public float attackPower;
        public float fireAttack;
        public float iceAttack;
        public float electricityAttack;
        public float defense;
        public float fireDefense;
        public float iceDefense;
        public float electricityDefense;
    }
    public struct MonsterData
    {
        public int id;
    }
    public struct ItemData
    {
        public int id;
        public eItemType itemType;
        public string name;
        public eItemTier itemTier;
        public int price;
        public int maxStack;
        public string spriteResourceName;
    }
    public struct WeaponData
    {
        public int id;
        public string name;
        public eWeaponType weaponType;
        public float attackPowerPerBullet;
        public int rpm;
        public float effectiveRange;
        public float reloadTime;
        public int magazine;
        public float firePower;
        public float icePower;
        public float electricityPower;
        public float fireIntervalTime;
        public string prefabResourceName;
        public string sprite2x1ResourceName;
        public string aimResourceName;
    }
    public struct ArmorData
    {
        public int id;
        public string name;
        public eArmorType armorType;
    }
    public struct ETCData
    {
        public int id;
        public string name;
        public eETCType etcType;
    }
    public struct QuestData
    {
        public int id;
        public string name;
        public string description;
        public QuestObjectives[] objectives;
        public QuestRewards rewards;
        public eQuestType questType;
        public QuestGiver? questGiver;
        public QuestRequirements? requirements;
        public QuestDialogue dialogue;
        public eQuestStatus status;
    }
    public struct QuestObjectives
    {
        public eObjectivesType type;
        public eDataTable targetType;
        public int targetID;
        public string targetName;
        public int quantity;
    }
    public struct QuestRewards
    {
        public int exp;
        public int gold;
        public QuestItems[] items;
    }
    public struct QuestItems
    {
        public eItemType itemType;
        public int itemID;
        public string itemName;
        public int quantity;
    }
    public struct QuestGiver
    {
        public int npcID;
        public string npcName;
    }
    public struct QuestRequirements
    {
        public int requiredQuestID;
        public int requiredMinLevel;
    }
    public struct QuestDialogue
    {
        public string start;
        public string inProgress;
        public string complete;
    }

    // 열거형
    public enum eResourceType
    {
        Prefabs,
        Models,
        Materials,
        Textures,
        Sprites,
        Animations,
        Audios
    }
    public enum ePrefabType
    {
        Character,
        FX,
        Other,
        UI,
        Weapon,
    }
    public enum eSpriteType
    {
        Armor,
        ETCItem,
        Other,
        Skill,
        UI,
        Weapon,
    }
    public enum eMonsterState
    {
        Idle,
        Walk,
        Trace,
        Attack,
        Damaged,
        Dead
    }
    public enum eBulletImpactFX
    {
        Concreate,
        Metal,
        Sand,
        SoftBody,
        Wood
    }
    public enum eDataTable
    {
        Monster,
        Item,
        Weapon,
        Armor,
        ETC,
        Quest
    }
    public enum eItemType
    {
        None,
        Weapon,
        Armor,
        ETC,
        Module
    }
    public enum eWeaponType
    {
        AR,
        SR,
        SMG,
        SG
    }
    public enum eArmorType
    {
        None,
        Head,
        Body,
        Leg
    }
    public enum eETCType
    {
        Bullet,
        Reinforcement
    }

    public enum eItemTier
    {
        R,
        SR,
        SSR
    }
    public enum eUIState
    {
        None,
        Inventory,
        Skill,
        Journal
    }
    public enum eInventoryState
    {
        Weapon,
        Armor,
        ETC
    }
    public enum eSlotType
    {
        None,
        Inventory,
        EquippedWeapon,
        EquippedArmor
    }
    public enum eObjectivesType
    {
        None,
        Kill,
        Find
    }
    public enum eQuestType
    {
        None,
        Main,
        Side
    }
    public enum eQuestStatus
    {
        None,
        NotStarted,
        InProgress,
        Completed
    }

    // 상수
    static class Constants
    {
        public const int MaxItemListCount = 30;

        public const int AR_BULLET_ID = 331100;
        public const int SR_BULLET_ID = 331200;
        public const int SMG_BULLET_ID = 331300;
        public const int SG_BULLET_ID = 331400;

        public const string BLUE = "#548CD3";
        public const string PURPLE = "#6F30A0";
        public const string ORANGE = "#F69647";

    }
}