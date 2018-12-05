using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;

namespace Core.Toolkit.Collections
{
    public class AsyncObservableCollection<T> : ObservableRangeCollection<T>
    {
        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        public AsyncObservableCollection()
        {
        }

        public AsyncObservableCollection(IEnumerable<T> list) : base(list)
        {
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {

            if (SynchronizationContext.Current == _synchronizationContext)
            {
                RaiseCollectionChanged(e);
            }
            else
            {
                _synchronizationContext.Post(RaiseCollectionChanged, e);
            }
        }

        private void RaiseCollectionChanged(object param)
        {
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs) param);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                RaisePropertyChanged(e);
            }
            else
            {
                _synchronizationContext.Post(RaisePropertyChanged, e);
            }
        }

        private void RaisePropertyChanged(object param)
        {
            base.OnPropertyChanged((PropertyChangedEventArgs) param);
        }

        public void RemoveAll(Predicate<T> predicate)
        {
            CheckReentrancy();

            List<T> itemsToRemove = Items.Where(x => predicate(x)).ToList();
            itemsToRemove.ForEach(item => Items.Remove(item));

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }       
    }
}
