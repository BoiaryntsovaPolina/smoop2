using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace StoreApp.Helpers
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        // Метод для сповіщення про зміну властивості
        protected void Raise([CallerMemberName] string? prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        // Зручний метод для встановлення значення і сповіщення
        protected bool Set<T>(ref T field, T value, [CallerMemberName] string? prop = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            Raise(prop);
            return true;
        }
    }
}