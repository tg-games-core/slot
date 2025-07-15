using UnityEngine;

public class PhysxGameManager : MonoBehaviour
{
    [Header("Синглтон")]
    public static PhysxGameManager Instance { get; private set; }
    
    [Header("Менеджеры")]
    public HapticManager hapticManager;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void OnDiceFellInGap(GameObject dice)
    {
        OnDiceLost();
    }
    
    void OnDiceLost()
    {
        if (hapticManager != null)
        {
            hapticManager.HapticTriggerMedium();
        }
    }
} 