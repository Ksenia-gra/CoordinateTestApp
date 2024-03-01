using CoordinateTestApp.MVVM.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinateTestApp.MVVM.IoC
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel => IoC.GetRequiredService<MainViewModel>();
    }
}
