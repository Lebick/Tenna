using System.Runtime.InteropServices;
using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Kirurobo;

public class TopMost : MonoBehaviour
{
    public Text text;
    public Toggle toggle;

    public UniWindowController uniWindowController;

    private void Start()
    {
        toggle.onValueChanged.AddListener((bool isOn) => {
            uniWindowController.isTransparent = isOn;
            text.text = $"{isOn}";
        });
    }
}
