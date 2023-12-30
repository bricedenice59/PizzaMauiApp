using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels;

namespace PizzaMauiApp.Pages;

public partial class CartViewPage : ContentPage
{
    public CartViewPage(IDIService diService)
    {
        BindingContext = diService.ResolveViewModel<CartViewModel>()!;
        InitializeComponent();
    }
}