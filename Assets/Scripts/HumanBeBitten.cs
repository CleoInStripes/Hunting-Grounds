using UnityEngine;

public class HumanBeBitten : MonoBehaviour
{

    public int bloodAmount;

    public void Interact()
    {
        Debug.Log("Interact");
        bloodAmount = 5;

    }
}
