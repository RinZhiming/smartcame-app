using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaneConnectView : MonoBehaviour
{
    [SerializeField] private InputField inputId;
    [SerializeField] private Button connectButton, disconnectButton, caneIdButton;
    [SerializeField] private GameObject connect, disconnect;
    [SerializeField] private Text textError, connectStatusText;
    [SerializeField] private CanvaUiController caneIdCanvas, caneCompleteCanvas;
    [SerializeField] private CanvasGroup settingCanvas;
    [SerializeField] private Image connectStatusUi;
    [SerializeField] private Sprite connectSprite, disconnectSprite;

    public GameObject Connect
    {
        get => connect;
        set => connect = value;
    }
    
    public GameObject Disconnect
    {
        get => disconnect;
        set => disconnect = value;
    }
    
    public InputField InputId
    {
        get => inputId;
        set => inputId = value;
    }
    
    public Button ConnectButton
    {
        get => connectButton;
        set => connectButton = value;
    }
    
    public Button DisconnectButton
    {
        get => disconnectButton;
        set => disconnectButton = value;
    }
    
    public Button CaneIdButton
    {
        get => caneIdButton;
        set => caneIdButton = value;
    }
    
    public Text TextError
    {
        get => textError;
        set => textError = value;
    }
    
    public Text ConnectStatusText
    {
        get => connectStatusText;
        set => connectStatusText = value;
    }
    
    public CanvaUiController CaneIdCanvas
    {
        get => caneIdCanvas;
        set => caneIdCanvas = value;
    }
    
    public CanvaUiController CaneCompleteCanvas
    {
        get => caneCompleteCanvas;
        set => caneCompleteCanvas = value;
    }
    
    public CanvasGroup SettingCanvas
    {
        get => settingCanvas;
        set => settingCanvas = value;
    }
    
    public Image ConnectStatusUi
    {
        get => connectStatusUi;
        set => connectStatusUi = value;
    }
    
    public Sprite ConnectSprite
    {
        get => connectSprite;
        set => connectSprite = value;
    }
    
    public Sprite DisconnectSprite
    {
        get => disconnectSprite;
        set => disconnectSprite = value;
    }
}
