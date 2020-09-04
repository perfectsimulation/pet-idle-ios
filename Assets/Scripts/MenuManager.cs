using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public Button MainMenuButton;
    public Button CloseButton;
    public Button InventoryMenuButton;
    public GameObject InventoryMenuPanel;

    // Start with no active menus, only the main menu button showing
    void Start()
    {
        this.FocusActiveBiome();
    }

    // Display the main menu panel and close button
    public void OnMainMenuButtonPress()
    {
        this.MainMenuPanel.SetActive(true);
        this.MainMenuButton.gameObject.SetActive(false);
        this.CloseButton.gameObject.SetActive(true);
        this.InventoryMenuPanel.SetActive(false);
    }

    // Display the inventory menu panel and hide the main menu panel
    public void OnInventoryMenuButtonPress()
    {
        this.MainMenuPanel.SetActive(false);
        this.InventoryMenuPanel.SetActive(true);
    }

    // Close out of all menus
    public void OnCloseButtonPress()
    {
        this.FocusActiveBiome();
    }

    // Hide all menu elements except the main menu button
    void FocusActiveBiome()
    {
        this.MainMenuPanel.SetActive(false);
        this.MainMenuButton.gameObject.SetActive(true);
        this.CloseButton.gameObject.SetActive(false);
        this.InventoryMenuPanel.SetActive(false);
    }

}
