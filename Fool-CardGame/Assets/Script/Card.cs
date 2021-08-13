using UnityEngine;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera _camera;
    private GameObject _table;
    private Deck _deck;
    private Vector3 _offSet;
    public Transform DefaultParent;
    public int[] dataCard = new int[2];//0-suit 1-amount
    public bool needForBeat=false;
    private GameObject _bot;
    private bool _canMove;
    
    private void Awake()
    {
        _camera = Camera.allCameras[0];
    }
    private void Start()
    {
        _table = GameObject.Find("Table");
        _bot = GameObject.Find("Bot");
        _deck = GameObject.Find("Main Camera").GetComponent<Deck>();
    }

   
    public void OnBeginDrag(PointerEventData eventData)
    {   if (_deck.amountInGame.Length == 0)
        {
            _offSet = transform.position - _camera.ScreenToWorldPoint(eventData.position);
            PushCard();
            _deck.AddAmount(dataCard[1]);
            
        }
        foreach (int i in _deck.amountInGame)
        {
            if (i == dataCard[1])
            {
                _offSet = transform.position - _camera.ScreenToWorldPoint(eventData.position);
                PushCard();
            }
        }
    }

    private void PushCard()
    {
        _canMove = true;
        DefaultParent = transform.parent;
        transform.SetParent(DefaultParent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_canMove)
        {
            Vector3 newPosition = _camera.ScreenToWorldPoint(eventData.position);
            newPosition.z = 0;
            transform.position = newPosition + _offSet;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_canMove)
        {
            Vector2 addRange = new Vector2(4, 1);
            if (transform.position.y <= _table.transform.position.y + addRange.y && transform.position.y >= _table.transform.position.y - addRange.y)
            {
                
                needForBeat = true;
                transform.SetParent(_table.transform, false);
                transform.SetParent(DefaultParent);
                GetComponent<CanvasGroup>().blocksRaycasts = true;

                _bot.GetComponent<Bot>().checkCard = true;
                    _bot.GetComponent<Bot>().SearchCardForBeat();

                
            }
            
        }
    }
}
