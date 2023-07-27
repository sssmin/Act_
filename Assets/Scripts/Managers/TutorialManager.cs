using System.Collections.Generic;
using UnityEngine;


public enum ETutorial
{
    ItemCraft,
    
    Max
}


public class TutorialManager : MonoBehaviour
{
    //NewGame을 제외한 나머지 완료했는지 여부 true면 완료.
    private Dictionary<ETutorial, bool> TutorialStatusCompleted { get; set; }= new Dictionary<ETutorial, bool>();
    [SerializeField] private SceneLoadManager sceneLoadManager;
    [SerializeField] private string nextSceneName;
    public Tutorial CurrentTutorial { get; private set; }

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (CurrentTutorial != null)
        {
            CurrentTutorial.Execute();
        }
    }

    private void Init()
    {
        for (int i = 0; i < (int)ETutorial.Max; i++)
        {
            TutorialStatusCompleted.Add((ETutorial)i, false);
        }
    }

    public void ExecuteNewGameTutorial()
    {
        CurrentTutorial = new Tutorial_NewGame();
        CurrentTutorial.StartTutorial();
    }
    
    //반환형 false == 튜토리얼 완료 상태
    public bool ExecuteTutorialIfNotCompleted(ETutorial tutorialType)
    {
        if (TutorialStatusCompleted.ContainsKey(tutorialType))
        {
            if (TutorialStatusCompleted[tutorialType])
                return false;
            
            switch (tutorialType)
            {
                case ETutorial.ItemCraft:
                    CurrentTutorial = new Tutorial_ItemCraft();
                    CurrentTutorial.StartTutorial();
                    break;
            }
        }
        return true;
    }

    public void SkipCurrentTutorial()
    {
        if (CurrentTutorial != null)
        {
            CurrentTutorial.SkipTutorial();
        }
    }

    public void SetTutorialCompleted(ETutorial tutorialType)
    {
        CurrentTutorial = null;
        
        switch (tutorialType)
        {
            case ETutorial.ItemCraft:
                if (TutorialStatusCompleted.ContainsKey(tutorialType))
                {
                    TutorialStatusCompleted[tutorialType] = true;
                }
                break;
        }
    }
    
    #region Serialize
    
    public TutorialStatusDictionary SerializeTutorialStatus()
    {
        TutorialStatusDictionary tutorialStatusDictionary = new TutorialStatusDictionary();
        foreach (KeyValuePair<ETutorial,bool> pair in TutorialStatusCompleted)
        {
            tutorialStatusDictionary.Add(pair.Key, pair.Value);
        }
        tutorialStatusDictionary.Serialize();

        return tutorialStatusDictionary;
    }

    public void DeserializeTutorialStatus(TutorialStatusDictionary dict)
    {
        dict.Deserialize();
        
        foreach (KeyValuePair<ETutorial,bool> pair in dict)
        {
            if (TutorialStatusCompleted.ContainsKey(pair.Key))
                TutorialStatusCompleted[pair.Key] =  pair.Value;
        }
    }
    
    #endregion
    
   
}
