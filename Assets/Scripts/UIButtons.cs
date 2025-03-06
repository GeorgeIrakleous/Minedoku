using UnityEngine;
using UnityEngine.UI;
using System;

public class UIButtons : MonoBehaviour
{
    // Public Button fields to assign from the Inspector.
    public Button revealButton;
    public Button mineFlagButton;
    public Button flag1Button;
    public Button flag2Button;
    public Button flag3Button;

    // Action event that sends an int. Subscribers can use this to know which button was pressed.
    public event Action<int> OnFlagButtonPressed;

    // Reference to the currently selected button.
    private Button lastSelectedButton;

    // Colors to indicate normal and selected states.
    public Color normalColor = Color.white;
    public Color selectedColor = Color.green;

    private void Start()
    {
        lastSelectedButton = revealButton;
        SetButtonColor(revealButton, selectedColor);

        // Subscribe each button's click event to call ButtonClicked with a unique int and the button itself.
        if (revealButton != null)
            revealButton.onClick.AddListener(() => ButtonClicked(0, revealButton));
        if (mineFlagButton != null)
            mineFlagButton.onClick.AddListener(() => ButtonClicked(1, mineFlagButton));
        if (flag1Button != null)
            flag1Button.onClick.AddListener(() => ButtonClicked(2, flag1Button));
        if (flag2Button != null)
            flag2Button.onClick.AddListener(() => ButtonClicked(3, flag2Button));
        if (flag3Button != null)
            flag3Button.onClick.AddListener(() => ButtonClicked(4, flag3Button));
    }

    // Called when any button is clicked.
    private void ButtonClicked(int buttonNumber, Button clickedButton)
    {
        // Fire the event, passing the button number.
        OnFlagButtonPressed?.Invoke(buttonNumber);

        // Deselect the previously selected button (if any) by resetting its color.
        if (lastSelectedButton != null)
        {
            SetButtonColor(lastSelectedButton, normalColor);
        }

        // Set the clicked button to the "selected" color.
        SetButtonColor(clickedButton, selectedColor);

        // Save the clicked button as the last selected one.
        lastSelectedButton = clickedButton;
    }

    // Helper method to update a button's color.
    private void SetButtonColor(Button btn, Color color)
    {
        // Copy the current ColorBlock so we can modify it.
        ColorBlock cb = btn.colors;
        // Update all the relevant states to ensure the button stays the chosen color.
        cb.normalColor = color;
        cb.highlightedColor = color;
        cb.pressedColor = color;
        cb.selectedColor = color;
        btn.colors = cb;
    }
}
