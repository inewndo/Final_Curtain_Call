using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image healthBar;

    public void UpdateHealthBar(float health, float currentHealth)
    {
        healthBar.fillAmount = currentHealth / health;
    }
}
