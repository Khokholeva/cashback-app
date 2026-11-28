using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using cashback_domain.Entities;

namespace cashback_maui.Views
{
    internal class DiscountCardListItem : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
        public string BackGroundColor => CardEntity.BackGroundColor;
        public IDiscountCardEntity CardEntity;
        public string TextColor => ConvertColorToBWContrast(BackGroundColor);

        public DiscountCardListItem(IDiscountCardEntity cardEntity)
        {
            CardEntity = cardEntity ?? throw new ArgumentNullException(nameof(cardEntity));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string ConvertColorToBWContrast(string color)
        {
            var actualColor = Color.FromArgb(color);

            var yiq = (actualColor.Red * 299 + actualColor.Green * 587 + actualColor.Blue * 114) / 1000;

            return yiq >= 0.5 ? Colors.Black.ToHex() : Colors.White.ToHex();
        }
    }
}
