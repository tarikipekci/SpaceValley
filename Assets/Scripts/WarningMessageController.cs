using UnityEngine;
using UnityEngine.UI;

public class WarningMessageController : MonoBehaviour
{
    public GameObject text;
    public Text warningMessage;
    public static WarningMessageController instance;
    public RectTransform warningMessageSpot;

    private void Awake()
    {
        instance = this;
        warningMessage = text.GetComponent<Text>();
    }
    
    public void PrintMessage(string message)
    {
        warningMessage.text = message;
        var newWarning = Instantiate(text, warningMessageSpot.position, transform.rotation);
        newWarning.transform.SetParent(GameObject.Find("UI Canvas").gameObject.transform);
        Destroy(newWarning, 1f);
    }
}