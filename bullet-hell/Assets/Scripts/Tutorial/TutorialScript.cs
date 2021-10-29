using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectiveObjects;

    private TutorialObject[] _objectives;

    private int tutorialIndex = 0;

    private bool _tutorialComplete = false;

    [SerializeField] private GameObject _prompt;

    private PromptScript _promptScript;

    private void Start()
    {
        _promptScript = _prompt.GetComponent<PromptScript>();
        _objectives = new TutorialObject[_objectiveObjects.Length];
        for (int i = 0; i < _objectiveObjects.Length; i++)
        {
            _objectives[i] = _objectiveObjects[i].GetComponent<TutorialObject>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_tutorialComplete)
        {
            _promptScript.destroy();
            Destroy(this.gameObject);
            return;
        }

        _promptScript.DispText = _objectives[tutorialIndex].getHintText();
        _promptScript.FloatAbove = _objectives[tutorialIndex].getHoverPoint();
        if (_objectives[tutorialIndex].isComplete())
        {
            if (tutorialIndex + 1 >= _objectives.Length)
            {
                _tutorialComplete = true;
            }
            else
            {
                tutorialIndex += 1;
            }
        }
    }
}