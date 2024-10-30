using UnityEngine;

public class CreatureType1Factory : ICreatureFactory
{
    private Creature creaturePrefab;

    public CreatureType1Factory(Creature prefab)
    {
        creaturePrefab = prefab;
    }

    public Creature Create(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        Creature creature = GameObject.Instantiate(creaturePrefab, position, rotation);
        return creature;
    }
}
