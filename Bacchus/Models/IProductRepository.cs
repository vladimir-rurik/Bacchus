using System.Linq;

namespace Bacchus.Models {

    public interface IProductRepository {

        IQueryable<Product> Products { get; }
    }
}
