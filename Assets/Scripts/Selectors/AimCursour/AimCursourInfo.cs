using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimCursourInfo : MonoBehaviour
{
    [SerializeField] private List<GameCard> cards = new List<GameCard>();

    public bool HasGameType<T>() where T : GameCard
    {
        foreach (var card in cards)
            if (card is T)
                return true;

        return false;
    }

    public void AddCard(GameCard card)
    {
        cards.Add(card);
    }

    public bool HasGameType(GameCard gameCard)
    {
        foreach (var card in cards)
            if (card.GetType().Name.Equals(gameCard.GetType().Name))
                return true;

        return false;
    }
}
