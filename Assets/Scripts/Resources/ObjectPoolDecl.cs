using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPoolDecl", menuName = "Scriptable Objects/ObjectPoolDecl")]
public class ObjectPoolDecl : ScriptableObject
{
    public PoolableObject objectToPool;
    public int amount = 20;
}
