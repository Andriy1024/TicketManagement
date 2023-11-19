﻿using TMS.Common.Errors;
using TMS.Common.Users;

using TMS.Ticketing.Domain.Events;
using TMS.Ticketing.Domain.Ordering;
using TMS.Ticketing.Domain.Ordeting;

namespace TMS.Ticketing.Application.UseCases.Carts;

public sealed class AddItemToCartCommand : IRequest<CartDetailsDto>
{
    public Guid CartId { get; set; }

    public Guid EventId { get; set; }

    public Guid SeatId { get; set; }

    public Guid PriceId { get; set; }
}

internal class AddItemToCartHandler : IRequestHandler<AddItemToCartCommand, CartDetailsDto>
{
    private readonly ICartsRepository _cartRepo;
    private readonly IEventsRepository _eventsRepo;
    private readonly IUserContext _userContext;

    public AddItemToCartHandler(
        ICartsRepository cartRepo,
        IEventsRepository eventsRepo, 
        IUserContext userContext)
    {
        this._cartRepo = cartRepo;
        this._eventsRepo = eventsRepo;
        this._userContext = userContext;
    }

    public async Task<CartDetailsDto> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _cartRepo.GetRequiredAsync(request.CartId);

        if (cart == null)
        {
            cart = new CartEntity
            {
                Id = request.CartId,
                AccountId = _userContext.GetUser().Id
            };

            await _cartRepo.AddAsync(cart);
        }

        var @event = await _eventsRepo.GetAsync(request.EventId);

        if (@event == null) throw AppError
            .NotFound("Event was not found")
            .ToException();

        var seat = @event.Seats.Find(x => x.SeatId == request.SeatId);
        var price = @event.Prices.Find(x => x.Id == request.PriceId);
        var offer = @event.Offers.Find(x => x.SeatId == request.SeatId && x.PriceId == request.PriceId);

        AppError? validationError = (seat, price, offer) switch
        {
            { seat: null } => AppError.NotFound("Seat was not found"),
            { price: null } => AppError.NotFound("Price was not found"),
            { offer: null } => AppError.NotFound("Offer was not found"),
            { seat.State: not SeatState.Available } => AppError.NotFound("Seat is not available"),
            _ => null
        };

        if (validationError != null) throw validationError.ToException();

        var orderItem = new OrderItem
        {
            EventId = @event.Id,
            SeatId = seat!.SeatId,
            PriceId = price!.Id,
            Amount = price.Amount
        };

        cart.OrderItems.Add(orderItem);

        await _cartRepo.UpdateAsync(cart);

        return new CartDetailsDto();
    }
}