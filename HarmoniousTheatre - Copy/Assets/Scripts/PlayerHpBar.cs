using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    [SerializeField] private Image healthbarSprite;

    public void UpdateHpBar(float startHealth, float currentHealth)
    {
        healthbarSprite.fillAmount = currentHealth / startHealth;
    }
}
