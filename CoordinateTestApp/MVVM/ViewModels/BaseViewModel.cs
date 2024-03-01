using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CoordinateTestApp.MVVM.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{			
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
