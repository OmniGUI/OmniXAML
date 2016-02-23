namespace Glass.ChangeTracking
{
    using System;
    using System.ComponentModel;
    using System.Reactive.Linq;

    public class ObservableProperty : Property, IObservable<object>
    {
        public ObservableProperty(INotifyPropertyChanged owner, string propertyName) : base(owner, propertyName)
        {                      
        }

        private INotifyPropertyChanged ChangeSource => (INotifyPropertyChanged)Owner;

        public IDisposable Subscribe(IObserver<object> observer)
        {
            return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                p => ChangeSource.PropertyChanged += p,
                p => ChangeSource.PropertyChanged -= p)
                .Where(pattern => pattern.EventArgs.PropertyName == PropertyName)
                .Select(_ => Value)
                .Distinct()
                .Subscribe(observer);
        }
    }
}