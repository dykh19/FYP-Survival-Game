using UnityEngine;

[CreateAssetMenu(fileName = "New Placeable Item", menuName = "Placeable Item")]
public class PlaceableItem : GameItem
{
    public GameObject prefab;

    public override void OnHoldEnter()
    {
        // TODO: Display an outline of the specified mesh where the player
        //       is pointing to. Use a Raycast and Camera.ScreenToWorldPoint.
    }

    public override void OnHoldStay()
    {
        // TODO: Keep the mesh outline following where the player is pointing to.
    }

    public override void OnHoldExit()
    {
        // TODO: Remove the mesh outline.
    }

    public override void OnUse()
    {
        // TODO: Place the actual object prefab on click.
    }
}
