using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TutorialController : MonoBehaviour
{
    public Image slideImage;
    public Sprite[] slides;
    public TMP_Text slideText;
    public Button next;
    public Button previous;
    static int currIndex = 0;
    string[] slideDescriptions = {
        "1. This is the playing field where the game takes place. Moles with equations will pop up here.",
        "2. These are the moles that will appear on the playing field. Each mole has a math equation on it. You will need to solve equations by entering the answer in the bottom-left box to whack them!",
        "3. Try to whack as many moles as you can before time runs out. Each correct answer will earn you points and consecutive correct answers will earn you bonus points!",
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void PreviousSlide()
    {
        int previousIndex = currIndex - 1;
        if (previousIndex >= 0) ShowSlide(previousIndex);
        else {
            Debug.Log("No previous slide, index: " + previousIndex);
        }
    }

    public void NextSlide()
    {
        int nextIndex = currIndex + 1;
        if (nextIndex < slides.Length) ShowSlide(nextIndex); 
        else {
            Debug.Log("No next slide");
        }
    }

    void ShowSlide(int index)
    {
        slideImage.sprite = slides[index];
        slideText.text = slideDescriptions[index];
        currIndex = index;
    }
}
