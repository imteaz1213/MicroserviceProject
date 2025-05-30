using Discount.Grpc.Protos;
using Discount.Grpc.Repository;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        ICouponRepository _couponRepository;
        ILogger<DiscountService> _logger;
        public DiscountService(ICouponRepository couponRepository)
        {
            this._couponRepository = couponRepository;
        }


    }
}
