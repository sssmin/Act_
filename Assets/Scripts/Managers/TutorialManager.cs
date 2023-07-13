using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tutorial : MonoBehaviour
{
    public virtual void BeginTutorial()
    {
        
    }

    public virtual void Execute(TutorialManager tutorialManager)
    {
        
    }

    public virtual void EndTutorial()
    {
        
    }
}

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private SceneLoadManager sceneLoadManager;
    [SerializeField] private List<Tutorial> tutorials;
    [SerializeField] private string nextSceneName;
    private Tutorial currentTutorial;
    private int currentIndex;

    private void Start()
    {
        currentIndex = -1;
        SetNextTutorial();
    }

    private void Update()
    {
        if (currentTutorial)
        {
            currentTutorial.Execute(this);
        }
    }

    public void SetNextTutorial()
    {
        if (currentTutorial)
        {
            currentTutorial.EndTutorial();
        }

        if (++currentIndex >= tutorials.Count)
        {
            ClearAllTutorials();
            return;
        }

        currentTutorial = tutorials[currentIndex];
        currentTutorial.BeginTutorial();
    }

    public void ClearAllTutorials()
    {
        currentTutorial = null;
        
        sceneLoadManager.RequestLoadSceneAsync(nextSceneName);
    }
}
