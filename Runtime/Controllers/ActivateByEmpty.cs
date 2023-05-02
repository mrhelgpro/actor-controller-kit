namespace AssemblyActorCore
{
    public class ActivateByEmpty : Activator
    {
        public override void CheckLoop()
        {
            if (stateMachine.IsEmpty)
            {
                TryToActivate();
            }
        }
    }
}