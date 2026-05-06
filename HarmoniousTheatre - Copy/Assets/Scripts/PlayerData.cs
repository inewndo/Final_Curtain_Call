using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;
    public Vector3 savedPosition;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
