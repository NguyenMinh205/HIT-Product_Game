using UnityEngine;

public interface IBuffEffect
{
    string Name { get; set; }
    int Value { get; set; }
    int Duration { get; set; }
    void Apply(Player player);
    void Remove(Player player);
}
