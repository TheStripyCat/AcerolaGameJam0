using FlaxEngine;
using FlaxEngine.GUI;

public class ExitOnEsc : Script
{    
    [Serialize, ShowInEditor] UIControl quitButton, resumeButton;
    [Serialize, ShowInEditor] UICanvas bookCanvas;

    public override void OnEnable()
    {
        quitButton.Get<Button>().Clicked += QuitGame;
        resumeButton.Get<Button>().Clicked += ResumeGame;
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        quitButton.Get<Button>().Clicked -= QuitGame;
        resumeButton.Get<Button>().Clicked -= ResumeGame;
    }
    public override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyboardKeys.Escape))
        {
            Time.TimeScale = 0f;
            Screen.CursorLock = CursorLockMode.None;
            Screen.CursorVisible = true;
            bookCanvas.IsActive = true;
        }            
    }

    public void QuitGame()
    {
        Engine.RequestExit();
    }

    public void ResumeGame()
    {
        bookCanvas.IsActive = false;
        Time.TimeScale = 1f;
        Screen.CursorLock = CursorLockMode.Locked;
        Screen.CursorVisible = false;
    }

}
