using UnityEngine;

public class UI_Manager : MonoBehaviour
{

    public static UI_Manager instance { get; private set; }
    public HUD_UI hud;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
