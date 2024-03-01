using CoordinateTestApp.MVVM.IoC;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CoordinateTestApp
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		protected override void OnStartup(StartupEventArgs e)
		{
			FrameworkCompatibilityPreferences.KeepTextBoxDisplaySynchronizedWithTextProperty = false;
			IoC.Init();
			base.OnStartup(e);
		}
	}

}
