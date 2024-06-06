#if UPM_PACKAGE_VALIDATOR_CHECK_IS_COMPLETE
#if UNITY_EDITOR
using UnityEngine;
using Unity.Tutorials.Core.Editor;


public class TutorialCallbacks : ScriptableObject
{
    
    /// SRIVELLO ///////////////////////////////////////////
    
  
    private const string DefaultFileName = "TutorialCallbacks";
    private static Vector2 TutorialWindowMinSize = new Vector2(775, 500); //Measured width with UI Debugger
    
    /// <summary>
    /// First Tutorial to load when Welcome Dialog Ends
    /// </summary>
    [SerializeField]
    private Tutorial _defaultTutorial;
    
    
    public void ShowWindowWithTutorial()
    {
        //Debug.Log("ShowWindowWithTutorial");
        ShowWindow();
        TutorialWindow.StartTutorial(_defaultTutorial);
    }
    
    public void ShowWindow()
    {
        //Debug.Log("ShowWindow");
        
        //Yes, call show window BEFORE and after
        TutorialWindow.ShowWindow();
        TutorialWindow.Instance.minSize = TutorialWindowMinSize;
        TutorialWindow.Instance.maxSize = TutorialWindowMinSize;
        TutorialWindow.ShowWindow();
        TutorialWindow.Instance.Focus();
    }
}
#endif // UNITY_EDITOR
#endif // UPM_PACKAGE_VALIDATOR_CHECK_IS_COMPLETE
