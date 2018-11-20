using UnityEngine;

// This script is a quick hacky solution. The layout, which displays the 'used programs' is now recalculated whenever a skill is selected.
// This way a weird scaling issue caused by changing the game's resolution, is only temporary.
public class ReactivateLayout : MonoBehaviour
{

    private void OnEnable()
    {
        CustomLayout layout = GetComponentInChildren<CustomLayout>();
        if(layout != null)
        {
            layout.SetLayout();
        }
    }
}
