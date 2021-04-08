using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class HUD : MonoBehaviour
{
    private bool leftControllerOn;
    private InputDevice leftController;

    private Canvas canvasHUD;

    private float lastPressTime;
    private bool lastTickPressed;

    private int pRoll;
    private bool upCRoll;
    private string result;

    // Start is called before the first frame update
    void Start()
    {
        leftControllerOn = false;
        canvasHUD = GetComponent<Canvas>();
        canvasHUD.enabled = false;
        lastPressTime = 0;
        lastTickPressed = false;
        upCRoll = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Left_Hand").GetComponentInChildren<LeftInputHandler>().controller.isValid)
        {
            if (leftControllerOn == false)
            {
                leftController = GameObject.Find("Left_Hand").GetComponentInChildren<LeftInputHandler>().controller;
                leftControllerOn = true;
            }
        }
        else { leftControllerOn = false; }
    }

    private void FixedUpdate()
    {
        if (leftControllerOn)
        {
            if (leftController.TryGetFeatureValue(CommonUsages.menuButton, out bool pressed) && pressed)
            {
                if((Time.fixedTime - lastPressTime) > 0.3f && !lastTickPressed)
                {
                    if (canvasHUD.enabled) { canvasHUD.enabled = false; }
                    else { canvasHUD.enabled = true; }

                    lastPressTime = Time.fixedTime;
                }

                lastTickPressed = true;
            }
            else { lastTickPressed = false; }
        }
    }

    public int UpdateHUD(int cRoll, int quillesTouche)
    {
        if (cRoll < 19)
        {
            if (quillesTouche == 10 && cRoll % 2 != 0)
            {
                result = "X";
                upCRoll = true;
            }
            else if (pRoll + quillesTouche == 10 && cRoll % 2 == 0)
            {
                result = "/";
            }
            else if (cRoll % 2 != 0)
            {
                result = quillesTouche.ToString();
                pRoll = quillesTouche;
            }
            else
            {
                result = quillesTouche.ToString();
            }
        }
        else if (cRoll == 19)
        {
            pRoll = 0;
            if (quillesTouche == 10)
            {
                result = "X";
            }
            else
            {
                result = quillesTouche.ToString();
                pRoll = quillesTouche;
            }
        }
        else if (cRoll == 20)
        {
            if (quillesTouche == 10 && GameObject.Find("Roll" + 19).GetComponent<Text>().text == "X")
            {
                result = "X";
            }
            else if (pRoll + quillesTouche == 10)
            {
                result = "/";
            }
            else
            {
                result = quillesTouche.ToString();
                pRoll = quillesTouche;
            }
        }
        else if (cRoll == 21)
        {
            if (quillesTouche == 10 && (GameObject.Find("Roll" + 20).GetComponent<Text>().text == "X" || GameObject.Find("Roll" + 20).GetComponent<Text>().text == "/"))
            {
                result = "X";
            }
            else if (pRoll + quillesTouche == 10)
            {
                result = "/";
            }
            else
            {
                result = quillesTouche.ToString();
            }
        }

        GameObject.Find("Roll" + cRoll).GetComponent<Text>().text = result;

        if (upCRoll)
        {
            cRoll++;
        }
        upCRoll = false;

        return cRoll;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Bowling");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
