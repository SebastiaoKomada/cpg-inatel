using UnityEngine;

public enum PickupType { Weapon, Letter }

public class ItemPickup : MonoBehaviour
{
    [Header("Pickup Settings")]
    public PickupType pickupType;
    [TextArea]
    public string letterText;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        switch (pickupType)
        {
            case PickupType.Weapon:
                var gun = other.GetComponentInChildren<Gun>();
                if (gun != null)
                    gun.Equip();
                    Destroy(gameObject);

                break;

            case PickupType.Letter:
                DialogueManager.Instance.StartDialogue(letterText);
                break;
        }

    }
}