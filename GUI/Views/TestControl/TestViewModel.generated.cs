﻿//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/linq2db).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------

#pragma warning disable 1591

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PerformanceTest.Views.TestControl
{
	public partial class TestViewModel : INotifyPropertyChanged
	{
		#region Platform : string

		private string _platform;
		public  string  Platform
		{
			get { return _platform; }
			set
			{
				if (_platform != value)
				{
					BeforePlatformChanged(value);
					_platform = value;
					AfterPlatformChanged();

					OnPlatformChanged();
				}
			}
		}

		#region INotifyPropertyChanged support

		partial void BeforePlatformChanged(string newValue);
		partial void AfterPlatformChanged ();

		public const string NameOfPlatform = "Platform";

		private static readonly PropertyChangedEventArgs _platformChangedEventArgs = new PropertyChangedEventArgs(NameOfPlatform);

		private void OnPlatformChanged()
		{
			OnPropertyChanged(_platformChangedEventArgs);
		}

		#endregion

		#endregion

		#region Name : string

		private string _name;
		public  string  Name
		{
			get { return _name; }
			set
			{
				if (_name != value)
				{
					BeforeNameChanged(value);
					_name = value;
					AfterNameChanged();

					OnNameChanged();
				}
			}
		}

		#region INotifyPropertyChanged support

		partial void BeforeNameChanged(string newValue);
		partial void AfterNameChanged ();

		public const string NameOfName = "Name";

		private static readonly PropertyChangedEventArgs _nameChangedEventArgs = new PropertyChangedEventArgs(NameOfName);

		private void OnNameChanged()
		{
			OnPropertyChanged(_nameChangedEventArgs);
		}

		#endregion

		#endregion

		#region Methods : ObservableCollection<MethodViewModel>

		private ObservableCollection<MethodViewModel> _methods;
		public  ObservableCollection<MethodViewModel>  Methods
		{
			get { return _methods; }
			set
			{
				if (_methods != value)
				{
					BeforeMethodsChanged(value);
					_methods = value;
					AfterMethodsChanged();

					OnMethodsChanged();
				}
			}
		}

		#region INotifyPropertyChanged support

		partial void BeforeMethodsChanged(ObservableCollection<MethodViewModel> newValue);
		partial void AfterMethodsChanged ();

		public const string NameOfMethods = "Methods";

		private static readonly PropertyChangedEventArgs _methodsChangedEventArgs = new PropertyChangedEventArgs(NameOfMethods);

		private void OnMethodsChanged()
		{
			OnPropertyChanged(_methodsChangedEventArgs);
		}

		#endregion

		#endregion

		#region Providers : ObservableCollection<ProviderViewModel>

		private ObservableCollection<ProviderViewModel> _providers;
		public  ObservableCollection<ProviderViewModel>  Providers
		{
			get { return _providers; }
			set
			{
				if (_providers != value)
				{
					BeforeProvidersChanged(value);
					_providers = value;
					AfterProvidersChanged();

					OnProvidersChanged();
				}
			}
		}

		#region INotifyPropertyChanged support

		partial void BeforeProvidersChanged(ObservableCollection<ProviderViewModel> newValue);
		partial void AfterProvidersChanged ();

		public const string NameOfProviders = "Providers";

		private static readonly PropertyChangedEventArgs _providersChangedEventArgs = new PropertyChangedEventArgs(NameOfProviders);

		private void OnProvidersChanged()
		{
			OnPropertyChanged(_providersChangedEventArgs);
		}

		#endregion

		#endregion

		#region INotifyPropertyChanged support

#if !SILVERLIGHT
		[field : NonSerialized]
#endif
		public virtual event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			var propertyChanged = PropertyChanged;

			if (propertyChanged != null)
			{
#if SILVERLIGHT
				if (System.Windows.Deployment.Current.Dispatcher.CheckAccess())
					propertyChanged(this, new PropertyChangedEventArgs(propertyName));
				else
					System.Windows.Deployment.Current.Dispatcher.BeginInvoke(
						() =>
						{
							var pc = PropertyChanged;
							if (pc != null)
								pc(this, new PropertyChangedEventArgs(propertyName));
						});
#else
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
#endif
			}
		}

		protected void OnPropertyChanged(PropertyChangedEventArgs arg)
		{
			var propertyChanged = PropertyChanged;

			if (propertyChanged != null)
			{
#if SILVERLIGHT
				if (System.Windows.Deployment.Current.Dispatcher.CheckAccess())
					propertyChanged(this, arg);
				else
					System.Windows.Deployment.Current.Dispatcher.BeginInvoke(
						() =>
						{
							var pc = PropertyChanged;
							if (pc != null)
								pc(this, arg);
						});
#else
				propertyChanged(this, arg);
#endif
			}
		}

		#endregion
	}
}

#pragma warning restore 1591
