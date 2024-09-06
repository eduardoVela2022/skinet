using Core.Entities;

namespace Core.Interfaces;

// Payment service interface
public interface IPaymentService
{
    Task<ShoppingCart?> CreateOrUpdatePaymentIntent(string cartId);
}
