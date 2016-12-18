using System;
using Xamarin.Forms;

namespace Ta7meel
{
	public class ItemTappedBehaviour:Behavior<ListView>
	{
		static readonly BindablePropertyKey isItemTapped =
		BindableProperty.CreateReadOnly("IsItemTapped", typeof(bool),typeof(ItemTappedBehaviour), false);
		public static readonly BindableProperty IsItemTappedProperty = isItemTapped.BindableProperty;
			  
		public bool IsItemTapped
		{
			private set { SetValue(isItemTapped, value); }
			get { return (bool)GetValue(IsItemTappedProperty); }
		}

		protected override void OnAttachedTo(ListView bindable)
		{
			base.OnAttachedTo(bindable);
			bindable.ItemTapped += this.OnItemTapped;

		}
		protected override void OnDetachingFrom(ListView bindable)
		{
			base.OnDetachingFrom(bindable);
			bindable.ItemTapped -= this.OnItemTapped;

		}

		public void OnItemTapped(object sender, EventArgs args)
		{
			
		}


	}
}
