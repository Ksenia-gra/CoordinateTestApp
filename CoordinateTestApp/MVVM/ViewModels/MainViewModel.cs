using CoordinateTestApp.Data.Models;
using CoordinateTestApp.Repository;
using RemoteControlWPFClient.MVVM.Command;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using static CoordinateTestApp.MVVM.IoC.IoC;
using Point = CoordinateTestApp.Data.Models.Point;

namespace CoordinateTestApp.MVVM.ViewModels
{
	public class MainViewModel : BaseViewModel, ITransient
	{
		private const double MaxLength = 10;
		private readonly IPointRepository pointRepository;

        public ObservableCollection<Point> Points { get; set; }

		public double? NewX { get; set; }

		public double? NewY { get; set; }

		public string AxisLength { get; set; }

		public ICommand AddPointsCommand => new RelayCommand(AddPointsAsync, ValidatePoint);

		public ICommand LoadPointsCommand => new AwaitableCommand(LoadPointsAsync);

		public ICommand SavePointsCommand => new AwaitableCommand(SavePointsAsync);

		public ICommand DeleteDataCommand => new AwaitableCommand(DeleteDataAsync);

		public MainViewModel(IPointRepository pointRepository)
		{
			this.pointRepository = pointRepository;
			Points = new ObservableCollection<Point>();
			AxisLength = MaxLength.ToString();
		}

		private void AddPointsAsync()
		{
			Points.Add(new Point(NewX.Value, NewY.Value));
			NewX = NewY = null;
		}

		private async Task LoadPointsAsync()
		{
			try
			{
				foreach (Point point in await pointRepository.GetAllAsync())
				{
					Points.Add(point);
				}
			}
			catch (IOException ioEx)
			{
				MessageBox.Show("При открытии файла произошла ошибка ввода-вывода.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private async Task SavePointsAsync()
		{
			try
			{
				await pointRepository.InsertAsync(Points);
			}	
			catch (IOException ioEx)
			{
				MessageBox.Show("При открытии файла произошла ошибка ввода-вывода.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private async Task DeleteDataAsync()
		{
			try
			{
				Points.Clear();
				await pointRepository.DeleteAsync();
			}
			catch (IOException ioEx)
			{
				MessageBox.Show("При открытии файла произошла ошибка ввода-вывода.", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private bool ValidatePoint()
		{
			return NewX != null && NewY != null && 
				Math.Abs(NewX.Value) <= MaxLength && Math.Abs(NewY.Value) <= MaxLength && 
				!Points.Contains(new Point(NewX.Value, NewY.Value));
		}
	}
}
