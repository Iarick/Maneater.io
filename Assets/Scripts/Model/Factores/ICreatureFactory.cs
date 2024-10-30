using UnityEngine;
public interface ICreatureFactory
{
    Creature Create(Vector3 position, Quaternion rotation, Vector3 scale);
}