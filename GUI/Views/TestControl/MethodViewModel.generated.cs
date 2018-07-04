﻿//---------------------------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated by T4Model template for T4 (https://github.com/linq2db/linq2db).
//    Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
//---------------------------------------------------------------------------------------------------
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PerformanceTest.Views.TestControl
{
	public partial class MethodViewModel : INotifyPropertyChanged
	{
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

					OnDisplayNameChanged();
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

		#region Repeat : long

		private long _repeat;
		public  long  Repeat
		{
			get { return _repeat; }
			set
			{
				if (_repeat != value)
				{
					BeforeRepeatChanged(value);
					_repeat = value;
					AfterRepeatChanged();

					OnDisplayNameChanged();
				}
			}
		}

		#region INotifyPropertyChanged support

		partial void BeforeRepeatChanged(long newValue);
		partial void AfterRepeatChanged ();

		public const string NameOfRepeat = "Repeat";

		private static readonly PropertyChangedEventArgs _repeatChangedEventArgs = new PropertyChangedEventArgs(NameOfRepeat);

		private void OnRepeatChanged()
		{
			OnPropertyChanged(_repeatChangedEventArgs);
		}

		#endregion

		#endregion

		#region Take : long?

		private long? _take;
		public  long?  Take
		{
			get { return _take; }
			set
			{
				if (_take != value)
				{
					BeforeTakeChanged(value);
					_take = value;
					AfterTakeChanged();

					OnDisplayNameChanged();
				}
			}
		}

		#region INotifyPropertyChanged support

		partial void BeforeTakeChanged(long? newValue);
		partial void AfterTakeChanged ();

		public const string NameOfTake = "Take";

		private static readonly PropertyChangedEventArgs _takeChangedEventArgs = new PropertyChangedEventArgs(NameOfTake);

		private void OnTakeChanged()
		{
			OnPropertyChanged(_takeChangedEventArgs);
		}

		#endregion

		#endregion

		#region DisplayName : string

		public string DisplayName
		{
			get { return Name + " / " + Repeat + (Take.HasValue ? " / " + Take : ""); }
		}

		#region INotifyPropertyChanged support

		public const string NameOfDisplayName = "DisplayName";

		private static readonly PropertyChangedEventArgs _displayNameChangedEventArgs = new PropertyChangedEventArgs(NameOfDisplayName);

		private void OnDisplayNameChanged()
		{
			OnPropertyChanged(_displayNameChangedEventArgs);
		}

		#endregion

		#endregion

		#region Times : ObservableCollection<TimeViewModel>

		private ObservableCollection<TimeViewModel> _times;
		public  ObservableCollection<TimeViewModel>  Times
		{
			get { return _times; }
			set
			{
				if (_times != value)
				{
					BeforeTimesChanged(value);
					_times = value;
					AfterTimesChanged();

					OnTimesChanged();
				}
			}
		}

		#region INotifyPropertyChanged support

		partial void BeforeTimesChanged(ObservableCollection<TimeViewModel> newValue);
		partial void AfterTimesChanged ();

		public const string NameOfTimes = "Times";

		private static readonly PropertyChangedEventArgs _timesChangedEventArgs = new PropertyChangedEventArgs(NameOfTimes);

		private void OnTimesChanged()
		{
			OnPropertyChanged(_timesChangedEventArgs);
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
