using UnityEngine;

[CreateAssetMenu(fileName = "New Cosmetic", menuName = "New Cosmetic", order = -1)]
public class CosmeticScriptableObject : ScriptableObject
{
    public int ID;
    public string Name;
    public GameObject Cosmetic;
    public Vector3 Offset = Vector3.zero;
}
