using UnityEngine;

public class CheckStep : MonoBehaviour
{
    private GameObject[] _hands;//0-my 1-bot;
    [SerializeField] private Deck _deck;
    private void Start()
    {
        _hands[0] = GameObject.Find("Hand"); 
        _hands[1] = GameObject.Find("Bot");
    }


    public GameObject FirstStep()
    {
        int hightCardMy=0;
        int hightCardBot=0;
        for (int i = 0; i < _hands[0].transform.childCount; i++)
        {
            if(_hands[0].transform.GetChild(i).TryGetComponent(out Card card))
            { 
                if (card.dataCard[0] == _deck.trump && card.dataCard[1] > hightCardBot)
                {
                    hightCardMy = card.dataCard[1];
                }
            }
            
        }
        for (int i = 0; i < _hands[1].transform.childCount; i++)
        {
            if (_hands[0].transform.GetChild(i).TryGetComponent(out Card card))
            {
                if (card.dataCard[0] == _deck.trump && card.dataCard[1] > hightCardBot)
                {
                    hightCardBot = card.dataCard[1];
                }
            }

        }
        if (hightCardMy > hightCardBot)
        {
            return _hands[0];
        }
        else
        {
            return _hands[1];
        }
    }

}
