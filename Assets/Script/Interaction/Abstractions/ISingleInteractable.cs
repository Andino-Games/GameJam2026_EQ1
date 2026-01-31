namespace Script.Interaction.Abstractions
{
    public interface ISingleInteractable : IInteractable
    {
        public void OnInteract(IInteractor interactor);
    }
}