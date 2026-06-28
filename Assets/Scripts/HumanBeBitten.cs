using UnityEngine;

public class HumanBeBitten : MonoBehaviour
{

    public int bloodAmount;

    public void Interact()
    {
        //Put the human explode animation here



        //Above here. For reload go to Player Controller and it should be at the bottom
        bloodAmount = 5;
        
        GameObject myObject = gameObject;
        Destroy(gameObject);
    }
}
