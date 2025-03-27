using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class PurchaseFulfilment : MonoBehaviour
{
    [SerializeField] private LifeManager lifeManager;

    public void PurchaseCompleted(Product product)
    {
        switch (product.definition.id)
        {
            case "product1":
                lifeManager.GainLife(1);
                break;
            case "product2":
                lifeManager.GainLife(5);
                break;
            case "product3":
                lifeManager.GainLife(10);
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
