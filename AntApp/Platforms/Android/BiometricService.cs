using AndroidX.Core.Content;
using AndroidX.Biometric;
using AndroidX.AppCompat.App;
using Java.Lang;

namespace AntApp;

public class BiometricService
{
    public static Task<bool> GetAuthenticationStatus()
    {
        if (Platform.CurrentActivity is not AppCompatActivity activity)
        {
            return Task.FromResult(false);
        }

        var biometricManager = BiometricManager.From(activity);
        var canAuthenticate = biometricManager.CanAuthenticate(BiometricManager.Authenticators.BiometricStrong); // Fingerprint only
        var response = canAuthenticate switch
        {
            BiometricManager.BiometricSuccess => true,
            _ => false,
        };

        return Task.FromResult(response);
    }

    public static async Task<bool> Authenticate(CancellationToken token)
    {
        if (Platform.CurrentActivity is not AppCompatActivity activity)
        {
            return false;
        }

        try
        {
            var promptBuilder = new BiometricPrompt.PromptInfo.Builder()
                    .SetTitle("Ant auth")
                    .SetSubtitle("Is this you?")
                    .SetNegativeButtonText("Cancel")
                    .SetAllowedAuthenticators(BiometricManager.Authenticators.BiometricStrong); // Fingerprint only

            var promptInfo = promptBuilder.Build();
            var executor = ContextCompat.GetMainExecutor(activity);
            var authCallback = new BioAuthCallback()
            {
                Response = new TaskCompletionSource<bool>()
            };

            var biometricPrompt = new BiometricPrompt(activity, executor, authCallback);

            await using (token.Register(biometricPrompt.CancelAuthentication))
            {
                biometricPrompt.Authenticate(promptInfo);
                var response = await authCallback.Response.Task;
                return response;
            }
        }
        catch
        {
            return false;
        }
    }
}

public class BioAuthCallback : BiometricPrompt.AuthenticationCallback
{
    public required TaskCompletionSource<bool> Response { get; set; }

    public override void OnAuthenticationSucceeded(BiometricPrompt.AuthenticationResult result)
    {
        base.OnAuthenticationSucceeded(result);
        Response.TrySetResult(true);
    }

    public override void OnAuthenticationError(int errorCode, ICharSequence errString)
    {
        base.OnAuthenticationError(errorCode, errString);
        Response.TrySetResult(false);
    }
}