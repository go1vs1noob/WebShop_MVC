using Shop.DataAccess.Data;
using Shop.DataAccess.Repository.IRepository;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        public OrderHeaderRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
			

        }

		public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
		{
			OrderHeader orderFromDb = _context.OrderHeaders.FirstOrDefault(u => u.Id == id);
			orderFromDb.OrderStatus = orderStatus;
			if (!string.IsNullOrEmpty(paymentStatus))
			{
				orderFromDb.PaymentStatus = paymentStatus;
			}
		}

		public void UpdateStripePaymentId(int id, string sessionId, string? paymentIntentId = null)
		{
			OrderHeader orderFromDb = _context.OrderHeaders.FirstOrDefault(u => u.Id == id);
			if (!string.IsNullOrEmpty(sessionId)) {
				// updated when user tries to make a payment
				orderFromDb.SessionId = sessionId;
			}
			if (!string.IsNullOrEmpty(paymentIntentId))
			{
				// updated when user tries to make a payment
				orderFromDb.PaymentIntentId = paymentIntentId;
				orderFromDb.PaymentDate = DateTime.Now;
			}
		}
	}
}
