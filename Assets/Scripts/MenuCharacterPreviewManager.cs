using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuCharacterPreviewManager : MonoBehaviour
{
    [System.Serializable]
    public class PanelPreview
    {
        public Transform modelHolder;      // Drag aici Model Holder-ul din fiecare panel
        public GameObject boxerPrefab;     // Drag aici prefab-ul corect (Player1, Player2, etc)
    }

    public PanelPreview[] panelPreviews; // 4 elemente, unul pentru fiecare panel

    public Transform previewSpawnPoint;
    public GameObject[] boxerPrefabs; // Prefab-urile cu MenuBoxerBase atașat
    public RawImage previewImage;
    public Text boxerNameText;
    public Text statsText;
    public Animator previewAnimator;

    private GameObject currentPreview;
    private MenuBoxerBase currentBoxer;
    private int currentIndex = 0;

    void Start()
    {
        foreach (var preview in panelPreviews)
        {
            // Curăță orice instanță veche
            foreach (Transform child in preview.modelHolder)
                Destroy(child.gameObject);

            // Instanțiază boxerul
            var boxer = Instantiate(preview.boxerPrefab, preview.modelHolder.position, preview.modelHolder.rotation, preview.modelHolder);

            // Dă play la idle
            var animator = boxer.GetComponent<Animator>();
            var boxerBase = boxer.GetComponent<MenuBoxerBase>();
            if (animator != null && boxerBase != null)
                animator.SetTrigger(boxerBase.GetIdleAnimation());
        }
    }

    // Pentru hover pe butonul de selectare
    public void PlayTaunt()
    {
        if (previewAnimator != null && currentBoxer != null)
            previewAnimator.SetTrigger(currentBoxer.GetTauntAnimation());
    }

    // Pentru revenire la Idle după hover
    public void PlayIdle()
    {
        if (previewAnimator != null && currentBoxer != null)
            previewAnimator.SetTrigger(currentBoxer.GetIdleAnimation());
    }

    // Pentru selectarea boxerului
    public void SelectBoxer()
    {
        MenuManager.SetPlayerSelection(currentIndex);
        // Navighează înapoi la meniu sau unde ai nevoie
    }
}