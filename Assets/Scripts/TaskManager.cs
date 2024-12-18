using System.Collections;
using UnityEngine;
using TMPro;
using System.Diagnostics;

[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class TaskManager : MonoBehaviour
{
    public TextMeshProUGUI taskText; 
    public GameObject completionTextObject; 
    public CanvasGroup taskCanvasGroup; 
    public float fadeDuration = 0.5f; 
    public float completionDisplayDuration = 2f; 

    private int currentTaskIndex = 0;
    private string[] tasks = {
        "Open the curtains",
        "Change the background"
    };
    private bool isTaskActive = false;

    void Start()
    {
        StartTask();
    }

    void Update()
    {
        
        if (currentTaskIndex == 0 && Input.GetKeyDown(KeyCode.F))
        {
            CompleteTask();
        }
        else if (currentTaskIndex == 1 && Input.GetKeyDown(KeyCode.Space))
        {
            CompleteTask();
        }
    }

    void StartTask()
    {
        if (currentTaskIndex < tasks.Length)
        {
            taskText.text = tasks[currentTaskIndex];
            StartCoroutine(FadeInText(taskCanvasGroup)); 
            isTaskActive = true;
        }
    }

    void CompleteTask()
    {
        if (isTaskActive)
        {
            isTaskActive = false;
            StartCoroutine(HandleCompletion());
        }
    }

    IEnumerator HandleCompletion()
    {
        
        completionTextObject.SetActive(true); 
        yield return new WaitForSeconds(completionDisplayDuration); 

        completionTextObject.SetActive(false); 

        
        yield return FadeOutText(taskCanvasGroup);

        
        currentTaskIndex++;
        yield return new WaitForSeconds(5f); 

        
        StartTask();
    }

    IEnumerator FadeInText(CanvasGroup canvasGroup)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1;
    }

    IEnumerator FadeOutText(CanvasGroup canvasGroup)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, timer / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}


