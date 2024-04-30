/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:EDKGC"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using EDKGC.ViewModel.CentralSolutions;
using EDKGC.ViewModel.ISO27001;
using EDKGC.ViewModel.SatelliteWindows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace EDKGC.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<CentralViewModel>();
           SimpleIoc.Default.Register<ConfirmationWindowViewModel>();
           SimpleIoc.Default.Register<ISOViewModel>();





        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();
        public CentralViewModel Central => ServiceLocator.Current.GetInstance<CentralViewModel>();
        public ConfirmationWindowViewModel ConfirmationViewM => ServiceLocator.Current.GetInstance<ConfirmationWindowViewModel>();
        public ISOViewModel IsoViewModelInstance => SimpleIoc.Default.GetInstance<ISOViewModel>();
        public ISOViewModel Iso => SimpleIoc.Default.GetInstance<ISOViewModel>();


        public static void Cleanup()
        {
            // TODO Clear the ViewModels
            
        }
    }
}