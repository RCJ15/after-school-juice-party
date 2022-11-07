using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Weapon HUD (Heads-Up Display). Displays your currently equipped weapons.
/// </summary>
public class WeaponHUD : MonoBehaviour // - Ruben
{
    public static WeaponHUD Instance;

    [SerializeField] private WeaponCard template;
    [SerializeField] private Transform content;
    [SerializeField] private float offset;
    [SerializeField] private float selectedYPos;
    private float _startYPos;

    [Space]
    [SerializeField] private int maxCardsBeforeScroll = 3;
    [SerializeField] private float lerpValue = 0.2f;

    [Space]
    [SerializeField] private GameObject tutorialText;

    public TMPro.VertexGradient TemplateGradient { get; private set; }

    private List<WeaponCard> _cards = new List<WeaponCard>();
    private int _cardCount;
    public int SelectedCard { get; set; }
    private float _currentScrollPos;
    private float _targetScrollPos;

    private float _xPos;

    private void Awake()
    {
        Instance = this;

        _startYPos = template.transform.localPosition.y;

        TemplateGradient = template.GetGradient();
        template.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_cardCount > maxCardsBeforeScroll)
        {
            _targetScrollPos = offset * Mathf.Clamp(SelectedCard, 1, _cardCount - 2);
        }

        _currentScrollPos = Mathf.Lerp(_currentScrollPos, -_targetScrollPos, lerpValue);

        content.localPosition = new Vector3(_currentScrollPos, 0);

        for (int i = 0; i < _cardCount; i++)
        {
            bool selected = i == SelectedCard;

            WeaponCard card = _cards[i];
            Vector3 pos = card.transform.localPosition;

            pos.y = Mathf.Lerp(pos.y, selected ? selectedYPos : _startYPos, lerpValue);

            card.transform.localPosition = pos;

            card.transform.rotation = Quaternion.Slerp(card.transform.rotation, Quaternion.Euler(0, selected ? 360 : 0, 0), lerpValue);
        }
    }

    public void AddCard(PlayerShoot weapon, bool playAnim = true)
    {
        GameObject newObj = Instantiate(template.gameObject, Vector3.zero, Quaternion.identity, template.transform.parent);

        newObj.transform.localPosition = new Vector3(_xPos, selectedYPos, 0);

        _xPos += offset;

        if (_cardCount <= maxCardsBeforeScroll)
        {
            _targetScrollPos = (_xPos - offset) / 2;
        }

        newObj.SetActive(true);

        WeaponCard newCard = newObj.GetComponent<WeaponCard>();
        _cards.Add(newCard);
        _cardCount++;

        if (_cardCount == 2)
        {
            tutorialText.SetActive(true);
        }

        SetCard(newCard, weapon, false);

        if (playAnim)
        {
            newCard.PlaySpawnAnim();
        }
    }

    public void DisappearTutorialText()
    {
        tutorialText.SetActive(false);
    }

    public void SetCard(int index, PlayerShoot weapon, bool doAnim = true)
    {
        SetCard(_cards[index], weapon, doAnim);
    }

    public void SetCard(WeaponCard card, PlayerShoot weapon, bool doAnim = true)
    {
        card.Set(weapon, doAnim);
    }
}
