using UnityEngine;

[CreateAssetMenu(fileName = "Order", menuName = "ScriptableObjects/Order", order = 1)]
public class OrderType : ScriptableObject
{
    public EnumDrink enumDrink;
    public Sprite orderImageSide;
    public Sprite orderImageTop;
}

public enum EnumDrink
{
    None,
    Beer,
    TequillaSunrise,
    Whiskey
}