using StreamingSubscriptionTrackerAPI.DTOs;

namespace StreamingSubscriptionTrackerAPI.Services
{
    public interface ISubscriptionService
    {
        //GET
        SubscriptionResponseDTO GetById(long id);
        Task<List<SubscriptionResponseDTO>> GetAll(long? filterByUserId);
        List<SubscriptionResponseDTO> GetSubscriptionFromCategory(long idCategory, long? filterByUserId);

        //POST
        SubscriptionResponseDTO Create(SubscriptionRequestDTO subscription, long userId);

        //PUT
        SubscriptionResponseDTO Update(long id, SubscriptionRequestDTO subscription, long? filterByUserId);

        //DELETE
        void Delete(long id, long? filterByUserId);
    }
}
