using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDKGC.ViewModel.ISO27001
{
    public class DataTable : INotifyPropertyChanged
    {
        private string _threatEvent;
        private double _sle;
        private float _ef;
        private RateOfOccurrence _rateOfOccurrence;
        private double _aro;
        private double _ale;

        public DataTable()
        {
            _rateOfOccurrence = new RateOfOccurrence();
            _rateOfOccurrence.PropertyChanged += RateOfOccurrence_PropertyChanged;
        }
        private void RateOfOccurrence_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RateOfOccurrence.FirstValue) || e.PropertyName == nameof(RateOfOccurrence.SecondValue) || e.PropertyName == nameof(RateOfOccurrence.Unit))
            {
                CalculateAro();
            }
        }
        public string ThreatEvent
        {
            get => _threatEvent;
            set
            {
                _threatEvent = value;
                NotifyPropertyChanged(nameof(ThreatEvent));
            }
        }

        public double SLE
        {
            get => _sle;
            set
            {
                _sle = value;
                NotifyPropertyChanged(nameof(SLE));
            }
        }

        public float EF
        {
            get => _ef;
            set
            {
                _ef = value;
                NotifyPropertyChanged(nameof(EF));
            }
        }

        public RateOfOccurrence RateOfOccurrence
        {
            get => _rateOfOccurrence;
            set
            {
                if (_rateOfOccurrence != null)
                {
                    _rateOfOccurrence.PropertyChanged -= RateOfOccurrence_PropertyChanged;
                }

                _rateOfOccurrence = value;

                if (_rateOfOccurrence != null)
                {
                    _rateOfOccurrence.PropertyChanged += RateOfOccurrence_PropertyChanged;
                }

                NotifyPropertyChanged(nameof(RateOfOccurrence));
                CalculateAro();
            }
        }


        public double ARO
        {
            get => _aro;
            set
            {
                _aro = value;
                NotifyPropertyChanged(nameof(ARO));
                CalculateAle();
            }
        }

        public double ALE
        {
            get => _ale;
            set
            {
                _ale = value;
                NotifyPropertyChanged(nameof(ALE));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void CalculateAro()
        {
            if (_rateOfOccurrence.Unit == "System.Windows.Controls.ComboBoxItem: лет")
            {
                if (_rateOfOccurrence != null && _rateOfOccurrence.FirstValue != 0 &&
                    _rateOfOccurrence.SecondValue != 0)
                {
                    var percentage = (double)(_rateOfOccurrence.FirstValue * 100) / _rateOfOccurrence.SecondValue;
                    ARO = percentage;
                }
            }
            else
            {
                if (_rateOfOccurrence != null && _rateOfOccurrence.FirstValue!=0 && _rateOfOccurrence.SecondValue!=0)
                {
                    var percentage = (double)(_rateOfOccurrence.FirstValue * 100) / (_rateOfOccurrence.SecondValue/12);

                    ARO = percentage;
                }
            }
           
        }

        private void CalculateAle()
        {
            try
            {
                ALE = (ARO * (EF * SLE)) / 100;
            }
            catch (Exception e)
            {
                ALE = 0;
                Console.WriteLine(e);
                throw;
            }
           
        }

    }

    public class RateOfOccurrence : INotifyPropertyChanged
    {
        private int _firstValue;
        private int _secondValue;
        private string _unit;

        public int FirstValue
        {
            get => _firstValue;
            set
            {
                _firstValue = value;
                NotifyPropertyChanged(nameof(FirstValue));
            }
        }

        public int SecondValue
        {
            get => _secondValue;
            set
            {
                _secondValue = value;
                NotifyPropertyChanged(nameof(SecondValue));
            }
        }

        public string Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                NotifyPropertyChanged(nameof(Unit));

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
