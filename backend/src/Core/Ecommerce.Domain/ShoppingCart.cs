using Ecommerce.Domain.Common;

namespace Ecommerce.Domain;

public class ShoppingCart : BaseDomainModel {

    public Guid? ShoppingCartMasterId { get; set; }
    public virtual ICollection<ShoppingCartItem>? ShoppingCartItems { get; set; }
}