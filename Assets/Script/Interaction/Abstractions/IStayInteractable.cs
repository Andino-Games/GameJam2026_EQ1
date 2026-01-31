namespace Script.Interaction.Abstractions
{
    public interface IStayInteractable : IInteractable
    {
        public void OnInteractStart(IInteractor interactor);
        public void OnInteractEnd(IInteractor interactor);
    }
}