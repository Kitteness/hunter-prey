using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using System.Collections;

public class PurchaseFulfilment : MonoBehaviour
{
    [SerializeField] private LifeManager lifeManager;
    [SerializeField] private GameObject beanieButton;
    [SerializeField] private GameObject beanie;

    public void PurchaseCompleted(Product product)
    {
        switch (product.definition.id)
        {
            case "product1":
                lifeManager.GainLife(1);
                AnalyticsManager.Instance.lifePurchaseEvent(1);
                break;
            case "product2":
                lifeManager.GainLife(5);
                AnalyticsManager.Instance.lifePurchaseEvent(5);
                break;
            case "product3":
                PlayerPrefs.SetInt("BeaniePurchased", 1);
                beanie.SetActive(true);
                break;
            default:
                print("Unmatched product ID");
                break;
        }
    }

    public void PurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        print($"Purchase failed - Product: {product.definition.id}," +
              $"Failure reason: {failureDescription.reason}," +
              $"Failure details: {failureDescription.message}.");
    }
}
