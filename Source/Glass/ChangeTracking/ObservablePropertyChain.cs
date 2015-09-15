namespace Glass.ChangeTracking
{
    using System;
    using System.ComponentModel;
    using System.Reactive.Subjects;

    public class ObservablePropertyChain : PropertyChain, IObservable<object>
    {
        private readonly ISubject<object> subject = new Subject<object>();
        private IDisposable childChanged;
        private object previousValue;

        public ObservablePropertyChain(INotifyPropertyChanged owner, string path) : base(path)
        {            
            Property = new ObservableProperty(owner, PropertyName);

            UpdateChild();

            previousValue = Value;
            ObservableProperty.Subscribe(OnNewPropertyValue);
        }

        private ObservableProperty ObservableProperty => (ObservableProperty) Property;

        public IDisposable Subscribe(IObserver<object> observer)
        {
            return subject.Subscribe(observer);
        }

        private void OnNewPropertyValue(object o)
        {
            UpdateChild();
            PushValue();
        }

        private void PushValue()
        {
            if (PreviousAndNewValuesAreDifferent)
            {
                subject.OnNext(Value);
            }

            previousValue = Value;
        }

        private bool PreviousAndNewValuesAreDifferent => !Equals(previousValue, Value);        

        private void UpdateChild()
        {
            childChanged?.Dispose();

            if (SubPath.Length == 0)
            {
                return;
            }

            if (IsValueChangeNotifier)
            {
                var observablePropertyChain = new ObservablePropertyChain(ValueAsNotifier, SubPath);
                Child = observablePropertyChain;
                childChanged = observablePropertyChain.Subscribe(OnNewChildValue);
            }
            else
            {
                var valueTypePropertyChain = new ValueTypePropertyChain(Property.Value, SubPath);
                Child = valueTypePropertyChain;
            }
        }

        private INotifyPropertyChanged ValueAsNotifier => (INotifyPropertyChanged) Property.Value;
        

        private bool IsValueChangeNotifier => Property.Value is INotifyPropertyChanged;

        private void OnNewChildValue(object o)
        {            
            PushValue();
        }
    }
}