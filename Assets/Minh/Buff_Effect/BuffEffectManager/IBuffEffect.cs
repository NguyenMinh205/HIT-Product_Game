using UnityEngine;

public interface IBuffEffect
{
    string Name { get; set; }
    float Value { get; set; }
    float Duration { get; set; }
    void Apply(Player player);
    void Remove(Player player);
    void RegisterEvents();
    void UnregisterEvents();
}