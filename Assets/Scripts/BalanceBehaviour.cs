using UnityEngine;
using UnityEngine.UI;

public class BalanceBehaviour : MonoBehaviour
{
    [SerializeField] public int balance;
    public Text balanceUI;

    private void Update()
    {
        balanceUI.text = balance.ToString();
    }

    public void SpendMoney(int cost)
    {
        balance -= cost;
    }
    public void AddMoney(int amount)
    {
        balance += amount;
    }
}