using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodDetail : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public Image FoodImage;
    public TextMeshProUGUI PriceText;
    public Button BuyButton;

    private Food Food;

    public void DelegatePurchaseFood()
    {
        // TODO
    }

    public void Display(Food food)
    {
        this.Food = food;
        this.SetNameText();
        this.SetFoodImageSprite();
        this.SetPriceText();
    }

    private void SetNameText()
    {
        this.NameText.text = this.Food.Name;
    }

    private void SetFoodImageSprite()
    {
        Sprite sprite = this.Food.GetFreshFoodSprite();
        this.FoodImage.sprite = sprite;
    }

    private void SetPriceText()
    {
        this.PriceText.text = this.Food.Price.ToString();
    }

}
