using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public Button button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (button != null)
            button.onClick.AddListener(OnButtonPressed);
    }
    
    private static void OnButtonPressed()
    {
        Debug.Log("Button pressed");
    }
}
