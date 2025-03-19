namespace AntApp;

public class LifecycleService(BiometricService biometric)
{
    public event Action Deactivated;

    public void OnDeactivated(object pSender, EventArgs pArgs)
    {
        biometric.FingersConfirmed = false;
        Deactivated?.Invoke();
    }
}