using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioManager : MonoBehaviour
{
    public Text scenarioText;
    public Button actionButton;
   
    private List<string> scenarios = new List<string>
    {
        "Single Tap(One-finger)",
        "Single Tap(Two-finger)",
        "Double Tap(One-finger)",
        "Double Tap(Two-finger)",
        "Hard press"
    };

    private int currentScenarioIndex = 0;
    //private bool isTaskCompleted = false;

    private void Start()
    {
        //MarkScenarioCompleted();
        DisplayScenario();
        actionButton.onClick.AddListener(OnActionButtonClick);
    }

    private void DisplayScenario()
    {
        if (currentScenarioIndex < scenarios.Count)
        {
            scenarioText.text = scenarios[currentScenarioIndex];
           // isTaskCompleted = false; // Reset completion status for the new scenario
            actionButton.interactable = true; // Enable the button for the new scenario
        }
        else
        {
            // All scenarios are completed
            scenarioText.text = "All scenarios completed!";
            actionButton.interactable = false; // Disable the button when all scenarios are completed
        }
    }

    private void OnActionButtonClick()
    {
        // Check if the current scenario is completed correctly before moving to the next one
     
        
            // Move to the next scenario
            currentScenarioIndex++;

            // Display the next scenario
            DisplayScenario();
        
        
        
            //// Show a message or take other actions to indicate that the current scenario is not completed correctly
            //Debug.Log("Complete the current scenario correctly before moving on!");
        
    }

    // Call this method when the current scenario is successfully completed
    //public void MarkScenarioCompleted()
    //{
    //    isTaskCompleted = true;
    //    Debug.Log("Marking scenario as completed.");
    //}
    
}
