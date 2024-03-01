using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using PizzaMauiApp.API.Dtos;
using PizzaMauiApp.Messages;
using PizzaMauiApp.Pages;
using PizzaMauiApp.Services;
using PizzaMauiApp.ViewModels.Models;

namespace PizzaMauiApp.ViewModels;

public partial class OrderPageViewModel : ViewModelBase
{
    #region Fields
    private readonly IDialogService _dialogService;
    private readonly IToastService _toastService;
    private readonly IOrderService _orderService;
    private readonly CancellationTokenSource _orderTokenCancellationSource;
    private readonly Guid _orderId;
    private readonly DateTime _orderDateTime;
    private const string CancelOrderStr = "Cancel Order";
    private bool _waitingForUserCancellationConfirmation;
    #endregion
    
    #region Ctor

    public OrderPageViewModel(
        IDialogService dialogService,
        IToastService toastService,
        IOrderService orderService)
    {
        _dialogService = dialogService;
        _toastService = toastService;
        _orderService = orderService;
        
        _orderTokenCancellationSource = new();
        _orderId = Guid.NewGuid();
        _orderDateTime = DateTime.Now;

        TimerTick = CancelOrderStr;
    }
    
    #endregion
    
    #region Properties
    private ObservableCollection<CartPizzaModel>? Items { get; set; } = new();
    [ObservableProperty] private string _timerTick;
    [ObservableProperty] private bool _isGracePeriodOver;
    [ObservableProperty] private double _orderTotal;
    [ObservableProperty] private bool _hasOrderBeenCreated;
    public Guid OrderId => _orderId;
    public string OrderDateTime => _orderDateTime.ToString("f");
    public int GracePeriodInSeconds => 15;

    #endregion
    
    #region Overrides

    public override Task OnNavigatingTo(object? parameter)
    {
        if (parameter is null) return base.OnNavigatingTo(parameter);
        
        Items = parameter as ObservableCollection<CartPizzaModel>;
        if (Items != null) 
            OrderTotal = Items.Sum(x => x.Amount);

        return base.OnNavigatingTo(parameter);
    }

    #endregion
    
    #region Commands
    
    
    [RelayCommand]
    private async Task OnSetGracePeriodTimer()
    {
        bool orderCanceled = false;
        using PeriodicTimer timer = new(TimeSpan.FromSeconds(1));
        int i = 0;
        try
        {
            while (await timer.WaitForNextTickAsync(_orderTokenCancellationSource.Token) && i < GracePeriodInSeconds)
            {
                if (!_waitingForUserCancellationConfirmation)
                {
                    i++;
                    TimerTick = $"{CancelOrderStr} ({GracePeriodInSeconds - i}s)";
                
                    timer.Period = TimeSpan.FromSeconds(1);
                }
                
                while (_waitingForUserCancellationConfirmation)
                {
                    await Task.Delay(250);
                }
            }
        }
        catch (OperationCanceledException)
        {
            orderCanceled = true;
        }
        finally
        {
            if(orderCanceled)
            {
                GoToCartPage();
            }
            else
            {
                IsGracePeriodOver = true;
                await Task.Delay(2000);
                HasOrderBeenCreated = await CreateOrderAsync();
                if (!HasOrderBeenCreated)
                {
                    await _toastService.DisplayToast("Technical error, order could not be created.", ToastDuration.Long);
                    await Task.Delay(2000);
                    GoToCartPage();
                }
            }
        }
    }
    
    [RelayCommand]
    private async Task OnOrderCanceledDuringGracePeriod()
    {
        _waitingForUserCancellationConfirmation = true;
        if (await _dialogService.DisplayConfirm("Confirm cancel order?", "Do you really want to cancel this order? No worries, you will not be charged.",
                "Yes", "No"))
        {
            _waitingForUserCancellationConfirmation = false;
            await _orderTokenCancellationSource.CancelAsync();
        }
        _waitingForUserCancellationConfirmation = false;
    }

    #endregion

    #region Methods

    private void GoToCartPage()
    {
        WeakReferenceMessenger.Default.Send(new ShellRouteMessage(new ShellRoute
        {
            RouteName = nameof(CartViewPage)
        }));
    }
    
    private async Task<bool> CreateOrderAsync()
    {
        var userId = Preferences.Get(PreferencesStorageModel.UserId, String.Empty);
        var orderDto = new OrderDto
        {
            Id = _orderId,
            UserId = userId,
            OrderItems = Items!.Select(item => new OrderItemDto
            {
                ItemId = item.Id, 
                Price = item.Price,
                Quantity = item.Quantity,
                OrderId = _orderId
            }).ToList()
        };

        return await _orderService.CreateOrderAsync(orderDto, CancellationToken.None);
    }
    #endregion
}