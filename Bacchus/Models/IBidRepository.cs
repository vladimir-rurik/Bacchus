using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bacchus.Models
{
	public interface IBidRepository
	{
		void SaveBid( Bid bid );
	}
}
