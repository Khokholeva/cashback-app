using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace cashback_maui.Views.DiscountCardListing
{
    internal class DiscountCardListViewModel
    {
        public ObservableCollection<DiscountCardListItem> DiscountCards;

        public DiscountCardListViewModel()
        {
            DiscountCards = new ObservableCollection<DiscountCardListItem>();
        }
    }
}
