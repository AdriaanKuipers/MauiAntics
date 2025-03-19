namespace AntApp;

public partial class App : Application
{
	private readonly BiometricService _biometric;
	private readonly LifecycleService _lifecycle;

	public App(BiometricService biometric, LifecycleService lifecycle)
	{
		InitializeComponent();

		MainPage = new NavigationPage(new MainPage());
		_biometric = biometric;
		_lifecycle = lifecycle;
	}

	protected override Window CreateWindow(IActivationState activationState)
	{
		Window window = base.CreateWindow(activationState);

		window.Deactivated += _lifecycle.OnDeactivated;

		return window;
	}
}
