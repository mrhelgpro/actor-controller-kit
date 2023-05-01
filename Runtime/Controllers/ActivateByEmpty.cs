namespace AssemblyActorCore
{
    public class ActivateByEmpty : Activator
    {
        public override void UpdateActivate()
        {
            if (stateMachine.IsEmpty)
            {
                TryToActivate();
            }
        }
    }
}