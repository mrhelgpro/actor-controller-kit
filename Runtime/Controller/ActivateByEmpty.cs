namespace AssemblyActorCore
{
    public class ActivateByEmpty : Activator
    {
        public override void UpdateActivate()
        {
            if (presenterMachine.IsEmpty)
            {
                TryToActivate();
            }
        }
    }
}
