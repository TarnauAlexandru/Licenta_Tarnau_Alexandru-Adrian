using UnityEngine;

public class MenuAnimationManager : MenuManager
{
    public Animator[] boxerAnimators; // un animator per boxer din panel, sau ce ai tu în UI
    public MenuBoxerBase[] boxerData; // Setezi în Inspector, corespunde cu boxerAnimators

    public static MenuBoxerBase player1Boxer;
    public static MenuBoxerBase player2Boxer;

    public void PlayIdleAnimation(int boxerIndex)
    {
        if (boxerIndex >= 0 && boxerIndex < boxerAnimators.Length && boxerData[boxerIndex] != null)
            boxerAnimators[boxerIndex].SetTrigger(boxerData[boxerIndex].GetIdleAnimation());
    }

    public void PlayHighlightAnimation(int boxerIndex)
    {
        if (boxerIndex >= 0 && boxerIndex < boxerAnimators.Length)
            boxerAnimators[boxerIndex].SetTrigger("Highlight");
    }

    public void PlayTauntAnimation(int boxerIndex)
    {
        if (boxerIndex >= 0 && boxerIndex < boxerAnimators.Length && boxerData[boxerIndex] != null)
            boxerAnimators[boxerIndex].SetTrigger(boxerData[boxerIndex].GetTauntAnimation());
    }

    // Poți folosi direct metodele și variabilele din MenuManager aici!
    // Exemplu:
    public void SelectAndAnimate(int boxerIndex)
    {
        SelectPlayer(MenuManager.selectingPlayer, boxerIndex);
        PlayIdleAnimation(boxerIndex);
    }

    public static void SetPlayerSelection(int boxerIndex, MenuBoxerBase boxerRef)
    {
        if (selectingPlayer == 1) {
            player1Index = boxerIndex;
            player1Boxer = boxerRef;
        } else if (selectingPlayer == 2) {
            player2Index = boxerIndex;
            player2Boxer = boxerRef;
        }
    }
}
