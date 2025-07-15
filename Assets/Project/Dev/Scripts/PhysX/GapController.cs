using System;
using Project.Bounce;
using UnityEngine;

public class GapController : MonoBehaviour
{
    public event Action<PlinkoDice> DiceFell;
    
    private PhysxGameManager gameManager;
    
    void Start()
    {
        gameManager = PhysxGameManager.Instance;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out PlinkoDice dice))
        {
            DiceFell?.Invoke(dice);
            
            HandleDiceFall(other.gameObject);
        }
    }
    
    void HandleDiceFall(GameObject dice)
    {
        Debug.Log("Кубик попал в пропасть! Уничтожаем: " + dice.name);
        
        if (gameManager != null)
        {
            gameManager.OnDiceFellInGap(dice);
        }
    }
    
    void OnDrawGizmosSelected()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawWireCube(transform.position, boxCollider.size);
        }
    }
    
    void OnDrawGizmos()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
            Gizmos.DrawCube(transform.position, boxCollider.size);
        }
    }
} 