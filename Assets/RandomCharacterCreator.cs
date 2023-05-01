using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCharacterCreator : MonoBehaviour
{
    public SpriteRenderer spriteHead_Skin;
    public SpriteRenderer spriteHead_Hair;
    public SpriteRenderer spriteHead_Eye;
    public SpriteRenderer spriteBody_Skin;
    public SpriteRenderer spriteBody_Shirt_Long;
    public SpriteRenderer spriteBody_Shirt_Sleeveless;
    public SpriteRenderer spriteArmLeft_Skin;
    public SpriteRenderer spriteArmLeft_Long;
    public SpriteRenderer spriteArmLeft_Short;
    public SpriteRenderer spriteArmLeft_No;
    public SpriteRenderer spriteArmRight_Skin;
    public SpriteRenderer spriteArmRight_Long;
    public SpriteRenderer spriteArmRight_Short;
    public SpriteRenderer spriteArmRight_No;
    public SpriteRenderer spriteFootRight_Shoe;
    public SpriteRenderer spriteFootRight_Pants;
    public SpriteRenderer spriteFootLeft_Shoe;
    public SpriteRenderer spriteFootLeft_Pants;


    public List<Color> skinColors;
    public List<Color> hairColors;
    public List<Sprite> spritesHead_Hairs;
    public List<Color> eyeColors;
    public List<Color> shirtColors;
    public List<Color> pantsColors;
    public List<Color> shoeColors;


    void Start()
    {
        int randomHair = Random.Range(0, spritesHead_Hairs.Count);
        int randomColorHair = Random.Range(0, hairColors.Count);
        int randomColorEye = Random.Range(0, eyeColors.Count);
        int randomColorSkin = Random.Range(0, skinColors.Count);
        int randomColorShirt = Random.Range(0, shirtColors.Count);
        int randomColorPants = Random.Range(0, pantsColors.Count);
        int randomColorShoes = Random.Range(0, shoeColors.Count);

        int randomShirtLongShort =  Random.Range(0, 100);
        int randomArmsLongShortNo =  Random.Range(0, 100);

        spriteHead_Hair.sprite = spritesHead_Hairs[randomHair];

        spriteHead_Skin.color = skinColors[randomColorSkin];
        spriteBody_Skin.color = skinColors[randomColorSkin];
        spriteArmLeft_Skin.color = skinColors[randomColorSkin];
        spriteArmRight_Skin.color = skinColors[randomColorSkin];
        spriteHead_Eye.color = eyeColors[randomColorEye];

        if (randomHair == spritesHead_Hairs.Count - 1)
        {
            spriteHead_Hair.color = new Color(236, 35, 153);
        }
        else
        {
            spriteHead_Hair.color = hairColors[randomColorHair];
        }

        spriteBody_Shirt_Long.color = shirtColors[randomColorShirt];
        spriteBody_Shirt_Long.gameObject.SetActive(false);
        spriteBody_Shirt_Sleeveless.color = shirtColors[randomColorShirt];
        spriteBody_Shirt_Sleeveless.gameObject.SetActive(false);

        spriteArmLeft_Long.color = shirtColors[randomColorShirt];
        spriteArmLeft_Long.gameObject.SetActive(false);
        spriteArmLeft_Short.color = shirtColors[randomColorShirt];
        spriteArmLeft_Short.gameObject.SetActive(false);
        spriteArmLeft_No.color = shirtColors[randomColorShirt];
        spriteArmLeft_No.gameObject.SetActive(false);
        spriteArmRight_Long.color = shirtColors[randomColorShirt];
        spriteArmRight_Long.gameObject.SetActive(false);
        spriteArmRight_Short.color = shirtColors[randomColorShirt];
        spriteArmRight_Short.gameObject.SetActive(false);
        spriteArmRight_No.color = shirtColors[randomColorShirt];
        spriteArmRight_No.gameObject.SetActive(false);

        if (randomShirtLongShort <= 50)
        {
            spriteBody_Shirt_Long.gameObject.SetActive(true);

            if (randomShirtLongShort <= 70)
            {
                spriteArmLeft_Long.gameObject.SetActive(true);
                spriteArmRight_Long.gameObject.SetActive(true);
            }
            else if (randomShirtLongShort <= 95)
            {
                spriteArmLeft_Short.gameObject.SetActive(true);
                spriteArmRight_Short.gameObject.SetActive(true);
            }
        }
        else
        {
            spriteBody_Shirt_Sleeveless.gameObject.SetActive(true);
            spriteArmLeft_No.gameObject.SetActive(true);
            spriteArmRight_No.gameObject.SetActive(true);
        }



        spriteFootLeft_Shoe.color = shoeColors[randomColorShoes];
        spriteFootRight_Shoe.color = shoeColors[randomColorShoes];
        spriteFootLeft_Pants.color = pantsColors[randomColorPants];
        spriteFootRight_Pants.color = pantsColors[randomColorPants];
    }
}
