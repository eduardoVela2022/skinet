using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services;

public class PaymentService(
    IConfiguration config,
    ICartService cartService,
    IGenericRepository<Core.Entities.Product> productRepo,
    IGenericRepository<DeliveryMethod> dmRepo
) : IPaymentService
{
    public async Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId)
    {
        // Gets the api key from the config
        StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];

        // Gets the cart
        var cart = await cartService.GetCartAsync(cartId);

        // Checks if cart exists
        if (cart == null)
            return null;

        // Shipping price is calculated
        var shippingPrice = 0m;

        if (cart.DeliveryMethod.HasValue)
        {
            var deliveryMethod = await dmRepo.GetByIdAsync((int)cart.DeliveryMethod);

            if (deliveryMethod == null)
                return null;

            shippingPrice = deliveryMethod.Price;
        }

        // If the price of the product doesn't match the one of the database, change it
        foreach (var item in cart.Items)
        {
            var productItem = await productRepo.GetByIdAsync(item.ProductId);

            if (productItem == null)
                return null;

            if (item.Price != productItem.Price)
            {
                item.Price = productItem.Price;
            }
        }

        // Payment intent is created
        var service = new PaymentIntentService();
        PaymentIntent? intent = null;

        // If payment intent doesn't exist create one
        if (string.IsNullOrEmpty(cart.PaymentIntentId))
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount =
                    (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100))
                    + (long)shippingPrice * 100,
                Currency = "usd",
                PaymentMethodTypes = ["card"],
            };
            intent = await service.CreateAsync(options);
            cart.PaymentIntentId = intent.Id;
            cart.ClientSecret = intent.ClientSecret;
        }
        // If payment intent does exist update it
        else
        {
            var options = new PaymentIntentUpdateOptions
            {
                Amount =
                    (long)cart.Items.Sum(x => x.Quantity * (x.Price * 100))
                    + (long)shippingPrice * 100,
            };
            intent = await service.UpdateAsync(cart.PaymentIntentId, options);
        }

        // Update cart
        await cartService.SetCartAsync(cart);

        // Return cart
        return cart;
    }
}
