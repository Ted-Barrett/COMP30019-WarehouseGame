using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PromptScript : MonoBehaviour
{
    private String _dispText;
    [SerializeField] private GameObject _floatAbove;

    [SerializeField] private TextMeshProUGUI _textObject;
    [SerializeField] private GameObject _backgroundObject;

    private float textBoxHeight;

    [SerializeField] private float vertOffset;

    private Camera _mainCam;
    private void Start()
    {
        _mainCam=Camera.main;
        _dispText = "test";
        _textObject.text = _dispText;
    }

    // Update is called once per frame
    void Update()
    {
        _textObject.text = _dispText;
        this.transform.position = _mainCam.WorldToScreenPoint(_floatAbove.transform.position);
    }

    public GameObject FloatAbove
    {
        get => _floatAbove;
        set => _floatAbove = value;
    }

    public string DispText
    {
        get => _dispText;
        set => _dispText = value;
    }

    public void destroy()
    {
        Destroy(gameObject);
    }
}
