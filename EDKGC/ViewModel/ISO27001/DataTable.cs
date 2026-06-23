using System.ComponentModel;

namespace EDKGC.ViewModel.ISO27001
{
    public class DataTable : INotifyPropertyChanged
    {
        private string _threatEvent;
        private double _sle;
        private double _ef;
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
            if (e.PropertyName == nameof(RateOfOccurrence.FirstValue) ||
                e.PropertyName == nameof(RateOfOccurrence.SecondValue) ||
                e.PropertyName == nameof(RateOfOccurrence.Unit))
            {
                CalculateAro();
            }
        }

        public string ThreatEvent
        {
            get => _threatEvent;
            set { _threatEvent = value; NotifyPropertyChanged(nameof(ThreatEvent)); }
        }

        public double SLE
        {
            get => _sle;
            set { _sle = value; NotifyPropertyChanged(nameof(SLE)); }
        }

        public double EF
        {
            get
            {
                if (_ef == 0 || _sle == 0) return 0;
                return _ef < _sle ? ((_sle / _ef) / 100) : (_ef / _sle);
            }
            set { _ef = value; NotifyPropertyChanged(nameof(EF)); }
        }

        public RateOfOccurrence RateOfOccurrence
        {
            get => _rateOfOccurrence;
            set
            {
                if (_rateOfOccurrence != null)
                    _rateOfOccurrence.PropertyChanged -= RateOfOccurrence_PropertyChanged;

                _rateOfOccurrence = value;

                if (_rateOfOccurrence != null)
                    _rateOfOccurrence.PropertyChanged += RateOfOccurrence_PropertyChanged;

                NotifyPropertyChanged(nameof(RateOfOccurrence));
                CalculateAro();
            }
        }

        public double ARO
        {
            get => _aro / 100;
            set { _aro = value; NotifyPropertyChanged(nameof(ARO)); CalculateAle(); }
        }

        public double ALE
        {
            get => _ale;
            set { _ale = value; NotifyPropertyChanged(nameof(ALE)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void CalculateAro()
        {
            if (_rateOfOccurrence == null ||
                _rateOfOccurrence.FirstValue == 0 ||
                _rateOfOccurrence.SecondValue == 0)
                return;

            // Unit "лет" (years): ARO = occurrences / years * 100
            // Unit "месяц" (months): convert second value to months first
            bool isYears = _rateOfOccurrence.Unit?.Contains("лет") == true;
            if (isYears)
            {
                ARO = (double)(_rateOfOccurrence.FirstValue * 100) / _rateOfOccurrence.SecondValue;
            }
            else
            {
                ARO = (_rateOfOccurrence.FirstValue * 100) / ((double)_rateOfOccurrence.SecondValue / 12);
            }
        }

        private void CalculateAle()
        {
            ALE = ARO * _ef;
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
            set { _firstValue = value; NotifyPropertyChanged(nameof(FirstValue)); }
        }

        public int SecondValue
        {
            get => _secondValue;
            set { _secondValue = value; NotifyPropertyChanged(nameof(SecondValue)); }
        }

        public string Unit
        {
            get => _unit;
            set { _unit = value; NotifyPropertyChanged(nameof(Unit)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
