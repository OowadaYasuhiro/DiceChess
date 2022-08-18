using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PaseImageController : MonoBehaviour
{
    [SerializeField] private Image paseImage;
    private Sprite sprite;
    [SerializeField] private Sprite PawnWhite;
    [SerializeField] private Sprite PawnBlock;
    [SerializeField] private Sprite RookWhite;
    [SerializeField] private Sprite RookBlock;
    [SerializeField] private Sprite KnightWhite;
    [SerializeField] private Sprite KnightBlock;
    [SerializeField] private Sprite BishopWhite;
    [SerializeField] private Sprite BishopBlock;
    [SerializeField] private Sprite QueenWhite;
    [SerializeField] private Sprite QueenBlock;
    [SerializeField] private Sprite KingWhite;
    [SerializeField] private Sprite KingBlock;

    // Start is called before the first frame update
    void Start()
    {
 
    }

    public void PaceCanChanger(int x,int y)
    {
        if (x == 1 && y == 0) { sprite = PawnWhite; }
        else if (x == 1 && y == 1) { sprite = PawnBlock; }
        else if (x == 2 && y == 0) { sprite = RookWhite; }
        else if (x == 2 && y == 1) { sprite = RookBlock; }
        else if (x == 3 && y == 0) { sprite = KnightWhite; }
        else if (x == 3 && y == 1) { sprite = KnightBlock; }
        else if (x == 4 && y == 0) { sprite = BishopWhite; }
        else if (x == 4 && y == 1) { sprite = BishopBlock; }
        else if (x == 5 && y == 0) { sprite = QueenWhite; }
        else if (x == 5 && y == 1) { sprite = QueenBlock; }
        else if(x == 6 && y == 0) { sprite = KingWhite; }
        else if (x == 6 && y == 1) { sprite = KingBlock; }
        paseImage = this.GetComponent<Image>();
        paseImage.sprite = sprite;
    }
}
