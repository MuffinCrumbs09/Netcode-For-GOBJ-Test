using UnityEngine;

public class CosmeticHolder : MonoBehaviour
{
    public static CosmeticHolder Instance { get; private set; }
    public CosmeticScriptableObject[] Cosmetics;
    private void Awake()
    {
        //If there is an instance that isnt me, kill me
        if (Instance != null && Instance != this)
            Destroy(this);

        //Sets me as the instance
        Instance = this;
    }
}
