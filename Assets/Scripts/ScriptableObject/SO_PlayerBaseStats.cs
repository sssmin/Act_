﻿using UnityEngine;

[CreateAssetMenu(fileName = "Stats_", menuName = "CharacterStats")]
public class PlayerBaseStats : ScriptableObject
{
    public Stats stats = new Stats();
}