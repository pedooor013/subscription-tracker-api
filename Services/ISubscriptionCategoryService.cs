using StreamingSubscriptionTrackerAPI.DTOs;

namespace StreamingSubscriptionTrackerAPI.Services
{
    public interface ISubscriptionCategoryService
    {
        //GET
        SubscriptionCategoryResponseDTO GetById(long id);
        SubscriptionCategoryResponseDTO GetByName(string name);
        List<SubscriptionCategoryResponseDTO> GetAll();
        bool ExistsByName(string name);
        //POST
        SubscriptionCategoryResponseDTO Create(SubscriptionCategoryRequestDTO subscriptionCategory, long userId);
        //PUT
        SubscriptionCategoryResponseDTO Update(long id, SubscriptionCategoryRequestDTO subscriptionCategory);
        //DELETE
        void Delete(long id);

    }
}
