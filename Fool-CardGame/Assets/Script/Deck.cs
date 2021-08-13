using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using System.Collections;

public class Deck : MonoBehaviour
{
    private int _amountCard = 9;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Sprite[] _suitImage = new Sprite[4]; //0-club 1-spade 2-heart 3 diamond
    [SerializeField] private Transform [] _handPoint; //0-my 1-bot
    [SerializeField] private GameObject _table;
    [SerializeField] private GameObject _beat;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private Transform _deckPoint;
    [SerializeField] private Transform _canvas;
    [SerializeField] private Transform _trump;
    [SerializeField] private CheckStep _checkStep;
    public int[] amountInGame;
    public int trump;
    
    GameObject newCard;
    int randomSuit;
    int randomAmount;

    public Dictionary<int,bool>[] suit = new Dictionary<int,bool>[4];//true-in Deck  false-in hand or hang up
    

    private void Start()
    {
        
        for (int j = 0; j < suit.Length; j++)
        {
            suit[j] = new Dictionary<int, bool>();
            int amount = 6;
            for (int i = 0; i < _amountCard; i++)
            {
                suit[j].Add(amount, true);
                amount++;
            }
        }
        

    }
    public void AddAmount(int amount)
    {
        Array.Resize(ref amountInGame, amountInGame.Length + 1);
        amountInGame[amountInGame.Length - 1] = amount;
    }
    public void ClearAmount()
    {
      Array.Resize(ref amountInGame, 0);
    }
    public void Beat()
    { 
        bool check=false;
    
               
        for (int i = 0; i < _table.transform.childCount; i++)
        {
                      
                if (!check)
                {
                    check = _table.transform.GetChild(i).GetComponent<Card>().needForBeat;
                    
                }
            
        }
        if (!check)
        {
            DestroyBeat();
            ClearAmount();
        }
        
    }

    private void DestroyBeat()
    {
        for (int i = 0; i < _table.transform.childCount; i++)
        {
            Destroy(_table.transform.GetChild(i).gameObject);
        }
        for (int j = 0; j < _beat.transform.childCount; j++)
        {
            Destroy(_beat.transform.GetChild(j).gameObject);
            
        }
        CheckCardInHand();
    }

    public void Take()
    {
        CheckCardInHand();
    }
    public void PullCard()
    {
        _startButton.SetActive(false);
        StartCoroutine(Turn());
    }
    private void Trump()
    {
        Vector3 spawnPosition = _trump.position+new Vector3(1,0,0);
        newCard= Instantiate(_cardPrefab, spawnPosition, Quaternion.Euler(0, 0, -90));
        
        newCard.transform.SetParent(_canvas, false);
        newCard.transform.position = spawnPosition;
        _trump.SetParent(newCard.transform);
        
        AppointCard();

        trump = newCard.GetComponent<Card>().dataCard[0];

        
    }

    public IEnumerator Go(int numberHand)
    {
        
        while (newCard.transform.position != _handPoint[numberHand].position)
        {

            yield return new WaitForFixedUpdate();
            newCard.transform.position = Vector3.MoveTowards(newCard.transform.position, _handPoint[numberHand].position, 3f);
        }
        newCard.transform.SetParent(_handPoint[numberHand]);
        
    }
    IEnumerator Turn()
    {
        int numberHand=0;
        for (int i = 0; i < 12; i++)
        {
            switch (numberHand)
            {
                case 0:
                    numberHand = 1;
                    break;
                case 1:
                    numberHand = 0;
                    break;

            }
            TakeCard(1,numberHand);
            
            yield return StartCoroutine(Go(numberHand));
        }
        Trump();
    }

    public void CheckCardInHand()
    {
        StartCoroutine(Check());
        IEnumerator Check()
        {
            int fullHand = 6;
            for (int i = 0; i < 2; i++)
            {
                int neededCard = fullHand - _handPoint[i].childCount;

                if (neededCard > 0)
                {
                    for (int j = 0; j < neededCard; j++)
                    {
                        TakeCard(1, i);
                        yield return StartCoroutine(Go(i));
                    }
                }
            }
        }
        
    }
    private void TakeCard(int quantity,int numberHand)
    {
        StartCoroutine(Wait());
        IEnumerator Wait()
        {
            
            for (int i = 0; i < quantity; i++)
            {
                
                newCard = Instantiate(_cardPrefab, _deckPoint.position, Quaternion.identity);
                newCard.transform.SetParent(_canvas, false);
                AppointCard();
                newCard.transform.position = new Vector3(newCard.transform.position.x, newCard.transform.position.x, 0);
                StartCoroutine(Go(numberHand));
                yield return new WaitForEndOfFrame();

            }
        }
    }
    private void AppointCard()
    {

        bool toRemove = false; ;
        
        string amount = "";
        do
        {
            randomSuit = UnityEngine.Random.Range(0, 4);

            randomAmount = UnityEngine.Random.Range(6, 15);
            suit[randomSuit].TryGetValue(randomAmount, out toRemove);

        }
        while (!toRemove);

        newCard.transform.GetChild(0).GetComponent<Image>().sprite = _suitImage[randomSuit];
        newCard.GetComponent<Card>().dataCard[0] = randomSuit;
        newCard.GetComponent<Card>().dataCard[1] = randomAmount;
        switch (randomAmount)
        {
            case 11:
                amount = "J";
                break;
            case 12:
                amount = "Q";
                break;
            case 13:
                amount = "K";
                break;
            case 14:
                amount = "A";
                break;
            default:
                amount = Convert.ToString(randomAmount);
                break;
        }
        newCard.transform.GetChild(1).GetComponent<Text>().text = amount;
        suit[randomSuit].Remove(randomAmount);

    }
}
    

    


