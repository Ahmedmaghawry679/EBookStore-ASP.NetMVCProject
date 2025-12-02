using Microsoft.AspNetCore.Identity;

namespace BookStoreMVCUI.Repositories
{
    public class OrderRepository:IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;


        public OrderRepository(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        // Old UserOrders Method
        //public async Task<IEnumerable<Order>> UserOrders(bool getAll=false)
        //{
        //    string userId = GetUserId();
        //    if (string.IsNullOrEmpty(userId))
        //        throw new Exception("User Is Not Logged-In");
        //    var orders = await _dbContext.Orders
        //                    .Include(o => o.OrderStatus)
        //                    .Include(o => o.OrderDetails)
        //                    .ThenInclude(od => od.Book)
        //                    .ThenInclude(b => b.genre)
        //                    .Where(o => o.UserId == userId).ToListAsync();
        //    return orders;
        //}

        public async Task<IEnumerable<Order>> UserOrders(bool getAll = false)
        {
            var orders = _dbContext.Orders
                           .Include(x => x.OrderStatus)
                           .Include(x => x.OrderDetails)
                           .ThenInclude(x => x.Book)
                           .ThenInclude(x => x.genre).AsQueryable();
            if (!getAll)
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                orders = orders.Where(a => a.UserId == userId);
                return await orders.ToListAsync();
            }

            return await orders.ToListAsync();
        }
        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
            return await _dbContext.OrderStatuses.ToListAsync();
        }

        public async Task<Order?> GetOrderById(int id)
        {
            return await _dbContext.Orders.FindAsync(id);
        }

        public async Task ChangeOrderStatus(UpdateOrderStatusModel data)
        {
            var order = await _dbContext.Orders.FindAsync(data.OrderId);
            if (order == null)
            {
                throw new InvalidOperationException($"order withi id:{data.OrderId} does not found");
            }
            order.OrderStatusId = data.OrderStatusId;
            await _dbContext.SaveChangesAsync();
        }



        public async Task TogglePaymentStatus(int orderId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"order withi id:{orderId} does not found");
            }
            order.IsPaid = !order.IsPaid;
            await _dbContext.SaveChangesAsync();
        }


        private string GetUserId()
        {
            var principle = _httpContextAccessor.HttpContext.User; // Claims Principle
            string userId = _userManager.GetUserId(principle);
            return userId;
        }
    }
}
