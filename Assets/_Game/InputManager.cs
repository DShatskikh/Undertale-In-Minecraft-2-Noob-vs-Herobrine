using System;
using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-25)]
public sealed class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    
    public bool IsSubmitDown;
    public bool IsSubmit;
    public bool IsSubmitUp;
    public bool IsCancelDown;
    public bool IsCancel;
    public bool IsCancelUp;
    public bool IsOpenInventoryDown;
    public bool IsOpenInventory;
    public bool IsOpenInventoryUp;
    public float Horizontal;
    public bool HorizontalUp;
    public bool HorizontalDown;
    public float Vertical;
    public bool VerticalDown;
    public bool VerticalUp;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
    }

    private void Update()
    {
#if PLATFORM_STANDALONE
        IsSubmitDown = Input.GetButtonDown("Submit");
        IsSubmit = Input.GetButton("Submit");
        IsSubmitUp = Input.GetButtonUp("Submit");
        
        IsCancelDown = Input.GetButtonDown("Cancel");
        IsCancel = Input.GetButton("Cancel");
        IsCancelUp = Input.GetButtonUp("Cancel");
        
        IsOpenInventoryDown = Input.GetButtonDown("OpenInventory");
        IsOpenInventory = Input.GetButton("OpenInventory");
        IsOpenInventoryUp = Input.GetButtonUp("OpenInventory");
        
        HorizontalDown = Input.GetButtonDown("Horizontal");
        Horizontal = Input.GetAxisRaw("Horizontal");
        HorizontalUp = Input.GetButtonUp("Horizontal");
        
        VerticalDown = Input.GetButtonDown("Vertical");
        Vertical = Input.GetAxisRaw("Vertical");
        VerticalUp = Input.GetButtonUp("Vertical");
#endif
    }

    private void LateUpdate()
    {
#if PLATFORM_STANDALONE
        
#endif
    }
    
    public IEnumerator AwaitSubmitDownOrClick()
    {
        while (true)
        {
            if (IsSubmitDown || Input.GetMouseButtonDown(0))
                yield break;
            
            yield return null;
        }
    }
}
