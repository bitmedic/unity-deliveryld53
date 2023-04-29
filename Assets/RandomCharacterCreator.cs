using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCharacterCreator : MonoBehaviour
{
    public SpriteRenderer spriteHead;
    public SpriteRenderer spriteBody;
    public SpriteRenderer spriteFootLeft;
    public SpriteRenderer spriteFootRight;
    public SpriteRenderer spriteArmLeft;
    public SpriteRenderer spriteArmRight;

    public List<Sprite> spritesHead;
    public List<Sprite> spritesBody;
    public List<Sprite> spritesFootLeft;
    public List<Sprite> spritesFootRight;
    public List<Sprite> spritesArmLeft;
    public List<Sprite> spritesArmRight;

    void Start()
    {
        int randomHead = Random.Range(0, spritesHead.Count);
        int randomBody = Random.Range(0, spritesBody.Count);
        int randomFoot = Random.Range(0, spritesFootLeft.Count);
        int randomArm = Random.Range(0, spritesArmLeft.Count);

        spriteHead.sprite = spritesHead[randomHead];
        spriteBody.sprite = spritesBody[randomBody];
        spriteFootLeft.sprite = spritesFootLeft[randomFoot];
        spriteFootRight.sprite = spritesFootRight[randomFoot];
        spriteArmLeft.sprite = spritesArmLeft[randomArm];
        spriteArmRight.sprite = spritesArmRight[randomArm];
    }
}
