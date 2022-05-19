using UnityEngine;
using UnityEngine.Events;

public class ToolAnimationEvents : MonoBehaviour
{
    public UnityEvent use;
    public UnityEvent altUse;

    public void Use()
    {
        use?.Invoke();
    }

    public void AltUse()
    {
        altUse?.Invoke();
    }

    public void CameraShake()
    {
        FindObjectOfType<CameraShake>().Shake();    
    }
}

