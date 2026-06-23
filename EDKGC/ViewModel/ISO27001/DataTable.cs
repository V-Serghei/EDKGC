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
            set
            {
                if (value < 0) value = 0;
                _sle = value;
                NotifyPropertyChanged(nameof(SLE));
                CalculateAle();
            }
        }

        public double EF
        {
            get => _ef;
            set
            {
                if (value < 0) value = 0;
                if (value > 1) value = 1;
                _ef = value;
                NotifyPropertyChanged(nameof(EF));
                CalculateAle();
            }
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
            get => _aro;
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
            // Annualized Rate of Occurrence: expected number of events per year.
            bool isYears = _rateOfOccurrence.Unit == "Years" ||
                           _rateOfOccurrence.Unit?.Contains("\u043B\u0435\u0442") == true ||
                           _rateOfOccurrence.Unit?.Contains("year") == true;
            if (isYears)
            {
                ARO = (double)_rateOfOccurrence.FirstValue / _rateOfOccurrence.SecondValue;
            }
            else
            {
                ARO = _rateOfOccurrence.FirstValue / ((double)_rateOfOccurrence.SecondValue / 12);
            }
        }

        private void CalculateAle()
        {
            ALE = SLE * EF * ARO;
        }
    }

    public class RateOfOccurrence : INotifyPropertyChanged
    {
        private int _firstValue;
        private int _secondValue;
        private string _unit;

        public RateOfOccurrence()
        {
            _unit = "Years";
        }

        public int FirstValue
        {
            get => _firstValue;
            set { _firstValue = value < 0 ? 0 : value; NotifyPropertyChanged(nameof(FirstValue)); }
        }

        public int SecondValue
        {
            get => _secondValue;
            set { _secondValue = value < 0 ? 0 : value; NotifyPropertyChanged(nameof(SecondValue)); }
        }

        public string Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                NotifyPropertyChanged(nameof(Unit));
                NotifyPropertyChanged(nameof(UnitText));
            }
        }

        public string UnitText => Unit == "Months" ? "months" : "years";

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
