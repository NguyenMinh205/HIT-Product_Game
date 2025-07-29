using System;
using System.Xml.Serialization;
using UnityEngine;

public interface IBuffEffect
{
    string Name { get; set; }
    float Value { get; set; }
    float Duration { get; set; }
    Sprite Icon { get; set; }
    void Apply(Player player);
    void Remove(Player player);

    void ApplyEnemy(Enemy enemy);
    void RemoveEnemy(Enemy enemy);
    void RegisterEvents();
    void UnregisterEvents();
}