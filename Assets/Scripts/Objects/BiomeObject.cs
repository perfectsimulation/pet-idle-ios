using UnityEngine;

public class BiomeObject : MonoBehaviour
{
    public Biome Biome;

    private bool IsOpen;

    public void OpenBiomeToGuests()
    {
        this.IsOpen = true;
    }

    public bool IsBiomeOpenToGuests()
    {
        return this.IsOpen;
    }

}
