using UnityEngine;
using System.Collections;

public class Bot : MonoBehaviour
{
    [SerializeField] private Deck _deck;
    [SerializeField] private Table _table;
    [SerializeField] private Beat _table1;

    private int _suitForBeat;
    private int _amountForBeat;
    private int _cardForBeat;
    private int _cardToBeat;
    int suitBot;
    int amountBot;
    private bool _canBeat;
    public bool checkCard=true;



    public void SearchCardForBeat()

    {
        
        int backChild;


        for (int j = 0; j < _table.transform.childCount; j++)
        {
            if (_table.transform.GetChild(j).GetComponent<Card>().needForBeat)
            {
               
                _suitForBeat = _table.transform.GetChild(j).GetComponent<Card>().dataCard[0];
                _amountForBeat = _table.transform.GetChild(j).GetComponent<Card>().dataCard[1];
                backChild = transform.childCount;
                 if (_suitForBeat == _deck.trump)
                {
                    _amountForBeat += 9;
                    
                }
                for (int i = 0; i < backChild; i++)
                {
                  
                    suitBot = transform.GetChild(i).GetComponent<Card>().dataCard[0];
                    amountBot = transform.GetChild(i).GetComponent<Card>().dataCard[1];
                    if (suitBot == _deck.trump)
                    {
                        suitBot = _suitForBeat;
                        amountBot += 9;//number of Card
                        
                    }
                   
                    
                    
                        _cardForBeat = i;
                        _cardToBeat = j;

                    Beat(_suitForBeat, _amountForBeat, suitBot, amountBot);


                }
            }
        }
        
    }

    private void Beat(int suitOnTable, int amountOnTable,int suitOnHand,int amountOnHand )
    {
        
        
        if (suitOnTable == suitOnHand && amountOnTable < amountOnHand&&checkCard)
        {
           
            if (transform.GetChild(_cardForBeat).GetComponent<Card>().dataCard[0] == _deck.trump)
            {
                int addToArray = amountOnHand - 9;
                
                _deck.AddAmount(addToArray);
            }

                _table.transform.GetChild(_cardToBeat).GetComponent<Card>().needForBeat = false;
            _deck.AddAmount(amountOnHand);
            
                StartCoroutine(Go(_cardForBeat));
            

        }
        else
        {
            int checkTr = transform.childCount - 1;

            if (_cardForBeat == checkTr)
            {

                _canBeat = true;
            }
        }
    }

    public void NoMore()
    {if (_canBeat)
        {
            


                _deck.CheckCardInHand();
                _deck.ClearAmount();
                TakeFromTable();
                _canBeat = false;

            
        }
    }

    private void TakeFromTable()
    {
        int cardOnTable= _table.transform.childCount;
        int cardOnBeat = _table1.transform.childCount;
        for (int i = 0; i < cardOnBeat; i++)
        {
            _table1.transform.GetChild(0).transform.SetParent(transform);
        }
        for (int j = 0; j < cardOnTable; j++)
        {
           
            _table.transform.GetChild(0).transform.SetParent(transform);
        }
    }
    public IEnumerator Go(int trueCard)
    {
        checkCard = false;
        //if (!_table.transform.GetChild(_cardToBeat).GetComponent<Card>().needForBeat)
        yield return new WaitForFixedUpdate();
        Vector3 offSet = new Vector3(0.5f, 0.5f);
        //while (transform.GetChild(_cardForBeat).transform.position != _table.transform.position)
        //{

        //    yield return new WaitForFixedUpdate();
        //    transform.GetChild(_cardForBeat).transform.position = Vector3.MoveTowards(transform.GetChild(_cardForBeat).transform.position, _table.transform.position,3f);
        //}
        transform.GetChild(trueCard).transform.SetParent(_table1.transform);
        
        
    }
}
