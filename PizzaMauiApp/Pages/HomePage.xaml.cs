using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaMauiApp.ViewModels;

namespace PizzaMauiApp.Pages;

public partial class HomePage : ContentPage
{
    public HomePage(IDIService diService)
    {
        BindingContext = diService.ResolveViewModel<HomePageViewModel>();
        InitializeComponent();
    }
}