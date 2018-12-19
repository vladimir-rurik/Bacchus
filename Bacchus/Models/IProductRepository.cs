using System.Collections.Generic;
using System.Linq;

namespace Bacchus.Models {

    public interface IProductRepository {

        IQueryable<Product> Products { get; }

		void UpsertProducts( List<Product> products );
    }
}
