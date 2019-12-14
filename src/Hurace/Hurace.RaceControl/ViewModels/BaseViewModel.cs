using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Hurace.RaceControl.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void Set<T>(ref T field, T value, [CallerMemberName]string propertyName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(value, field))
            {
                field = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
