using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardsSystem : MonoBehaviour
{
    public static CardsSystem singleton;

    [SerializeField] private Transform spawnCardTranform;

    [SerializeField] private List<GameObject> prefabsAllCards = new List<GameObject>();

    [SerializeField] private List<GameCard> handedCards = new List<GameCard>();

    [Header("Spawn settings")]
    [SerializeField] private float distanceBettwenCards = 5f;
    [SerializeField] private float offSetRight = 5f;

    private void Awake()
    {
        singleton = this;
    }

    private void Start()
    {
        if (spawnCardTranform == null)
            throw new System.Exception("Card spawn position is null");

        foreach (var pref in prefabsAllCards)
        {
            var gc = pref.GetComponent<GameCard>();

            if (gc == null)
                throw new System.Exception($"{typeof(CardsSystem).Name} have a false prefab in 'prefabsAllCards'");
        }

        //StartCoroutine(GetStartCards());

    }

    public GameCard GetPatternCard<T>() where T : GameCard
    {
        foreach (var card in prefabsAllCards)
        {
            var gc = card.GetComponent<GameCard>();
            if (gc is T)
                return gc;
        }

        return null;
    }

    public void SwitchCard(GameCard first, GameCard second)
    {
        if (handedCards.Contains(first) && handedCards.Contains(second))
        {
            var sI = handedCards.IndexOf(second);

            handedCards.Remove(first);
            handedCards.Insert(sI, first);

            RecalculateCardsPositions();
        }
    }

    public void AddCard<T>() where T : GameCard
    {
        foreach (var pref in prefabsAllCards)
        {
            var gc = pref.GetComponent<GameCard>();

            if (gc != null && gc is T)
            {
                var card = SpawnCard(pref);
                handedCards.Add(card);
                return;
            }
        }
    }

    public void AddCard(GameObject cardPref)
    {
        var card = SpawnCard(cardPref);

        handedCards.Add(card);
    }

    public void RemoveCard(GameCard cardObject)
    {
        if (handedCards.Contains(cardObject))
        {
            handedCards.Remove(cardObject);
            Destroy(cardObject.gameObject);
            RecalculateCardsPositions();
        }
    }

    private void RecalculateCardsPositions()
    {
        for (var i = 0; i < handedCards.Count; i++)
        {
            var offSet = (Vector3.right * (i * distanceBettwenCards)) + (Vector3.right * offSetRight) + Vector3.forward * 0.1f * i;

            handedCards[i].SetStartPosition(offSet);
            handedCards[i].transform.SetParent(transform);
            handedCards[i].transform.SetParent(spawnCardTranform);
        }


    }

    private GameCard SpawnCard(GameObject pref)
    {
        var offSet = spawnCardTranform.position;
        offSet += (Vector3.right * (handedCards.Count * distanceBettwenCards)) + (Vector3.right * offSetRight) + Vector3.forward * 0.1f * handedCards.Count;

        var card = Instantiate(pref, spawnCardTranform);
        card.GetComponent<RectTransform>().localPosition = offSet;

        //card.GetComponent<GameCard>().SetStartPosition(offSet);
        //card.transform.SetParent(spawnCardTranform);

        return card.GetComponent<GameCard>();
    }

    private IEnumerator GetStartCards()
    {
        yield return new WaitForSeconds(0.3f);
        AddCard<PlagueDoctorCard>();

        yield return new WaitForSeconds(0.1f);

        AddCard<PlagueDoctorCard>();
        yield return new WaitForSeconds(0.1f);

        AddCard<PlagueDoctorCard>();
        yield return new WaitForSeconds(0.1f);

        AddCard<KnightCard>();
        yield return new WaitForSeconds(0.1f);

        AddCard<KnightCard>();

    }
}
