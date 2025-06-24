using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuPlayerPanelUI : MonoBehaviour
{
    public Image panelImage;
    public Color normalColor = Color.white;
    public Color dimColor = new Color(0.5f, 0.5f, 0.5f, 0.7f);
    public TextMeshProUGUI statsText; // Pentru TextMeshPro
    public Transform modelHolder; // Setezi în Inspector, e un GameObject gol în panel
    private GameObject currentModel; // Referință la modelul instanțiat

    public void SetDimmed(bool dimmed)
    {
        if (panelImage != null)
            panelImage.color = dimmed ? dimColor : normalColor;
    }

    public void UpdateStats(MenuBoxerBase boxer)
    {
        if (boxer != null && statsText != null)
        {
            statsText.text = $"DMG: {boxer.damage}  STA: {boxer.stamina}  CRIT: {boxer.crit}  RED: {boxer.damageReduction}";
        }
        else if (statsText != null)
        {
            statsText.text = "";
        }
    }

    public void UpdatePanels(MenuPlayerPanelUI playerPanelUI, MenuPlayerPanelUI cpuPanelUI, MenuBoxerBase[] listaBoxeri)
    {
        if (MenuManager.playerIndex != -1)
            playerPanelUI.UpdateStats(listaBoxeri[MenuManager.playerIndex]);
        if (MenuManager.cpuIndex != -1)
            cpuPanelUI.UpdateStats(listaBoxeri[MenuManager.cpuIndex]);
    }

    public void SetBoxerModel(MenuBoxerBase boxer)
    {
        if (currentModel != null)
            Destroy(currentModel);

        if (boxer != null && boxer.menuPreviewPrefab != null && modelHolder != null)
        {
            currentModel = Instantiate(boxer.menuPreviewPrefab, modelHolder);
            currentModel.transform.localPosition = Vector3.zero;
            currentModel.transform.localRotation = Quaternion.identity;
            currentModel.transform.localScale = Vector3.one;

            var animator = currentModel.GetComponent<Animator>();
            if (animator != null)
                animator.SetTrigger(boxer.GetIdleAnimation());
        }
    }
}