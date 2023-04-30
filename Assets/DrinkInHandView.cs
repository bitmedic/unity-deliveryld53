using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkInHandView : MonoBehaviour
{
    public SpriteRenderer drinkBeer;
    public SpriteRenderer drinkTequillaSunrise;
    public SpriteRenderer drinkWhiskey;

    private void Start()
    {
        drinkBeer.enabled = false;
        drinkTequillaSunrise.enabled = false;
        drinkWhiskey.enabled = false;
    }

    public void ShowDrink(EnumDrink drink)
    {
        drinkBeer.enabled = false;
        drinkTequillaSunrise.enabled = false;
        drinkWhiskey.enabled = false;

        if (drink.Equals(EnumDrink.Beer))
        {
            drinkBeer.enabled = true;
        }
        else if (drink.Equals(EnumDrink.TequillaSunrise))
        {
            drinkTequillaSunrise.enabled = true;
        }
        else if (drink.Equals(EnumDrink.Whiskey))
        {
            drinkWhiskey.enabled = true;
        }
    }
}

